﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using System;
using System.Linq;

public class PauseMenu : MonoBehaviour {

	// 選択中の項目
	private IPauseItem _item = null;

	// Tweenに使用する変数
	private Vector3 _startPos;
	private Vector3 _startRot;
	private Vector3 _goalPos = new Vector3(-6.0f, 1.0f, -20.0f);
	private Vector3 _goalRot = new Vector3(0.0f, -75.0f, 0.0f);
	private float _duration = 1.0f;

	private Tweener _tweener;

	// マウス座標
	private Vector3ReactiveProperty _mousePosition = new Vector3ReactiveProperty();
	// 入力方向
	private IntReactiveProperty _inputDirection = new IntReactiveProperty();
	// 入力のしきい値
	private float _threshold = 0.9f;
	// 連続入力を許容する時間
	[SerializeField, Range(0.0f, 1.0f)]
	private float _throttleSeconds = 0.1f;
	// 項目の数
	private int _itemNum;

	// 選択中の項目番号
	[SerializeField]
	private int _selectNum = 0;
	private int SelectNum{
		set{ _selectNum = value;
			if(_selectNum < 0){
				_selectNum = _itemNum - 1;
			}
			else if(_selectNum >= _itemNum){
				_selectNum = 0;
			}}
		get{ return _selectNum; }
	}

	void Start () {
		// 項目数
		_itemNum = GetComponentsInChildren<IPauseItem>().Length;
		// 移動前のカメラ座標を記録
		_startPos = Camera.main.transform.position;
		// アニメーション情報
        _tweener = Camera.main.transform.DOMove(Pausable.Instance.pausing ? _goalPos : _startPos, _duration);

        // カメラを規定の座標に移動させる
        this.ObserveEveryValueChanged(x => Pausable.Instance.pausing)
            .Where(_ => GucchiCS.StageManager.Instance.IsPlay)
			.Subscribe(x => {
                // 停止中で再生中でないなら基点の更新
                if (x && !_tweener.IsPlaying())
                {
                    _startPos = Camera.main.transform.position;
                    _startRot = Camera.main.transform.rotation.eulerAngles;
                }
                _tweener = Camera.main.transform.DOMove(x ? _goalPos : _startPos, _duration);
                Camera.main.transform.DORotate(x ? _goalRot : _startRot, _duration);
            });

		// マウスカーソル移動を取得
		this.UpdateAsObservable()
			.Select(_ => Input.mousePosition)
			.Subscribe(x => _mousePosition.SetValueAndForceNotify(x));

		// 垂直方向の入力をIntに変換する
		this.UpdateAsObservable()
            .Where(_ => Pausable.Instance.pausing)
            .Where(_ => !GucchiCS.ControlState.Instance.IsStateMouse)
            .Select(_ => {
				int result = 0;
				result += Input.GetKeyDown(KeyCode.W) ? 1 : 0;
				result += Input.GetKeyDown(KeyCode.S) ? -1 : 0;
				return -result;
			})
			.Subscribe(x => _inputDirection.SetValueAndForceNotify(x));

		// 入力方向が変更されたら選択項目を移動する
		_inputDirection
			.Subscribe(x => {
				SelectNum += x;
			});

		// 決定された時の処理
		this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Space))
			.Where(_ => _item != null)
			.Where(_ => Pausable.Instance.pausing)
			.Subscribe(_ => {
                // 対象のアイテムのアクションを実行
                _item.OnClick();
			});

		this.UpdateAsObservable()
			.Where(_ => Input.GetMouseButtonDown(0))
			.Where(_ => _item != null)
			.Where(_ => Pausable.Instance.pausing)
			.Subscribe(_ => {
				Ray ray = Camera.main.ScreenPointToRay(_mousePosition.Value);
				// 衝突したオブジェクトを全て取得
				// TODO: レイの長さを制限する
				RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);

				foreach(var obj in hits){
					// 対象のコンポーネントを特定
					var component = obj.transform.GetComponent<IPauseItem>();
					if(component != null){
						component.OnClick();
						break;
					}
				}
			});


		// マウス座標が変更された時にレイを飛ばす
		_mousePosition
			// ポーズされている
			.Where(_ => Pausable.Instance.pausing)
            .Where(_ => GucchiCS.ControlState.Instance.IsStateMouse)
			.Subscribe(x => {
				// スクリーン上のマウス座標からのレイ
				Ray ray = Camera.main.ScreenPointToRay(x);
				// 衝突したオブジェクトを全て取得
				// TODO: レイの長さを制限する
				RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);

				foreach(var obj in hits){
					// 対象のコンポーネントを特定
					var component = obj.transform.GetComponent<IPauseItem>();
					if(component != null && component != _item){
						component.OnEnter();

						// ポーズメニューを持ったオブジェクトの番号を選択中にする
						var trans = transform.GetComponentsInChildren<Transform>()
							.FirstOrDefault(t => t.GetComponent<IPauseItem>() == component);
						SelectNum = trans.GetSiblingIndex();

						if(_item != null){
							_item.OnExit();
						}
						_item = component;
					}
				}
			});

		// 選択項目が変更されたら
		this.ObserveEveryValueChanged(x => _selectNum)
			.Subscribe(x => {
				if(_item != null){
					_item.OnExit();
				}
				_item = GetComponentsInChildren<IPauseItem>()[x];
				_item.OnEnter();
			});
	}
}

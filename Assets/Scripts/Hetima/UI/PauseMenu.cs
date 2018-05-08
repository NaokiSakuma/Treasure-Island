using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

public class PauseMenu : MonoBehaviour {

	private IPauseItem _item = null;

	private Vector3 _goalPos = new Vector3(-6.0f, 1.0f, -20.0f);
	private Vector3 _goalRot = new Vector3(0.0f, -75.0f, 0.0f);
	private float _duration = 1.0f;

	private Vector3 _startPos;

	private Tweener _tweener;

	void Start () {

		_startPos = Camera.main.transform.position;

		// カメラを規定の座標に移動させる
		this.ObserveEveryValueChanged(x => Pausable.Instance.pausing)
			.Subscribe(x => {
				if(x && !_tweener.IsPlaying()){
					_startPos = Camera.main.transform.position;
				}
				_tweener = Camera.main.transform.DOMove(x ? _goalPos : _startPos, _duration);
				Camera.main.transform.DORotate(x ? _goalRot : Vector3.zero, _duration);
			});

		// レイを飛ばして判定を取る
		this.UpdateAsObservable()
			// ポーズされている
			.Where(_ => Pausable.Instance.pausing)
			.Subscribe(_ =>{
				// スクリーン上のマウス座標からのレイ
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				// 衝突したオブジェクトを全て取得
				// TODO: レイの長さを制限する
				RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);

				foreach(var obj in hits){
					// 対象のコンポーネントを特定
					var component = obj.transform.GetComponent<IPauseItem>();
					if(component != null && component != _item){
						component.OnEnter();
						if(_item != null){
							_item.OnExit();
						}
						_item = component;
					}
				}
			});

		// クリックされた時の処理
		this.UpdateAsObservable()
			.Where(_ => Input.GetMouseButtonDown(0))
			.Where(_ => _item != null)
			.Subscribe(_ => {
				// 対象のアイテムのアクションを実行
				_item.OnClick();
			});
	}
}

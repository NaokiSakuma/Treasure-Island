using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using System;

public class ModeIcons : MonoBehaviour {

	[SerializeField, Tooltip("キャラ操作アイコン")]
	private Sprite _characterIcon;

	[SerializeField, Tooltip("オブジェクト操作アイコン")]
	private Sprite _objectIcon;

	// リール画像
	private Image _reel;
	// リール回転
	private Sequence _reelRotater;
	// 回転に掛ける時間
	[SerializeField, Range(0.0f, 5.0f)]
	private float _rotateDuration;

	// 操作モード
	public enum Mode{
		None,
		Character,
		Object,
	}

	// 現在の操作モード
	// TODO: どこか他の場所で設定し、それを取得するようにする
	[SerializeField]
	private Mode _currentMode;

	void Start () {
		_reel = gameObject.GetComponentInChildrenWithoutSelf<Mask>().gameObject.GetComponent<Image>();
		var image = _reel.gameObject.GetComponentInChildrenWithoutSelf<Image>();
		var target = GetComponent<Button>();
		var skipButton = transform.parent.GetComponentInChildren<GucchiCS.StartSkip>().gameObject;

		// ポーズによって画像の表示非表示を切り替える
		this.ObserveEveryValueChanged(x => Pausable.Instance.pausing)
			.Subscribe(x => {
				SetActiveImages(!x);
			});

		// モードが変更されたら画像を差し替える
		this.ObserveEveryValueChanged(x => _currentMode)
			.Subscribe(mode => {
				CreateSeqence(image, mode);
			});

		// スキップボタンが破壊されたら表示する
		skipButton.OnDestroyAsObservable()
			.Subscribe(_ => {
				SetActiveImages(true);
                var instance = GameOverManager.Instance;
                if (instance) GameOverManager.Instance.AddBlinkObject(gameObject);
                //try
                //{
                //    GameOverManager.Instance.AddBlinkObject(gameObject);
                //}
                //catch (NullReferenceException ex) { print("myLight was not set in the inspector"); }
            });

		// モード切替
		target.OnPointerClickAsObservable()
            // オブジェクトと重なっていなかったら
            .Where(_ => !RotateManager.Instance.IsMouseRayHit())
            .Subscribe(_ => {
				// プレイ状態ではないとき、ポーズ中、モード切り替え中、オブジェクト回転中は処理しない
				if(!GucchiCS.StageManager.Instance.IsPlay || Pausable.Instance.pausing || GucchiCS.ModeChanger.Instance.IsChanging || GucchiCS.ModeChanger.Instance.IsRotate){
					return;
				}

				var mc = GucchiCS.ModeChanger.Instance;
				// プレイヤー操作モードもしくはオブジェクト選択モードなら切り替える
				if(mc.Mode == GucchiCS.ModeChanger.MODE.GAME || mc.Mode == GucchiCS.ModeChanger.MODE.OBJECT_CONTROL || mc.Mode == GucchiCS.ModeChanger.MODE.OBJECT_CONTROL_SELECTED){
					mc.Mode = mc.Mode == GucchiCS.ModeChanger.MODE.GAME ? GucchiCS.ModeChanger.MODE.OBJECT_CONTROL : GucchiCS.ModeChanger.MODE.GAME;
				}
			});

		// 最初は非表示にしておく
		SetActiveImages(false);

		_currentMode = Mode.Character;
	}

	void SetActiveImages(bool flg){
		foreach (var item in GetComponentsInChildren<Image>()){
			Color color = item.color;
			color.a = flg ? 1.0f : 0.0f;
			item.color = color;
		}
	}

	void CreateSeqence(Image image, Mode mode){
		if(mode == Mode.None){
			return;
		}
		if(_reelRotater != null &&  _reelRotater.IsPlaying()){
			_reelRotater.Kill();
		}
		_reelRotater = DOTween.Sequence()
			// 半回転と読み込み
			.Append(transform.DORotate(new Vector3(0.0f,0.0f,-180.0f), _rotateDuration * 0.5f)
				.SetEase(Ease.InCubic)
				.OnComplete(() =>  IconChange(image, mode)))
			// 元の角度へ
			.Append(transform.DORotate(new Vector3(0.0f,0.0f,0.0f), _rotateDuration * 0.5f)
				.SetEase(Ease.OutBack));
	}

	void IconChange(Image image, Mode mode){
		switch(mode){
			case Mode.Character:
				image.sprite = _characterIcon;
				break;
			case Mode.Object:
				image.sprite = _objectIcon;
				break;
		}
	}

    /* プロパティ */

    public Mode CurrentMode
    {
        get { return _currentMode; }
        set { _currentMode = value; }
    }
}

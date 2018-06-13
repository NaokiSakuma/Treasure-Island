using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class ModeIcons : MonoBehaviour {

	[SerializeField, Tooltip("キャラ操作アイコン")]
	private Sprite _characterIcon;

	[SerializeField, Tooltip("オブジェクト操作アイコン")]
	private Sprite _objectIcon;

	// 監視対象のボタン
	private GameObject _skipButton;

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
		var image = gameObject.GetComponentInChildrenWithoutSelf<Image>();
		var frame = GetComponent<Image>();
		var skipButton = transform.parent.GetComponentInChildren<GucchiCS.StartSkip>().gameObject;

		// ポーズによって画像の表示非表示を切り替える
		this.ObserveEveryValueChanged(x => Pausable.Instance.pausing)
			.Subscribe(x => {
				SetActiveImages(!x);
			});

		// モードが変更されたら画像を差し替える
		this.ObserveEveryValueChanged(x => _currentMode)
			.Subscribe(mode => {
				IconChange(image, mode);
			});

		// スキップボタンが破壊されたら表示する
		skipButton.OnDestroyAsObservable()
			.Subscribe(_ => {
				SetActiveImages(true);
                GameOverManager.Instance.AddBlinkObject(gameObject);
			});

		// モード切替
		frame.OnPointerClickAsObservable()
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

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

		this.ObserveEveryValueChanged(x => Pausable.Instance.pausing)
			.Subscribe(x => {
				SetActiveImages(x);
			});

		this.ObserveEveryValueChanged(x => _currentMode)
			.Subscribe(mode => {
				IconChange(image, mode);
			});

		_currentMode = Mode.Character;
	}

	void SetActiveImages(bool flg){
		foreach (var item in GetComponentsInChildren<Image>()){
			Color color = item.color;
			color.a = flg ? 0.0f : 1.0f;
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

	/// <summary>
	/// モード切り替え
	/// </summary>
	public void SwitchMode(){
		// プレイ状態ではないとき、ポーズ中、モード切り替え中、オブジェクト回転中は処理しない
		if(!GucchiCS.StageManager.Instance.IsPlay || Pausable.Instance.pausing || GucchiCS.ModeChanger.Instance.IsChanging || GucchiCS.ModeChanger.Instance.IsRotate){
			return;
		}
		var mc = GucchiCS.ModeChanger.Instance;
		// プレイヤー操作モードもしくはオブジェクト選択モードなら切り替える
		if(mc.Mode == GucchiCS.ModeChanger.MODE.GAME || mc.Mode == GucchiCS.ModeChanger.MODE.OBJECT_CONTROL || mc.Mode == GucchiCS.ModeChanger.MODE.OBJECT_CONTROL_SELECTED){
			mc.Mode = mc.Mode == GucchiCS.ModeChanger.MODE.GAME ? GucchiCS.ModeChanger.MODE.OBJECT_CONTROL : GucchiCS.ModeChanger.MODE.GAME;
		}
	}

    /* プロパティ */

    public Mode CurrentMode
    {
        get { return _currentMode; }
        set { _currentMode = value; }
    }
}

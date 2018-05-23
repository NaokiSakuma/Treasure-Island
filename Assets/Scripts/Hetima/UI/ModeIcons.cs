using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ModeIcons : MonoBehaviour {

	[SerializeField, Tooltip("キャラ操作アイコン")]
	private Sprite _characterIcon;

	[SerializeField, Tooltip("オブジェクト操作アイコン")]
	private Sprite _objectIcon;

	[SerializeField, Tooltip("ライト操作アイコン")]
	private Sprite _lightIcon;

	// 操作モード
	public enum Mode{
		None,
		Character,
		Object,
		Light
	}

	// 現在の操作モード
	// TODO: どこか他の場所で設定し、それを取得するようにする
	[SerializeField]
	private Mode _currentMode = Mode.Character;

	void Start () {
		var image = GetComponentInChildren<Image>();

		this.ObserveEveryValueChanged(x => _currentMode)
			.Subscribe(mode => {
				IconChange(image, mode);
			});
	}

	void IconChange(Image image, Mode mode){
		switch(mode){
			case Mode.Character:
				image.sprite = _characterIcon;
				break;
			case Mode.Object:
				image.sprite = _objectIcon;
				break;
			case Mode.Light:
				image.sprite = _lightIcon;
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

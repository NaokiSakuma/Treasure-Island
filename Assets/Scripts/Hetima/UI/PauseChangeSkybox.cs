using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseChangeSkybox : MonoBehaviour {

	// 仮で色を設定しているのでインスペクタで調整したほうがいいかも
	[SerializeField]
	private Color _color = new Color(203,207,200,255);

	void Start () {
		var cam = Camera.main;
		cam.clearFlags = CameraClearFlags.SolidColor;
		cam.backgroundColor = _color;
	}
}

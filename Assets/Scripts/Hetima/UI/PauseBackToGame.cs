using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBackToGame : SimplePauseItem {

	[SerializeField]
	private string _text = "ゲームに戻る";

	void Start () {
		GetComponentInChildren<TextMesh>().text = _text;
	}

	public override void OnClick(){
		Pausable.Instance.pausing = false;
	}
}

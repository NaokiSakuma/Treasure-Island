using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseStageNumber : MonoBehaviour {

	private TextMesh _textMesh;
	private string _text = "Stage : ";
	private int _stageNumber = 0;

	public int StageNumber{
		get{ return _stageNumber; }
		set{ _stageNumber = value;
			 _textMesh.text = _text + _stageNumber; }
	}

	void Start () {
		_textMesh = GetComponent<TextMesh>();
		StageNumber = GucchiCS.StageManager.Instance.StageNo;
	}
}

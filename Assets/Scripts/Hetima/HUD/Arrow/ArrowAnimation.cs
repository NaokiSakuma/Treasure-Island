using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ArrowAnimation : MonoBehaviour {

	[SerializeField]
	MoveMap _moveMap;

	[SerializeField]
	private Image[] _arrows;

	// Use this for initialization
	void Start () {
		var mm = _moveMap;

		_arrows = GetComponentsInChildren<Image>();

		//foreach(var item in _arrows){
		//	this.ObserveEveryValueChanged(x => mm.MoveDirection)
		//		.Where(move => move.sqrMagnitude > 0.0f)
		//	    .Where(_ => item.transform.localScale.x <= 1.5f)
		//		.Subscribe(move => {
		//			item.rectTransform.localScale += Vector3.one * Time.deltaTime;
		//		});	
		//}

		this.ObserveEveryValueChanged(x => mm.MoveDirection)
				.Where(move => move.x <= 0.0f)
		   		.Where(_ => _arrows[0].transform.localScale.x >= 1.0f)
				.Subscribe(move => {
					Debug.Log(_arrows[0].transform.localScale);
					_arrows[0].transform.localScale -= Vector3.one * Time.deltaTime;
				});	
	}
}

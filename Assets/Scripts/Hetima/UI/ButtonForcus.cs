using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;

public class ButtonForcus : MonoBehaviour {

	[SerializeField]
	private int _forcusNum;

	void Start() {
		// フォーカスが外れたらフォーカスをn番目の子にする
		this.ObserveEveryValueChanged(x => EventSystem.current.currentSelectedGameObject)
			.Where(x => x == null)
			.Subscribe(_ => {
				EventSystem.current.SetSelectedGameObject(transform.GetChild(_forcusNum).gameObject);
			});
	}
}

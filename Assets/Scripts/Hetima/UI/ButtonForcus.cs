using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UniRx;

public class ButtonForcus : MonoBehaviour {

	// 直近のターゲット
	private GameObject _recentTarget;

	void Start() {
		// 最初のターゲットは一番近いボタン
		_recentTarget = transform.GetComponentInChildren<Button>().gameObject;

		// フォーカス先が変更された時に直近のターゲットにフォーカス先オブジェクトを保存する
		this.ObserveEveryValueChanged(x => EventSystem.current.currentSelectedGameObject)
			.Where(x => x != null)
			.Subscribe(obj => {
				_recentTarget = obj;
			});

		// フォーカスが外れたらフォーカスを直近のターゲットにする
		this.ObserveEveryValueChanged(x => EventSystem.current.currentSelectedGameObject)
			.Where(x => x == null)
			.ThrottleFrame(5)
			.Subscribe(_ => {
				EventSystem.current.SetSelectedGameObject(_recentTarget);
			});
	}
}

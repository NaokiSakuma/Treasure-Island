using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PausableController : MonoBehaviour {

	void Start () {
		// Escape押下でポーズ切り替え
		this.UpdateAsObservable()
			.Where(_ => Input.GetKeyDown(KeyCode.Escape))
			.Subscribe(_ =>{
				Pausable.Instance.pausing = !Pausable.Instance.pausing;
			});
	}
}

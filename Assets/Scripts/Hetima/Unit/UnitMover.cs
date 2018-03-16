using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

[RequireComponent(typeof(UnitCore))]
public class UnitMover : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var core = GetComponent<UnitCore>();

		// TODO: 暫定的に味方は右、敵は左に向けている
		this.UpdateAsObservable()
			.First()
			.Subscribe(_ => {
				if (core.Team == 0) {
					transform.forward = transform.right;
				}
				else if(core.Team == 1) {
					transform.forward = -transform.right;
				}
				transform.DOLocalMoveX(transform.forward.x * -1.0f, 1.5f);
			});
	}
}

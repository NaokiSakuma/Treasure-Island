using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

[RequireComponent(typeof(UnitCore))]
public class UnitMover : MonoBehaviour {

	UnitCore _core;

	// 近づく限界距離
	[SerializeField]
	private float _limitDistance = 2.0f;

	private Vector3 _velocity = Vector3.zero;

	// 移動中か
	private BoolReactiveProperty _isMoving = new BoolReactiveProperty(false);
	public BoolReactiveProperty IsMoving{
		get { return _isMoving; }
	}

	// Use this for initialization
	void Start() {
		_core = GetComponent<UnitCore>();

		// 移動中かどうかを更新する
		this.UpdateAsObservable()
		    // TODO: 比較が大丈夫なのかちょっと不安
			.Select(_ => _velocity != Vector3.zero)
			.Subscribe(x => _isMoving.SetValueAndForceNotify(x));

		// 移動
		this.UpdateAsObservable()
		    .Where(_ => _isMoving.Value)
			.Subscribe(_ => {
				// 移動
				transform.position += _velocity;
			});
	}

	/// <summary>
	/// 近すぎないかを返す
	/// </summary>
	/// <returns><c>true</c>, if too near was checked, <c>false</c> otherwise.</returns>
	bool IsTooNear() {
		return _limitDistance < Vector3.Distance(transform.position, _core.Target.transform.position);
	}

	/// <summary>
	/// 一番近くの敵に向かって移動する
	/// </summary>
	public void MoveToNearestEnemy() {
		// ターゲットのほうを向く
		transform.LookAt(_core.Target.transform.position);
		if(IsTooNear()){
			_velocity = transform.forward * _core.MoveSpeed * Time.deltaTime;
		}
		else{
			_velocity = Vector3.zero;
		}
	}
}
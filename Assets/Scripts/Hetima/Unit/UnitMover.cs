using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

[RequireComponent(typeof(UnitCore))]
public class UnitMover : MonoBehaviour {

	// TODO: 正しい値に変更
	[SerializeField]
	private float _moveSpeed = 2.0f;
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

		this.UpdateAsObservable()
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
			_velocity = transform.forward * _moveSpeed * Time.deltaTime;
		}
		else{
			_velocity = Vector3.zero;
		}
	}
}
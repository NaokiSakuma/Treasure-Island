using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(UnitCore))]
public class UnitAttacker : MonoBehaviour {

	UnitCore _core;

	// Use this for initialization
	void Start () {
		_core = GetComponent<UnitCore>();

		// 攻撃
		this.UpdateAsObservable()
		    // 自分が生きていて
		    .Where(_ => _core.Health > 0)
		    // ターゲットがいる
		    .Where(_ => _core.Target)
		    // ターゲットが攻撃の届く距離にいる
		    .Where(_ => Vector3.Distance(transform.position, _core.Target.transform.position) < _core.AttackReach)
		    // 一定間隔で
			.ThrottleFirst(System.TimeSpan.FromSeconds(_core.AttackSpeed))
		    // 攻撃
			.Subscribe(_ => {
				Attack();
			});
	}

	void Attack(){
		// TODO: アニメーションの追加もしくは連動
		// 対象にダメージ
		_core.Target.Health -= _core.Strength + _core.AdditionalStrength;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(UnitCore))]
public class UnitAttacker : MonoBehaviour {

	UnitCore _core;

    // 拠点の位置
    private Vector3 _basePos = Vector3.zero;

    // Use this for initialization
    void Start () {
		_core = GetComponent<UnitCore>();

        // inspecterでやった方がいいかもしれん　baseが動く可能性有なら修正しないと unitcoreに持たせるワンチャン
        _basePos = GameObject.Find("base").transform.position;

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

        this.UpdateAsObservable()
            // 自分が生きていて
            .Where(_ => _core.Health > 0)
            // ターゲットがいない
            .Where(_ => !_core.Target)
            // ターゲットが攻撃の届く距離にいる
            .Where(_ => Vector3.Distance(transform.position, _basePos) < _core.AttackReach)
            // 一定間隔で
            .ThrottleFirst(System.TimeSpan.FromSeconds(_core.AttackSpeed))
            // 攻撃 拠点がわからんからとりあえずコンソールで表示
            .Subscribe(_ => {
                // 処理
                print("attack");
            });

    }

    void Attack(){

        //攻撃SE
        AudioManager.Instance.PlaySE(AUDIO.SE_BATTLE);

        // TODO: アニメーションの追加もしくは連動
        // 対象にダメージ
        _core.Target.Health -= _core.Strength + _core.AdditionalStrength;
	}
}

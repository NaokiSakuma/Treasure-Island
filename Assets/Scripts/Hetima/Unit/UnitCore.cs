using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnitState;
using System;

public class UnitCore : MonoBehaviour {

	// 体力
	private IntReactiveProperty _health = new IntReactiveProperty(1);
	public IntReactiveProperty Health {
		get { return _health; }
		set {
			_health = value;
			if (_health.Value <= 0) {
				_health.Value = 0;
			}
		}
	}

	// 攻撃力
	private int _strength = 1;
	public int Strength{
		get { return _strength; }
	}

	// 所属陣営
	// TODO: ひとまずint
	[SerializeField]
	private int _team = 0;
	public int Team{
		get { return _team; }
	}

	// ステート
	private ReactiveProperty<UnitState.UnitState> _state = new ReactiveProperty<UnitState.UnitState>(new UnitStateDefault());
	public ReactiveProperty<UnitState.UnitState> State{
		get { return _state; }
	}

	private StateProcessor _stateProcessor = new StateProcessor();
	private UnitStateIdle _stateIdle = new UnitStateIdle();
	private UnitStateAttack _stateAttack = new UnitStateAttack();

	// Use this for initialization
	void Start () {
		// ステートの設定
		_stateProcessor.State = _stateIdle;
		_stateIdle.execDelegate = Idle;
		_stateAttack.execDelegate = Attack;

		// ステートの更新
		this.UpdateAsObservable()
		    .Where(_ => _stateProcessor.State != null)
		    .Subscribe(_ => {
				_stateProcessor.Execute();
		});

		// ステートが変わった時
		_state.ObserveEveryValueChanged(_ => true)
		      .Subscribe(_ => {
				// TODO: Enter実装
				//_stateProcessor.Enter();
		});
	}


	public void Idle(){
		// 待機ステート
		Debug.Log(_stateIdle.GetStateName());
		//１秒後に状態遷移
		Observable
			.Timer(TimeSpan.FromSeconds(1))
			.Subscribe(x => _stateProcessor.State = _stateAttack);
	}

	public void Attack(){
		// 待機ステート
		Debug.Log(_stateAttack.GetStateName());
		//１秒後に状態遷移
		Observable
			.Timer(TimeSpan.FromSeconds(1))
			.Subscribe(x => _stateProcessor.State = _stateIdle);
	}

	/// <summary>
	/// 一番近くにいる敵を取得
	/// </summary>
	/// <returns>一番近くにいる敵、いない場合はnull</returns>
	/// <param name="range">索敵範囲</param>
	GameObject GetNearestEnemy(float range) {
		// 一番近い敵
		GameObject nearestEnemy = null;
		// 一番近い距離
		float minDis = range;
		// 範囲内のコライダーのついているオブジェクトを全て探す
		Collider[] targets = Physics.OverlapSphere(transform.position, range);

		foreach (Collider target in targets) {
			// ユニットかどうかを判別する
			var core = target.GetComponent<UnitCore>();
			// ユニットでない
			// 生きている
			// 自分と所属している陣営が違う
			if (core == null || core.Health.Value < 0 || core.Team != this.Team) {
				continue;
			}
			// 対象との距離を求める
			float dis = Vector3.Distance(transform.position, target.transform.position);
			// 最小距離を更新していたら対象を最小距離として
			if (dis < minDis) {
				minDis = dis;
				nearestEnemy = target.gameObject;
			}
		}

		return nearestEnemy;
	}
}

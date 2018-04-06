using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnitState;
using System;

[RequireComponent(typeof(UnitMover))]
public class UnitCore : MonoBehaviour {
	 
	// TODO: データを持つ場所はもう少し考えたほうがいいと思う

	// 最大体力
	[SerializeField]
	private IntReactiveProperty _maxHealth = new IntReactiveProperty(10);
	public IntReactiveProperty MaxHealth{
		get { return _maxHealth; }
	}

	// 体力
	[SerializeField]
	private int _health = 1;
	public int Health {
		get { return _health; }
		set {
			_health = value;
			// HPが負の値にならないようにする
			if (_health < 0) {
				_health = 0;
			}
		}
	}

	// 攻撃力
	[SerializeField]
	private int _strength = 1;
	public int Strength {
		get { return _strength; }
	}

	// 攻撃力のバフ
	[SerializeField]
	private int _additionalStrength = 0;
	// TODO: あとでバフの制限かけるかもしれないので一応プロパティ化
	public int AdditionalStrength{
		get { return _additionalStrength; }
		set { _additionalStrength = value; }

	}

	// 攻撃速度
	[SerializeField]
	private float _attackSpeed = 1.0f;
	public float AttackSpeed{
		get { return _attackSpeed; }
	}

	// 攻撃が届く距離
	[SerializeField]
	private float _attackReach = 2.0f;
	public float AttackReach{
		get { return _attackReach; }
	}

	// 索敵範囲
	[SerializeField]
	private float _searchRange = 10.0f;
	public float SearchRange{
		get { return _searchRange; }
	}

	// 移動速度
	[SerializeField]
	private float _moveSpeed = 0.5f;
	public float MoveSpeed{
		get { return _moveSpeed; }
	}

	// 大きさ
	// TODO:整数か少数か不明なので保留

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
	private UnitStateHolded _stateHolded = new UnitStateHolded();
	private UnitStateAttack _stateAttack = new UnitStateAttack();
	private UnitStateDead _stateDead = new UnitStateDead();

	[SerializeField]
	private UnitCore _target;
	public UnitCore Target{
		get { return _target; }
	}

	// Use this for initialization
	void Start () {
		// HPの初期値は最大HPにしておく
		_health = MaxHealth.Value;

		// 味方はつかめるようにする
		if(_team == 0){
			gameObject.AddComponent<GucchiCS.Unit>();
		}

		// ステートの設定
		_stateProcessor.State = _stateIdle;
		_stateIdle.execDelegate = Idle;
		_stateHolded.execDelegate = Holded;
		_stateAttack.execDelegate = Attack;
		_stateDead.enterDelegate = DeadEnter;

		// ステートの更新
		this.UpdateAsObservable()
		    .Where(_ => _stateProcessor.State != null)
		    .Subscribe(_ => {
				_stateProcessor.Execute();
		});

		// ダメージを受けた時
		this.ObserveEveryValueChanged(_ => Health)
			.Subscribe(x => {
				var textMesh = GetComponentInChildren<TextMesh>();
				textMesh.text = x.ToString() + "/" + MaxHealth.Value.ToString();
			});

		// HPが0になったら死亡ステートに変更
		this.ObserveEveryValueChanged(_ => Health)
		    .Where(x => x <= 0)
			.Subscribe(_ => {
				_stateProcessor.State = _stateDead;
			});
	}

	public void Idle(){
		// 近くにいる敵を探す
		_target = GetNearestEnemy(100.0f);
		// ターゲットがいたら攻撃フェーズに移行
		if(_target != null){
			_stateProcessor.State = _stateAttack;
		}
	}

	public void Holded(){
	}

	public void Attack(){
		// ターゲットが死んでいたら
		if(_target.GetComponent<UnitCore>().Health <= 0){
			// 待機ステートにする
			_stateProcessor.State = _stateIdle;
		}
		else{
			transform.GetComponent<UnitMover>().MoveToNearestEnemy();
		}
	}

	public void DeadEnter() {
		// 死亡ステート
		Debug.Log(_stateDead.GetStateName());
		// 死亡した時につかめなくする
		var unit = GetComponent<GucchiCS.Unit>();
		if(unit){
			Destroy(unit);
		}
	}

	/// <summary>
	/// 一番近くにいる敵を取得
	/// </summary>
	/// <returns>一番近くにいる敵、いない場合はnull</returns>
	/// <param name="range">索敵範囲</param>
	UnitCore GetNearestEnemy(float range) {
		// 一番近い敵
		UnitCore nearestEnemy = null;
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
			if (core == null || core.Health <= 0 || core.Team == this.Team) {
				continue;
			}
			// 対象との距離を求める
			float dis = Vector3.Distance(transform.position, target.transform.position);
			// 最小距離を更新していたら対象を最小距離として
			if (dis < minDis) {
				minDis = dis;
				nearestEnemy = core;
			}
		}

		return nearestEnemy;
	}
}

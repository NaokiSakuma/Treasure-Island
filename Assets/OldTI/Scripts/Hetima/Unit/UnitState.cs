using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitState{
	//ステートの実行を管理するクラス
	public class StateProcessor {
		//ステート本体
		private UnitState _State;
		public UnitState State {
			set {
				if(_State != null){
					_State.Exit();
				}
				_State = value;
				_State.Enter();
			}
			get { return _State; }
		}
		// 入場
		private void Enter() {
			State.Enter();
		}
		// 実行
		public void Execute() {
			State.Execute();
		}
		// 退場
		private void Exit() {
			State.Exit();
		}
	}

	//ステートのクラス
	public abstract class UnitState {
		// デリゲート
		public delegate void enterState();
		public enterState enterDelegate;
		public delegate void executeState();
		public executeState execDelegate;
		public delegate void exitState();
		public exitState exitDelegate;

		// 入場処理
		public virtual void Enter() {
			if (enterDelegate != null) {
				enterDelegate();
			}
		}
		// 実行処理
		public virtual void Execute() {
			if (execDelegate != null) {
				execDelegate();
			}
		}
		// 退場処理
		public virtual void Exit() {
			if (exitDelegate != null) {
				exitDelegate();
			}
		}

		//ステート名を取得するメソッド
		public abstract string GetStateName();
	}

	// 以下状態クラス

	//  DefaultPosition
	public class UnitStateDefault : UnitState {
		public override string GetStateName() {
			return "State:Default";
		}
	}

	/// <summary>
	/// ニュートラルステート
	/// </summary>
	public class UnitStateNeutral : UnitState{
		public override string GetStateName() {
			return "State:Neutral";
		}
	}

	/// <summary>
	/// 待機ステート
	/// </summary>
	public class UnitStateIdle : UnitStateNeutral {
		// TODO: 徘徊ステートになるかも
		public override string GetStateName() {
			return "State:Neutral.Idle";
		}
	}

	/// <summary>
	/// 掴まれている
	/// </summary>
	public class UnitStateHolded : UnitStateNeutral {
		public override string GetStateName() {
			return "State:Neutral.Holded";
		}
	}

	/// <summary>
	/// 敵対ステート
	/// </summary>
	public class UnitStateHostility : UnitState {
		public override string GetStateName() {
			return "State:Hostility";
		}
	}

	/// <summary>
	/// 攻撃ステート
	/// </summary>
	public class UnitStateAttack : UnitStateHostility{
		public override string GetStateName() {
			return "State:Hostility.Attack";
		}
	}

	/// <summary>
	/// 死亡ステート
	/// </summary>
	public class UnitStateDead : UnitState{
		public override string GetStateName() {
			return "State:Dead";
		}
	}
}

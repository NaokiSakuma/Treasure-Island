using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitState{
	//ステートの実行を管理するクラス
	public class StateProcessor {
		//ステート本体
		private UnitState _State;
		public UnitState State {
			set { _State = value; }
			get { return _State; }
		}

		// 実行
		public void Execute() {
			State.Execute();
		}
	}

	//ステートのクラス
	public abstract class UnitState {
		// デリゲート
		public delegate void executeState();
		public executeState execDelegate;

		// 実行処理
		public virtual void Execute() {
			if (execDelegate != null) {
				execDelegate();
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

	////  StateA
	//public class UnitStateA : UnitState {
	//	public override string GetStateName() {
	//		return "State:A";
	//	}
	//}

	////  StateB
	//public class UnitStateB : UnitState {
	//	public override string GetStateName() {
	//		return "State:B";
	//	}

	//	public override void Execute() {
	//		Debug.Log("特別な処理がある場合は子が処理してもよい");
	//		if (execDelegate != null) {
	//			execDelegate();
	//		}
	//	}
	//}


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
}

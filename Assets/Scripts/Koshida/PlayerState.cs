using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerState
{
    //ステートの実行を管理するクラス
    public class StateProcessor
    {
        //ステート本体
        private PlayerState _State;
        public PlayerState State
        {
            set
            {
                if (_State != null)
                {
                    _State.Exit();
                }
                _State = value;
                _State.Enter();
            }
            get { return _State; }
        }
        // 入場
        private void Enter()
        {
            State.Enter();
        }
        // 実行
        public void Execute()
        {
            State.Execute();
        }
        // 退場
        private void Exit()
        {
            State.Exit();
        }
    }

    //ステートのクラス
    public abstract class PlayerState
    {
        // デリゲート
        public delegate void enterState();
        public enterState enterDelegate;
        public delegate void executeState();
        public executeState execDelegate;
        public delegate void exitState();
        public exitState exitDelegate;

        // 入場処理
        public virtual void Enter()
        {
            if (enterDelegate != null)
            {
                enterDelegate();
            }
        }
        // 実行処理
        public virtual void Execute()
        {
            if (execDelegate != null)
            {
                execDelegate();
            }
        }
        // 退場処理
        public virtual void Exit()
        {
            if (exitDelegate != null)
            {
                exitDelegate();
            }
        }

        //ステート名を取得するメソッド
        public abstract string GetStateName();
    }

    // 以下状態クラス

    //  DefaultPosition
    public class PlayerStateDefault : PlayerState
    {
        public override string GetStateName()
        {
            return "State:Default";
        }
    }

    /// <summary>
    /// ニュートラルステート
    /// </summary>
    public class PlayerStateNeutral : PlayerState
    {
        public override string GetStateName()
        {
            return "State:Neutral";
        }
    }

    /// <summary>
    /// 待機ステート
    /// </summary>
    public class PlayerStateIdle : PlayerStateNeutral
    {
        // TODO: 徘徊ステートになるかも
        public override string GetStateName()
        {
            return "State:Neutral.Idle";
        }
    }

    /// <summary>
    /// 死亡ステート
    /// </summary>
    public class PlayerStateDead : PlayerState
    {
        public override string GetStateName()
        {
            return "State:Dead";
        }
    }
}

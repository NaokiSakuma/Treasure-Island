using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Konji
{
    [RequireComponent(typeof(PlayerMove))]
    public class PlayerControl : MonoBehaviour
    {
        //プレイヤー
        private PlayerMove _player;
        //ジャンプ
        private bool _jump;

        //移動方向
        private int _move = 0;

        //死亡フラグ
        private bool _isDead = false;
        public bool IsDead
        {
            get { return _isDead; }
        }

        //衝突チャック
        private CrushChecker[] _crushCheck;

        private void Awake()
        {
            _player = GetComponent<PlayerMove>();
            _crushCheck = GetComponentsInChildren<CrushChecker>();
        }

        void Start()
        {
            //いい書き方がわかりません

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    InputMove(Input.GetKey(KeyCode.D), Input.GetKey(KeyCode.A));
                });

            //挟まれたら死亡
            this.UpdateAsObservable()
                .Where(_ => (_crushCheck[0].IsCrush && _crushCheck[1].IsCrush) || (_crushCheck[2].IsCrush && _crushCheck[3].IsCrush))
                .Take(1)
                .Subscribe(_ =>
                {
                    Dead();
                });

            //プレイヤーの移動(ゲームモードのみ移動可能)
            this.FixedUpdateAsObservable()
                .Where(_ => !_isDead)
                .Where(_ => GucchiCS.ModeChanger.Instance.Mode == GucchiCS.ModeChanger.MODE.GAME)
                .Subscribe(_ =>
                {
                    _player.Move(_move, _jump);
                    _jump = false;
                });
        }

        //死亡処理
        void Dead()
        {
            Debug.Log("死にました～");

            _isDead = true;
        }

        //移動入力
        void InputMove(bool D,bool A)
        {
            //D入力
            if(D)
            {
                _move = 1;
            }
            //A入力
            else if(A)
            {
                _move = -1;
            }
            else
            {
                _move = 0;
            }
        }
    }
}
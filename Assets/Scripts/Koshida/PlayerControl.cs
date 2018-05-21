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

        //移動方向
        private int _move = 0;

        //死亡フラグ
        [SerializeField]
        private bool _isDead = false;
        public bool IsDead
        {
            get { return _isDead; }
        }

        private void Awake()
        {
            _player = GetComponent<PlayerMove>();
        }

        void Start()
        {
            //いい書き方がわかりません

            this.UpdateAsObservable()
                .Where(_ => GucchiCS.ModeChanger.Instance.Mode == GucchiCS.ModeChanger.MODE.GAME)
                .Subscribe(_ =>
                {
                    InputMove(Input.GetKey(KeyCode.D), Input.GetKey(KeyCode.A));
                });

            //モードが変わったら速度0(突貫)
            GucchiCS.ModeChanger.Instance
                .ObserveEveryValueChanged(mode => mode.Mode)
                .Subscribe(_ =>
                {
                    _move = 0;
                });

            //挟まれたら死亡
            this.UpdateAsObservable()
                .Where(_ => (_player.IsNest[0] && _player.IsNest[1]) || (_player.IsNest[2] && _player.IsNest[3]))
                .Take(1)
                .Subscribe(_ =>
                {
                    Dead();
                });

            //プレイヤーの移動(ゲームモードのみ移動可能)
            this.FixedUpdateAsObservable()
                .Where(_ => GucchiCS.GameManagerKakkoKari.Instance.IsPlay)          // ぐっち追記（プレイ中のみ通る）
                .Where(_ => !_isDead)
                .Subscribe(_ =>
                {
                    _player.Move(_move);
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
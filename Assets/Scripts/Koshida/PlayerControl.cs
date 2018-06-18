using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            //プレイヤーの左右移動
            this.UpdateAsObservable()
                .Where(_ => GucchiCS.ModeChanger.Instance.Mode == GucchiCS.ModeChanger.MODE.GAME)
                .Where(_ => !GucchiCS.ModeChanger.Instance.IsChanging)
                .Subscribe(_ =>
                {
                    bool right = Input.GetKey(KeyCode.D) | Input.GetKey(KeyCode.RightArrow);
                    bool left = Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.LeftArrow);

                    InputMove(right, left);
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

            //下の方に落ちたら死亡
            this.UpdateAsObservable()
                .Where(_ => _player.transform.position.y <= -10.0f)
                .Take(1)
                .Subscribe(_ =>
                {
                    Dead();
                });

            //プレイヤーの移動(ゲームモードのみ移動可能)
            this.FixedUpdateAsObservable()
                .Where(_ => GucchiCS.StageManager.Instance.IsPlay)
                .Where(_ => !_isDead)
                .Subscribe(_ =>
                {
                    _player.Move(_move);
                });

            //クリアしたらクリアモーション
            this.ObserveEveryValueChanged(_ => GucchiCS.ModeChanger.Instance.Mode)
                .Where(x => x == GucchiCS.ModeChanger.MODE.CLEAR)
                .Take(1)
                .Subscribe(_ =>
                {
                    _player.ClearMove();
                });
        }

        //死亡処理
        void Dead()
        {
            Debug.Log("死にました～");

            _isDead = true;

            _player.DeadMove();

            // プレイ状態解除
            GucchiCS.StageManager.Instance.GameoverEnter();
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
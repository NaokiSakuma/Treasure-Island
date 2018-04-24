using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Konji
{
    public class PlayerMove : MonoBehaviour
    {
        //移動速度
        [SerializeField]
        private float _maxSpeed = 0.01f;

        //右向き
        private bool _facingRight = true;

        private Rigidbody _rigit;

        //重力
        [SerializeField]
        private Vector3 _localGravity;

        void Awake()
        {
            _rigit = GetComponent<Rigidbody>();
            _rigit.useGravity = false;
        }

        // Use this for initialization
        void Start()
        {
            //重力
            this.FixedUpdateAsObservable()
                .Subscribe(_ =>
                {
                    _rigit.AddForce(_localGravity, ForceMode.Acceleration);
                });
        }

        //移動
        public void Move(float move, bool jump)
        {
            _rigit.velocity = new Vector2(move * _maxSpeed, _rigit.velocity.y);

            //振り返り
            if (move > 0 && !_facingRight)
            {
                Flip();
            }
            else if (move < 0 && _facingRight)
            {
                Flip();
            }
        }

        //振り向く
        private void Flip()
        {
            _facingRight = !_facingRight;

            float rot = transform.localEulerAngles.y;

            rot += 180;

            transform.localEulerAngles = new Vector3(0, rot, 0);
        }

    }
}
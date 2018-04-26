using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Konji
{
    public class PlayerMove2D : MonoBehaviour
    {
        //移動速度
        [SerializeField]
        private float _maxSpeed = 0.01f;

        //右向き
        private bool _facingRight = true;

        private Rigidbody2D _rigit;

        private GetColor _getColor;

        //重力
        [SerializeField]
        private Vector3 _localGravity;

        [SerializeField]
        private Color _ground;

        void Awake()
        {
            _rigit = GetComponent<Rigidbody2D>();
            _getColor = FindObjectOfType<GetColor>();
        }

        // Use this for initialization
        void Start()
        {
            //重力
            this.FixedUpdateAsObservable()
                .Subscribe(_ =>
                {

                });
        }

        //移動
        public void Move(int move)
        {
            _rigit.velocity = new Vector2(move * _maxSpeed, _rigit.velocity.y);

            //下のめり込み排斥
            if (_getColor._groundColor.r <= _ground.r && _getColor._groundColor.g <= _ground.g && _getColor._groundColor.b <= _ground.b)
            {
                transform.position += new Vector3(0, 0.001f, 0);

                _rigit.gravityScale = 0;

                _rigit.velocity = new Vector2(_rigit.velocity.x, 0);
            }
            else
            {
                _rigit.gravityScale = _localGravity.y;
            }

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

            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Konji
{
    public class PlayerMove : MonoBehaviour
    {
        enum NEST
        {
            TOP,
            BOTTOM,
            FRONT,
            BACK,

            NUM
        }

        //移動速度
        [SerializeField]
        private float _maxSpeed = 0.01f;

        //右向き
        private bool _facingRight = true;

        private Rigidbody _rigit;

        //重力
        [SerializeField]
        private Vector3 _localGravity;

        [SerializeField]
        private Transform[] _nestCheck = new Transform[(int)NEST.NUM];

        //めり込みフラグ
        private bool[] _isNest = new bool[(int)NEST.NUM];
        public bool[] IsNest
        {
            get { return _isNest; }
        }

        //アニメーター
        Animator _animator;

        void Awake()
        {
            _rigit = GetComponent<Rigidbody>();
            _rigit.useGravity = false;

            _animator = GetComponent<Animator>();
        }

        // Use this for initialization
        void Start()
        {
        }

        public void Reset(Vector3 pos, Quaternion rot)
        {
            transform.position = pos;
            transform.rotation = rot;

            _facingRight = true;
        }

        //移動
        public void Move(int move)
        {
            //重力初期化
            Vector3 grv = _localGravity;
            _rigit.velocity = new Vector2(move * _maxSpeed, _rigit.velocity.y);
            _animator.SetBool("Ground", false);

            RaycastHit hit;
            LayerMask layermask = 1 << LayerMask.NameToLayer("Shadow");
            int distance = 30;

            //地面判定
            Ray groundRay = new Ray(_nestCheck[(int)NEST.BOTTOM].position + new Vector3(0, 0, distance / 2), new Vector3(0, 0, -1));
            Debug.DrawRay(groundRay.origin, groundRay.direction * distance);
            if (Physics.Raycast(groundRay, out hit, distance, layermask))
            {
                //Y軸の移動制限
                grv = Vector3.zero;
                _rigit.velocity = new Vector2(_rigit.velocity.x, 0);

                _animator.SetBool("Ground", true);
            }

            //下めり込み判定
            Vector3 nestPos = _nestCheck[(int)NEST.BOTTOM].position + new Vector3(0, 0.05f, 0);
            Ray nestRay = new Ray(nestPos + new Vector3(0, 0, distance / 2), new Vector3(0, 0, -1));
            Debug.DrawRay(nestRay.origin, nestRay.direction * distance);
            if (Physics.Raycast(nestRay, out hit, distance, layermask))
            {
                //めり込み排斥
                transform.position += nestPos - _nestCheck[(int)NEST.BOTTOM].position;

                _isNest[(int)NEST.BOTTOM] = true;
            }
            else
            {
                _isNest[(int)NEST.BOTTOM] = false;
            }

            //上めり込み判定
            nestPos = _nestCheck[(int)NEST.TOP].position;
            nestRay = new Ray(nestPos + new Vector3(0, 0, distance / 2), new Vector3(0, 0, -1));
            Debug.DrawRay(nestRay.origin, nestRay.direction * distance);
            if (Physics.Raycast(nestRay, out hit, distance, layermask))
            {
                //めり込み排斥
                transform.position -= nestPos + new Vector3(0, -0.05f, 0) - _nestCheck[(int)NEST.TOP].position;
                _isNest[(int)NEST.TOP] = true;
            }
            else
            {
                _isNest[(int)NEST.TOP] = false;
            }

            //前めり込み判定
            nestPos = _nestCheck[(int)NEST.FRONT].position;
            nestRay = new Ray(nestPos + new Vector3(0, 0, distance / 2), new Vector3(0, 0, -1));
            Debug.DrawRay(nestRay.origin, nestRay.direction * distance);
            if (Physics.Raycast(nestRay, out hit, distance, layermask))
            {
                //めり込み排斥
                transform.position -= nestPos + new Vector3(transform.right.x * 0.05f, 0, 0) - _nestCheck[(int)NEST.FRONT].position;
                _isNest[(int)NEST.FRONT] = true;
            }
            else
            {
                _isNest[(int)NEST.FRONT] = false;
            }

            //後めり込み判定
            nestPos = _nestCheck[(int)NEST.BACK].position;
            nestRay = new Ray(nestPos + new Vector3(0, 0, distance / 2), new Vector3(0, 0, -1));
            Debug.DrawRay(nestRay.origin, nestRay.direction * distance);
            if (Physics.Raycast(nestRay, out hit, distance, layermask))
            {
                //めり込み排斥
                transform.position += nestPos + new Vector3(transform.right.x * 0.05f, 0, 0) - _nestCheck[(int)NEST.BACK].position;
                _isNest[(int)NEST.BACK] = true;
            }
            else
            {
                _isNest[(int)NEST.BACK] = false;
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

            _animator.SetInteger("Move", move);
            _rigit.AddForce(grv, ForceMode.Acceleration);
        }

        //振り向く
        private void Flip()
        {
            _facingRight = !_facingRight;

            float rot = transform.localEulerAngles.y;

            rot += 180;

            transform.localEulerAngles = new Vector3(0, rot, 0);
        }

        public void ClearMove()
        {
            DeleteRigitBody();

            _animator.speed = 0;

            //クリアアニメーション
            Observable.Timer(System.TimeSpan.FromSeconds(2))
                .Subscribe(_ =>
                {
                    _animator.speed = 1;
                    _animator.SetBool("Clear", true);
                })
                .AddTo(this);
        }

        public void DeadMove()
        {
            DeleteRigitBody();

            //死亡アニメーション
            _animator.SetBool("Dead", true);
        }

        public void DeleteRigitBody()
        {
            //rigitbody削除
            Destroy(GetComponent<Rigidbody>());
            //Collider削除
            Collider[] colliders = GetComponents<Collider>();
            foreach (Collider col in colliders)
            {
                Destroy(col);
            }
        }
    }
}
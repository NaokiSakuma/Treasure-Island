using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //移動速度
    [SerializeField]
    private float _maxSpeed = 10.0f;

    //ジャンプ力ぅ
    [SerializeField]
    private float _jumpForce = 400.0f;

    //接地
    private bool _grounded;
    public bool isGrounded
    {
        get { return _grounded; }
    }

    //重力
    private float _gravity;

    private Rigidbody _rigit;

    void Awake()
    {
        _rigit = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start()
    {
        _grounded = false;
    }

    //壁に当たってもジャンプできるから後で修正
    void OnCollisionEnter(Collision col)
    {
        _grounded = true;
    }

    //移動
    public void Move(float move, bool jump)
    {
        _rigit.velocity = new Vector2(move * _maxSpeed, _rigit.velocity.y);

        if (_grounded && jump)
        {
            _grounded = false;
            _rigit.AddForce(new Vector2(0f, _jumpForce));
        }
    }
}

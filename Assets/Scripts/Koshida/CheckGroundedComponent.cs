using UnityEngine;
using UniRx;
using System;

public class CheckGroundedComponent : MonoBehaviour
{
    private bool _isGrounded;

    /// <summary>
    /// 地面に接地しているかどうか
    /// </summary>
    public bool IsGrounded { get { return _isGrounded; } }

    void Start()
    {
        var controller = GetComponent<PlayerMove>();
        controller
            .ObserveEveryValueChanged(x => x.isGrounded)
            .ThrottleFrame(5)
            .Subscribe(x => _isGrounded = x);
    }
}

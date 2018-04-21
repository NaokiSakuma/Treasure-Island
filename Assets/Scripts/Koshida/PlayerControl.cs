using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Konji
{
    [RequireComponent(typeof(PlayerMove))]
    public class PlayerControl : MonoBehaviour
    {
        private PlayerMove _player;
        private bool _jump;

        private void Awake()
        {
            _player = GetComponent<PlayerMove>();
        }

        private void Update()
        {
            if (!_jump)
            {
                _jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }

        private void FixedUpdate()
        {
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            _player.Move(h, _jump);
            _jump = false;
        }
    }
}
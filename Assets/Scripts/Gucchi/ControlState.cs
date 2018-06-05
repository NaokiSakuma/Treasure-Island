using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GucchiCS
{
    public class ControlState : SingletonMonoBehaviour<ControlState>
    {
        // 操作モード（マウスならtrue、キーボードならfalse）
        bool _isStateMouse = false;

        // Use this for initialization
        void Start()
        {
            // 何らかのキーが押されたらキーステートにする
            this.UpdateAsObservable()
                .Where(_ => _isStateMouse)
                .Where(_ => Input.anyKeyDown && !Input.GetMouseButtonDown(0))
                .Subscribe(_ =>
                {
                    Cursor.visible = false;
                    _isStateMouse = false;
                });

            // マウスが動かされたらマウスステートにする
            this.UpdateAsObservable()
                .Where(_ => !_isStateMouse)
                .Where(_ => Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                .Subscribe(_ =>
                {
                    Cursor.visible = true;
                    _isStateMouse = true;
                });
        }

        // マウス操作状態かどうか
        public bool IsStateMouse
        {
            get { return _isStateMouse; }
        }
    }
}
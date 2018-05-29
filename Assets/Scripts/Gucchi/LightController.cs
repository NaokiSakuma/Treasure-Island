using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GucchiCS
{
    public class LightController : MonoBehaviour
    {
        // 移動制限
        [SerializeField]
        float _limit = 10f;

        // オブジェクトスクリーン（これをつけないなら別の方法を考える）
        [SerializeField]
        Transform _objectScreen;

        // ライトの移動スピード
        [SerializeField]
        float _moveSpeed = 5f;

        // 見かけ上のライト
        [SerializeField]
        GameObject _lightObject;

        void Awake()
        {
            // スポットライトの角度変更
            this.UpdateAsObservable()
                .Where(_ => ModeChanger.Instance.Mode == ModeChanger.MODE.SPOTLIGHT_CONTROL)
                .Where(_ => !ModeChanger.Instance.IsChanging)
                .Where(_ => Input.anyKey)
                .Subscribe(_ =>
                {
                    // 現在の角度
                    Vector3 eulerAngle = this.EulerAngles;

                    // キー操作でDictional Lightの角度と見かけ上のライトを変える
                    // 左
                    if (Input.GetKey(KeyCode.LeftArrow) && eulerAngle.y < _limit)
                    {
                        transform.Rotate(0f, _moveSpeed * Time.deltaTime, 0f, Space.World);
                        _lightObject.transform.Translate((-_moveSpeed * 0.05f) * Time.deltaTime, 0f, 0f, Space.World);
                    }

                    // 右
                    if (Input.GetKey(KeyCode.RightArrow) && eulerAngle.y > -_limit)
                    {
                        transform.Rotate(0f, -_moveSpeed * Time.deltaTime, 0f, Space.World);
                        _lightObject.transform.Translate((_moveSpeed * 0.05f) * Time.deltaTime, 0f, 0f, Space.World);
                    }

                    // 上
                    if (Input.GetKey(KeyCode.UpArrow) && eulerAngle.x < _limit)
                    {
                        transform.Rotate(_moveSpeed * Time.deltaTime, 0f, 0f, Space.World);
                        _lightObject.transform.Translate(0f, (_moveSpeed * 0.05f) * Time.deltaTime, 0f, Space.World);
                    }

                    // 下
                    if (Input.GetKey(KeyCode.DownArrow) && eulerAngle.x > -_limit)
                    {
                        transform.Rotate(-_moveSpeed * Time.deltaTime, 0f, 0f, Space.World);
                        _lightObject.transform.Translate(0f, (-_moveSpeed * 0.05f) * Time.deltaTime, 0f, Space.World);
                    }

                    // 見かけ上のライトをオブジェクトスクリーンの方へ向ける
                    _lightObject.transform.LookAt(_objectScreen);
                    _lightObject.transform.Rotate(new Vector3(0f, 180f, 0f));
                });
        }

        /* プロパティ */

        // 現在の角度（オイラー角）
        public Vector3 EulerAngles
        {
            get
            {
                // 現在の角度
                Vector3 eulerAngle = transform.localEulerAngles;

                // 角度補正（マイナスでなく360度になってしまう現象を-limit ～ limit内に収める）
                if (eulerAngle.x > 180f) eulerAngle.x -= 360f;
                if (eulerAngle.y > 180f) eulerAngle.y -= 360f;

                return eulerAngle;
            }
        }

        // 角度の限度（オイラー角）
        public float LimitAngle
        {
            get { return _limit; }
        }
    }
}
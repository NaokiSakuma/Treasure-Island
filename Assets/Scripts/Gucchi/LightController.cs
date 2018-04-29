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
        public Transform _objectSetter;

        // ライトの移動スピード
        [SerializeField]
        float _moveSpeed = 5f;

        void Awake()
        {
            // スポットライトの角度変更
            this.UpdateAsObservable()
                .Where(_ => ModeChanger.Instance.Mode == ModeChanger.MODE.SPOTLIGHT_CONTROL)
                .Where(_ => Input.anyKey)
                .Subscribe(_ =>
                {
                    // 現在の角度
                    Vector3 eulerAngle = this.EulerAngles;

                    // キー操作で角度を変える
                    // 左
                    if (Input.GetKey(KeyCode.LeftArrow) && eulerAngle.y > -_limit)
                        transform.Rotate(0f, -_moveSpeed * Time.deltaTime, 0f);

                    // 右
                    if (Input.GetKey(KeyCode.RightArrow) && eulerAngle.y < _limit)
                        transform.Rotate(0f, _moveSpeed * Time.deltaTime, 0f);

                    // 上
                    if (Input.GetKey(KeyCode.UpArrow) && eulerAngle.x < _limit)
                        transform.Rotate(_moveSpeed * Time.deltaTime, 0f, 0f);

                    // 下
                    if (Input.GetKey(KeyCode.DownArrow) && eulerAngle.x > -_limit)
                        transform.Rotate(-_moveSpeed * Time.deltaTime, 0f, 0f);
                });
        }

        /* プロパティ */

        //// 現在の座標
        //public Vector3 Position
        //{
        //    get { return transform.position; }
        //}

        //// ゲームスクリーンに反映させるための座標
        //public Vector2 ReflectedGameViewPosition
        //{
        //    get
        //    {
        //        // 係数
        //        float coefX = 1f / _limit.x;
        //        float coefY = 1f / _limit.y;

        //        // 係数によって補完させる
        //        float compX = transform.position.x * coefX;
        //        float compY = transform.position.y * coefY;

        //        return new Vector2(compX, compY);
        //    }
        //}

        //// 現在の角度（Quaternion）
        //public Quaternion Rotate
        //{
        //    get { return transform.rotation; }
        //}

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
    }
}
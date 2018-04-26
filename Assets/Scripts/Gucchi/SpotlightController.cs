using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GucchiCS
{
    public class SpotlightController : MonoBehaviour
    {
        // 移動制限
        [SerializeField]
        Vector2 _limit = new Vector2(0.7f, 0.4f);

        // オブジェクトスクリーン（これをつけないなら別の方法を考える）
        public Transform _objectSetter;

        // ライトの移動スピード
        [SerializeField]
        float _moveSpeed = 0.02f;

        void Awake()
        {
            // 角度はオブジェクトの方へ常に向ける
            this.ObserveEveryValueChanged(pos => transform.position)
                .Subscribe(pos =>
                {
                    // オブジェクトスクリーンのほうへ向く
                    transform.LookAt(_objectSetter);
                    transform.Rotate(new Vector3(0f, 180f, 0f));
                });

            // スポットライトの移動
            this.UpdateAsObservable()
                .Where(_ => Input.anyKey)
                .Where(_ => ModeChanger.Instance.Mode == ModeChanger.MODE.SPOTLIGHT_CONTROL)
                .Subscribe(_ =>
                {
                    // キー操作によってライトの位置を変える
                    var newPos = transform.position;
                    if      (Input.GetKey(KeyCode.LeftArrow))   newPos.x += -_moveSpeed;
                    else if (Input.GetKey(KeyCode.RightArrow))  newPos.x += _moveSpeed;
                    else if (Input.GetKey(KeyCode.UpArrow))     newPos.y += _moveSpeed;
                    else if (Input.GetKey(KeyCode.DownArrow))   newPos.y += -_moveSpeed;

                    // クランプがゴミなので一旦if文
                    if (newPos.x < -_limit.x) newPos.x = -_limit.x;     if (newPos.x > _limit.x) newPos.x = _limit.x;
                    if (newPos.y < -_limit.y) newPos.y = -_limit.y;     if (newPos.y > _limit.y) newPos.y = _limit.y;
                    transform.position = newPos;
                });
        }

        /* プロパティ */

        // 現在の座標
        public Vector3 Position
        {
            get { return transform.position; }
        }

        // ゲームスクリーンに反映させるための座標
        public Vector2 ReflectedGameViewPosition
        {
            get
            {
                // 係数
                float coefX = 1f / _limit.x;
                float coefY = 1f / _limit.y;

                // 係数によって補完させる
                float compX = transform.position.x * coefX;
                float compY = transform.position.y * coefY;

                return new Vector2(compX, compY);
            }
        }

        // 現在の角度
        public Quaternion Rotate
        {
            get { return transform.rotation; }
        }
    }
}
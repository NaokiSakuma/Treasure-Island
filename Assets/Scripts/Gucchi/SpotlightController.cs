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

        // 距離
        [SerializeField]
        float _distance = 10f;

        // オブジェクトスクリーン（これをつけないなら別の方法を考える）
        public Transform _objectSetter;

        // スポットライトの移動時の座標
        Vector3 _beginPos;

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

            // クリックイベント
            var controlBegin = Observable
                .EveryUpdate()
                .Where(_ => Input.GetMouseButton(0))
                .Where(_ => CheckRaycastHit());

            // クリックを離した時
            var controlEnd = Observable
                .EveryUpdate()
                .Where(_ => Input.GetMouseButtonUp(0));

            // スポットライトの移動
            this.UpdateAsObservable()
                .SkipUntil(controlBegin)
                .Do(_ => { _beginPos = transform.position; })
                .TakeUntil(controlEnd)
                .Repeat()
                .Subscribe(_ =>
                {
                    // マウスに追従
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        var newPos = new Vector3(hit.point.x, hit.point.y, -_distance);
                        // クランプがゴミなので一旦if文
                        if (newPos.x < -_limit.x) newPos.x = -_limit.x;     if (newPos.x > _limit.x) newPos.x = _limit.x;
                        if (newPos.y < -_limit.y) newPos.y = -_limit.y;     if (newPos.y > _limit.y) newPos.y = _limit.y;
                        transform.position = newPos;
                    }
                });
        }

        // マウスのレイキャストが自身に触れたかどうか
        bool CheckRaycastHit()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    return true;
                }
            }

            return false;
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
                float compX = transform.position.x / coefX;
                float compY = transform.position.y / coefY;

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
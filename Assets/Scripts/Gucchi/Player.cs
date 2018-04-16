/*
 * @Date    18/04/014
 * @Create  Yuta Higuchi
*/
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GucchiCS
{
    public class Player : MonoBehaviour
    {
        // レイヤー
        public enum Layer
        {
            ALL = -1,
            ISLAND = 9
        };

        // ワームホールの半径（味方ユニットのサイズ次第で後々変更）
        [SerializeField]
        float _wormholeRadius = 3f;

        // 神の手のワームホールモード（true: 入口のワームホールが存在している状態）
        bool _isExistWormhole = false;

        // ワームホール
        public GameObject _wormhole;
        GameObject _wormholeEnter = null;
        GameObject _wormholeExit = null;

        // ワームホールに入る対象の味方ユニット
        List<UnitCore> _followerList = new List<UnitCore>();

        void Awake()
        {
            /* ～に乗っている味方ユニットのように後々制限の設定が必要 */

            // 入口の処理
            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Where(_ => !_isExistWormhole)
                .Subscribe(_ =>
                {
                    Debug.Log(GetRaycastPoint());
                    Debug.Log("Enter!");

                    // ワームホールの出現
                    _wormholeEnter = CreateWormholeEnter();

                    // リストに追加
                    GetIntoRangeUnit();
                });

            // 出口の処理
            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Where(_ => _isExistWormhole)
                .Subscribe(_ =>
                {
                    Debug.Log(GetRaycastPoint());
                    Debug.Log("Exit!");

                    // ワームホールの出現
                    _wormholeExit = CreateWormholeEnter();

                    // 味方ユニットの移動
                    TranslateUnit();

                    // ワームホールを消す
                    Destroy(_wormholeEnter.gameObject);
                    Destroy(_wormholeExit.gameObject);
                    _wormholeEnter = null;
                    _wormholeExit = null;
                });

            // トラッカー
            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonUp(0))
                .Subscribe(_ => { _isExistWormhole = !_isExistWormhole; });
        }

        // ワームホール生成
        GameObject CreateWormholeEnter()
        {
            // ワームホールの出現（スケールのYは後でなんとかする）
            GameObject wormhole = Instantiate(_wormhole, GetRaycastPoint(), Quaternion.identity);
            wormhole.transform.localScale = new Vector3(_wormholeRadius * 2, 0.005f, _wormholeRadius * 2);

            return wormhole;
        }

        // 味方ユニットの移動
        void TranslateUnit()
        {
            if (!_wormholeEnter || !_wormholeExit || _followerList.Count <= 0)
                return;

            // 入口と出口のベクトル差分を計算
            Vector3 trans = _wormholeExit.transform.position - _wormholeEnter.transform.position;

            // 移動
            foreach (UnitCore follower in _followerList)
            {
                follower.transform.Translate(trans.x, 0f, trans.z);
            }

            // リストをクリア
            _followerList.Clear();
        }

        // レイキャスト取得
        RaycastHit GetRaycast(int layerMask = -1)
        {
            // クリックした位置のレイキャストを取得
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // レイキャストで触れたオブジェクトの格納用
            RaycastHit hit = new RaycastHit();

            // レイヤーマスクの設定
            layerMask = layerMask >= 0 ? (1 << layerMask) : 1;

            // レイキャストの座標を取得
            Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);

            return hit;
        }

        // レイキャスト座標取得
        Vector3 GetRaycastPoint(int layerMask = -1)
        {
            // レイキャストを取得
            RaycastHit hit = GetRaycast(layerMask);

            // レイキャストの座標を返す
            return hit.point;
        }

        // レイキャストで触れたオブジェクトを取得
        GameObject GetRaycastHitObeject(int layerMask = -1)
        {
            // レイキャストを取得
            RaycastHit hit = GetRaycast(layerMask);

            // レイキャストの座標を返す
            return hit.collider ? hit.collider.gameObject : null;
        }

        // 範囲内の味方ユニットを取得（後々味方ユニット用クラスに変える予定）
        void GetIntoRangeUnit()
        {
            // 範囲内にあるColliderを取得
            Collider[] targets = Physics.OverlapSphere(GetRaycastPoint(), _wormholeRadius);

            // Colliderのうち味方ユニットのみを取得
            foreach (Collider target in targets)
            {
                if (target.GetComponent<UnitCore>() != null)
                {
                    _followerList.Add(target.GetComponent<UnitCore>());
                }
            }
        }
    }
}
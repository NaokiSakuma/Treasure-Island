/*
 * @Date    18/03/19
 * @Create  Yuta Higuchi
*/
using Konji;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GucchiCS
{
    public class GodHand : MonoBehaviour
    {
        List<Unit>      _unitList;
        Relic           _relic;
        bool            _catchMode;

        // レイヤーテスト用。後々enumかなにかで管理したほうがいい
        const int       _islandLayer = 9;
        const int       _relicSetterLayer = 10;

        //public Text     _unitNumText;

		void Awake()
		{
            _unitList = new List<Unit>();
            _catchMode = true;
		}

		// Update is called once per frame
		void Update()
        {
            // キャッチャー処理
            if (_catchMode)
            {
                CatchMode();
            }
            else
            {
                UncatchMode();
            }

            // 追従処理
            FollowHand();

            // つかんでいるユニットがいる島から移動できる島はいくつあるか表示
            if (GetUnitNum > 0)
            {
                Debug.Log(_unitList[0].Ground.GetNearIslands().Count);
            }

            // ユニット数を更新
            //UpdateSeizeNumText();
        }

        // キャッチモード中（つかめる状態）の処理
        void CatchMode()
        {
            // 左クリック
            if (Input.GetMouseButton(0))
            {
                GameObject hit = GetHitObject();

                // nullチェック
                if (hit == null)
                    return;

                // ユニットの場合
                if (hit.GetComponent<Unit>() != null)
                {
                    Unit unit = hit.GetComponent<Unit>();

                    // まだつかまれていないユニットだったら
                    if (!unit.IsClutched)
                    {
						unit.IsClutched = true;
                        _unitList.Add(unit);
                    }
                }
            }
            // 右クリック
            else if (Input.GetMouseButtonDown(1))
            {
                GameObject hit = GetHitObject();

                // nullチェック
                if (hit == null)
                    return;

                // 遺物でないとき
                if (hit.GetComponent<Relic>() == null)
                    return;

                Relic relic = hit.GetComponent<Relic>();

                // まだつかまれていない遺物だったら
                if (!relic.IsClutched)
                {
                    // 島の設定
                    GameObject islandHit = GetHitObject(_islandLayer);
                    if (islandHit && islandHit.GetComponent<Island>() != null)
                        islandHit.GetComponent<Island>().RemoveRelic(relic);
                    relic.LandingIsland = null;

                    // 遺物の土台設定
                    relic.LandingFoundation.LandingRelic = null;
                    relic.LandingFoundation = null;

                    // 遺物をつかむ
                    relic.IsClutched = true;
                    _relic = relic;
                    _relic.IsPut = false;
					_catchMode = false;
                }
            }
            // クリックを離したとき
            else if (Input.GetMouseButtonUp(0))
            {
                _catchMode = false;
            }
        }

        // アンキャッチモード中（すでにつかんでいる状態）の処理
        void UncatchMode()
        {
            // 左クリック
            if (Input.GetMouseButtonUp(0) && !_relic)
            {
                GameObject hit = GetHitObject(_islandLayer);
                if (hit && CheckGroundIntoMovingRange(hit))
                {
                    foreach (Unit unit in _unitList)
                    {
                        // 島
                        if (hit.GetComponent<Island>() != null)
                        {
                            // 島の取得
                            Island island = hit.GetComponent<Island>();

                            island.UnitList.Add(unit);
                            unit.IsClutched = false;
                            unit.Ground = hit.GetComponent<IGround>();

                            // 通知
                            island.LandingNotify(unit);
                        }
                        // 魚群
                        else if (hit.GetComponent<Fishes>() != null)
                        {
                            // 魚群の取得
                            Fishes fishes = hit.GetComponent<Fishes>();

                            fishes.UnitList.Add(unit);
                            unit.IsClutched = false;
                            unit.Ground = hit.GetComponent<IGround>();

                            // 通知
                            fishes.LandingNotify(unit);
                        }
                    }

					_unitList.Clear();
                }
            }
            // 右クリック
            else if (Input.GetMouseButtonDown(1))
            {
                // ユニット
                if (!_relic)
                {
                    GameObject hit = GetHitObject(_islandLayer);

                    if (hit && CheckGroundIntoMovingRange(hit))
                    {
                        // 島
                        if (hit.GetComponent<Island>() != null)
                        {
                            // 島の取得
                            Island island = hit.GetComponent<Island>();

                            island.UnitList.Add(_unitList[0]);
                            _unitList[0].IsClutched = false;
                            _unitList[0].Ground = hit.GetComponent<IGround>();
                            _unitList.RemoveAt(0);

                            // 通知
                            island.LandingNotify(_unitList[0]);
                        }
                        // 魚群
                        else if (hit.GetComponent<Fishes>() != null)
                        {
                            // 魚群の取得
                            Fishes fishes = hit.GetComponent<Fishes>();

                            fishes.UnitList.Add(_unitList[0]);
                            _unitList[0].IsClutched = false;
                            _unitList[0].Ground = hit.GetComponent<IGround>();

                            // 通知
                            fishes.LandingNotify(_unitList[0]);
                        }
                    }
                }
                // 遺物
                else
                {
                    GameObject hit = GetHitObject(_relicSetterLayer);

                    // 土台を取得できてかつその土台に別の遺物がなかったら
                    if (hit && hit.GetComponent<RelicFoundation>().LandingRelic == null)
                    {
                        // 島の設定
                        Island landingIsland = GetHitObject(_islandLayer).GetComponent<Island>();
                        landingIsland.RegisterRelic(_relic);
                        _relic.LandingIsland = landingIsland;

                        // 遺物の土台の設定
                        hit.GetComponent<RelicFoundation>().LandingRelic = _relic;
                        _relic.LandingFoundation = hit.GetComponent<RelicFoundation>();

                        // 遺物の設置
                        _relic.IsClutched = false;
                        _relic.transform.position = new Vector3(hit.transform.position.x, 18f, hit.transform.position.z);
                        _relic.IsPut = true;
                        _relic = null;
                    }
                }
            }
        }

        // キャッチャー（マウスポインタなど）のレイキャストで触れたオブジェクトを取得
        GameObject GetHitObject(int layerMask = -1)
        {
            // クリックした位置のレイキャストを取得
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // レイキャストで触れたオブジェクトの格納用
            RaycastHit hit = new RaycastHit();

            // レイヤーマスクの設定
            layerMask = layerMask >= 0 ? (1 << layerMask) : 1;

            // レイキャストで触れたオブジェクトを取得
            Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);

            return hit.collider ? hit.collider.gameObject : null;
        }

        // 神の手に追従
        void FollowHand()
        {
            // 遺物
            if (_relic)
            {
                // クリックした位置のレイキャストを取得
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // レイキャストで触れたものの場所に置く（y軸固定）
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    var targetPos = new Vector3(hit.point.x, 18f, hit.point.z);
                    _relic.transform.position = targetPos;
                }

                return;
            }

            if (GetUnitNum == 0)
            {
                _catchMode = true;
                return;
            }

            // ユニット
            foreach (Unit unit in _unitList)
            {
                // クリックした位置のレイキャストを取得
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				// レイキャストで触れたものの場所に置く（y軸固定）
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    var targetPos = new Vector3(hit.point.x, 20f, hit.point.z);
                    unit.transform.position = targetPos;
                }
            }
        }

        // 現在つかんでいるユニットの数を取得
        public int GetUnitNum
        {
            get
            {
                return _unitList.Count;
            }
        }

        // 移動しようした場所が移動範囲内にあるか
        bool CheckGroundIntoMovingRange(GameObject hit)
        {
            // そもそも移動できるオブジェクトかどうか
            if (hit.GetComponent<IGround>() == null)
                return false;

            // 移動範囲内かどうか
            IGround ground = null;
            foreach (IGround nearGrounds in _unitList[0].Ground.GetNearIslands())
            {
                IGround target = hit.GetComponent<IGround>();
                if (nearGrounds == target)
                {
                    ground = target;
                    break;
                }
            }

            if (ground == null)
                return false;

            return true;
        }

        // 所持している遺物を選択した場合
        public void CatchHavingRelic(Relic relic)
        {
            // 遺物をつかむ
            relic.IsClutched = true;
            _relic = relic;
            _relic.IsPut = false;
            _catchMode = false;
        }

        // 現在つかんでいる遺物を取得
        public Relic CatchingRelic
        {
            get { return _relic; }
        }

        // つかんでいるユニット数テキストを更新
        //void UpdateSeizeNumText()
        //{
        //    _unitNumText.text = "List: " + GetUnitNum.ToString();
        //}
    }
}
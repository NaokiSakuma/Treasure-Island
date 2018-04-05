/*
 * @Date    18/03/19
 * @Create  Yuta Higuchi
*/
using Konji;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GucchiCS
{
    public class GodHand : MonoBehaviour
    {
        List<Unit>      _unitList;
        Relic           _relic;
        bool            _catchMode;

        // レイヤーテスト用。後々enumかなにかで管理したほうがいい
        const int       _islandLayer = 8;
        const int       _relicSetterLayer = 9;

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

                // つかめるオブジェクトでないとき
                if (hit.GetComponent<IUnit>() == null)
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
                        hit.GetComponent<Island>().UnitList.Add(unit);
                        unit.IsClutched = false;
                        unit.Ground = hit.GetComponent<IGround>();
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
                        hit.GetComponent<Island>().UnitList.Add(_unitList[0]);
                        _unitList[0].IsClutched = false;
                        _unitList[0].Ground = hit.GetComponent<IGround>();
                        _unitList.RemoveAt(0);
                    }
                }
                // 遺物
                else
                {
                    GameObject hit = GetHitObject(_relicSetterLayer);

                    if (hit)
                    {
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
                    var targetPos = new Vector3(hit.point.x, 5f, hit.point.z);
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

        // つかんでいるユニット数テキストを更新
        //void UpdateSeizeNumText()
        //{
        //    _unitNumText.text = "List: " + GetUnitNum.ToString();
        //}
    }
}
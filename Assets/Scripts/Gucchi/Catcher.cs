/*
 * @Date    18/03/19
 * @Create  Yuta Higuchi
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GucchiCS
{
    public class Catcher : MonoBehaviour
    {
        List<Unit>      _unitList;
        bool            _catchMode;

        // レイヤーテスト用。後々enumかなにかで管理したほうがいい
        const int       _islandLayer = 8;

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
                        _unitList.Add(unit);
                        unit.IsClutched = true;
                    }
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
            if (Input.GetMouseButtonDown(0))
            {
                GameObject hit = GetHitObject(_islandLayer);
                if (hit && hit.GetComponent<IGround>() != null)
                {
					_unitList.Clear();
                }
            }
            // 右クリック
            if (Input.GetMouseButtonDown(1))
            {
                GameObject hit = GetHitObject(_islandLayer);

                if (hit && hit.GetComponent<IGround>() != null)
                {
                    _unitList.RemoveAt(0);
                }
            }

            if (GetUnitNum == 0)
            {
                _catchMode = true;
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
            foreach (Unit unit in _unitList)
            {
                // 自身の座標をスクリーン座標に変換
                Vector3 screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);

                // マウスの座標を取得
                Vector3 mousePos = Input.mousePosition;

                // マウスの座標に追従させる
                Vector3 tmpPos = new Vector3(mousePos.x, mousePos.y, screenPos.z);

                // 座標の更新
                unit.transform.position = Camera.main.ScreenToWorldPoint(tmpPos);
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

        // つかんでいるユニット数テキストを更新
        //void UpdateSeizeNumText()
        //{
        //    _unitNumText.text = "List: " + GetUnitNum.ToString();
        //}
    }
}
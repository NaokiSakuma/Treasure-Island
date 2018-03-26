/*
 * @Date    18/03/20
 * @Create  Yuta Higuchi
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GucchiCS
{
    // 島
    public class Island : MonoBehaviour, IGround
    {
        // 島にいるユニットリスト
        List<Unit> _unitList;

        // 島にいる敵リスト
        // 敵に関するスクリプトが現在ない、今後作るか不明なので一旦Unit
        List<Unit> _enemyList;

        // 島のサイズ
        // 一旦int、0
        [SerializeField]
        int _size = 0;

        // 移動範囲
        [SerializeField]
        float _movingRange = 8f;

        void Awake()
        {
            _unitList = new List<Unit>();
            _enemyList = new List<Unit>();
        }

        // 移動できる範囲内にある島を取得
        public List<IGround> GetNearIslands()
        {
            List<IGround> nearIslands = new List<IGround>();

            // 移動範囲内にあるColliderを取得
            Collider[] targets = Physics.OverlapSphere(transform.position, _movingRange);

            // 取得したColliderのうち島のみを取得
            foreach (Collider col in targets)
            {
                if (col.gameObject.GetComponent<Island>() != null)
                {
                    IGround ground = col.gameObject.GetComponent<Island>();

                    nearIslands.Add(ground);
                }
            }

            return nearIslands;
        }
    }
}
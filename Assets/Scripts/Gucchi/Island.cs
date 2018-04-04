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
        // マテリアル
        enum ISLAND_MATERIAL : int
        {
            NULL,
            UNIT,
            ENEMY
        }
        public List<Material> _islandMaterial;

        // 島の初期状態（マテリアルのやつを使い回し）
        [SerializeField]
        ISLAND_MATERIAL _islandState = ISLAND_MATERIAL.NULL;

        // サイズリスト
        public List<float> _islandSizeData = new List<float>() {
            35f,
            50f, 
            75f
        };

        // サイズプルダウン用
        public enum ISLAND_SIZE : int
        {
            SMALL,
            MIDDLE,
            LARGE
        }

        // サイズの管理
        Dictionary<ISLAND_SIZE, float> _islandSizeDic = new Dictionary<ISLAND_SIZE, float>() {
            { ISLAND_SIZE.SMALL,  0f },
            { ISLAND_SIZE.MIDDLE, 0f },
            { ISLAND_SIZE.LARGE,  0f }
        };

        // 島のサイズ
        [SerializeField]
        ISLAND_SIZE _islandSize = ISLAND_SIZE.SMALL;

        // 移動範囲
        [SerializeField]
        float _movingRange = 8f;

        // 島にいるユニットリスト
        List<Unit> _unitList;

        // 島にいる敵リスト
        // 敵に関するスクリプトが現在ない、今後作るか不明なので一旦Unit
        List<Unit> _enemyList;

        void Awake()
        {
            _unitList = new List<Unit>();
            _enemyList = new List<Unit>();

            // サイズの設定（コード的にやばいので余裕があるときになおす）
            _islandSizeDic[ISLAND_SIZE.SMALL] = _islandSizeData[(int)ISLAND_SIZE.SMALL];
            _islandSizeDic[ISLAND_SIZE.MIDDLE] = _islandSizeData[(int)ISLAND_SIZE.MIDDLE];
            _islandSizeDic[ISLAND_SIZE.LARGE] = _islandSizeData[(int)ISLAND_SIZE.LARGE];

            // 指定されたスケールに変える
            float islandSize = _islandSizeDic[_islandSize];
            transform.localScale = new Vector3(islandSize, 3f, islandSize);

            // マテリアル設定
            transform.GetComponent<Renderer>().material = _islandMaterial[(int)_islandState];

            // 遺物設置場所を設定
            transform.GetComponent<RelicSetter>().SetRelicSetter(transform.position, _islandSize);
        }

        // 更新処理
        void Update()
        {
            // 占拠状況
            //CheckOccupationState();
        }

        // 占拠状況
        void CheckOccupationState()
        {
            // 島にいるユニットによって占拠状況を変える
            if (_unitList.Count > 0 && _enemyList.Count <= 0)
            {
                // マテリアル設定
                transform.GetComponent<Renderer>().material = _islandMaterial[(int)ISLAND_MATERIAL.UNIT];
            }
            else if (_unitList.Count <= 0 && _enemyList.Count > 0)
            {
                // マテリアル設定
                transform.GetComponent<Renderer>().material = _islandMaterial[(int)ISLAND_MATERIAL.ENEMY];
            }
            else
            {
                // マテリアル設定
                transform.GetComponent<Renderer>().material = _islandMaterial[(int)ISLAND_MATERIAL.NULL];
            }
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
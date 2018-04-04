using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GucchiCS
{
    public class RelicSetter : MonoBehaviour
    {
        // 遺物設置場所
        public GameObject _relicSetterPrefab;

        // 遺物設置場所のサイズ
        [SerializeField]
        float _relicSetterSize = 100f;

        // 島の大きさに応じて遺物を置ける場所を設置
        public void SetRelicSetter(Vector3 islandPos, Island.ISLAND_SIZE size)
        {
            // 島の中心を基準に並べる
            for (int i = 0; i < ((int)size + 1) * ((int)size + 1); i++)
            {
                // 遺物設置場所の生成
                Instantiate(_relicSetterPrefab,
                            new Vector3(islandPos.x + (i % ((int)size + 1)) * _relicSetterSize - ((int)size * (0.5f * _relicSetterSize)), 2.8f, islandPos.z + (i / ((int)size + 1)) * _relicSetterSize - ((int)size * (0.5f * _relicSetterSize))),
                            Quaternion.identity);
            }
        }
    }
}
/*
 * @Date    18/03/20
 * @Create  Yuta Higuchi
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GucchiCS
{
    public interface IGround
    {
        // 移動できる範囲内にある島を取得
        List<IGround> GetNearIslands();

        // 味方の解放
        void RemoveUnit(Unit unit);

        // 敵の解放
        void RemoveEnemy(Konji.LandingEnemy enemy);
    }
}
/*
 * @Date    18/03/19
 * @Create  Yuta Higuchi
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GucchiCS
{
    // ユニットデータ
    public class Unit : MonoBehaviour, ICharacter
    {
        // 初期地点
        [SerializeField]
        Island firstIsland = null;

        // 現在いる場所
        IGround ground;

        void Awake()
        {
            ground = firstIsland;
            this.IsClutched = false;
        }

        // つかまれている状態かどうか
        public bool IsClutched
        {
            get;
            set;
        }

        // 現在いる場所
        public IGround Ground
        {
            get
            {
                return ground;
            }
            set
            {
                ground = value;
            }
        }
    }
}
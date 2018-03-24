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
    public class Unit : MonoBehaviour, IUnit
    {
        void Awake()
        {
            this.IsClutched = false;
        }

        public bool IsClutched
        {
            get;
            set;
        }
    }
}
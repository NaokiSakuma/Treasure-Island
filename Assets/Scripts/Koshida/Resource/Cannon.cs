using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Konji
{
    public class Cannon : Relic
    {
        [SerializeField]
        private float _range = 300;

        protected override void Start()
        {
            base.Start();

            //種類の設定
            _type = CO.RelicType.Cannon;

            //名前の設定
            _name = CO.RELIC_LIST[(int)_type];
        }

        //最近ユニット検索
        private UnitCore SearchUnit(float range)
        {
            UnitCore unit = null;

            //最短距離
            float minDis = range;

            //範囲内のユニットを検索
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);

            foreach(Collider target in hitColliders)
            {
                UnitCore core = target.GetComponent<UnitCore>();

                //ユニットが見つからない
                if(core == null)
                {
                    continue;
                }

                //対象との距離を求める
                float dis = Vector3.Distance(transform.position, target.transform.position);

                //最短距離の更新
                if(dis < minDis)
                {
                    minDis = dis;
                    unit = core;
                }
            }
            return unit;
        }
    }
}
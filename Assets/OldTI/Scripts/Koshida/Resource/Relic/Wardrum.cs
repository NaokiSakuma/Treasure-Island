using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Konji
{
    [RequireComponent(typeof(Rigidbody))]
    public class Wardrum : Relic
    {
        [SerializeField]
        private int _buffAmo = 1;   //バフ量

        public int _team = -1;     //所属チーム

        protected override void Start()
        {
            base.Start();

            //種類の設定
            _type = CO.RelicType.WarDrum;

            //名前の設定
            _name = CO.RELIC_LIST[(int)_type];
        }

        //入ったユニットにバフ
        void OnTriggerEnter(Collider col)
        {
            if (IsPut)
            {
                //ユニットのステータス
                UnitCore core = col.GetComponent<UnitCore>();

                //ユニットの掴み情報
                GucchiCS.Unit unitInfo = col.GetComponent<GucchiCS.Unit>();

                //ユニットが見つからないか死亡済み、もしくは所属チームが違う 掴まれている場合
                if ((core == null || core.Health <= 0 || core.Team != _team) || (unitInfo == null || unitInfo.IsClutched))
                {
                    return;
                }

                //ステータス変化量にバフ
                core.AdditionalStrength = _buffAmo;
            }
        }

        //出たユニットのステータス初期化
        void OnTriggerExit(Collider col)
        {
            //ユニットのステータス
            UnitCore core = col.GetComponent<UnitCore>();

            //ユニットの掴み情報
            GucchiCS.Unit unitInfo = col.GetComponent<GucchiCS.Unit>();

            //ユニットが見つからないか死亡済み、もしくは所属チームが違う 掴まれている場合
            if ((core == null || core.Team != _team) || (unitInfo == null || unitInfo.IsClutched))
            {
                return;
            }

            //ステータス変化量の初期化
            core.AdditionalStrength = 0;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Konji
{
    public class Wardrum : Relic
    {
        protected override void Start()
        {
            base.Start();

            //種類の設定
            _type = CO.RelicType.WarDrum;

            //名前の設定
            _name = CO.RELIC_LIST[(int)CO.RelicType.WarDrum];

        }

        //プレイヤーにバフ
    }
}
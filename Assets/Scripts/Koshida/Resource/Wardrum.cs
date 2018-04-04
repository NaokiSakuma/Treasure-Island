using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Konji
{
    public class Wardrum : Relic
    {
        [SerializeField]
        private int _buffAmo = 1;   //バフ量

        protected override void Start()
        {
            base.Start();

            //種類の設定
            _type = CO.RelicType.WarDrum;

            //名前の設定
            _name = CO.RELIC_LIST[(int)_type];

        }

        //プレイヤーにバフ
    }
}
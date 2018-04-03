using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Konji
{
    public class MoneyTree : Relic
    {
        [SerializeField]
        private int _interval = 10;     //回復間隔

        [SerializeField]
        private int _healAmo = 100;     //回復量

        protected override void Start()
        {
            base.Start();

            //種類の設定
            _type = CO.RelicType.MoneyTree;

            //名前の設定
            _name = CO.RELIC_LIST[(int)CO.RelicType.MoneyTree];

            //置かれているときn秒間隔ごとにお宝回復
            Observable.Interval(System.TimeSpan.FromSeconds(_interval))
                .Where(_ => IsPut)
                .Subscribe(_ =>
                {
                    Debug.Log("+" + _healAmo.ToString() + "Treasure");
                });
        }
    }
}
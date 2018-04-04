using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Konji
{
    public class BlessingForest : Relic
    {
        [SerializeField]
        private int _interval = 2;     //回復間隔

        [SerializeField]
        private int _healAmo = 1;     //回復量

        protected override void Start()
        {
            base.Start();

            //種類の設定
            _type = CO.RelicType.BlessingForest;

            //名前の設定
            _name = CO.RELIC_LIST[(int)CO.RelicType.BlessingForest];

            //置かれているときn秒間隔ごとに食料回復
            Observable.Interval(System.TimeSpan.FromSeconds(_interval))
                .Where(_ => IsPut)
                .Subscribe(_ =>
                {
                    Debug.Log("+" + _healAmo.ToString() + "Food");
                });
        }
    }
}
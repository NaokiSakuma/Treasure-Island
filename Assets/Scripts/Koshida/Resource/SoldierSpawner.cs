using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Konji
{
    public class SoldierSpawner : MonoBehaviour
    {
        int resource = 0;

        //兵士生成間隔
        [SerializeField]
        private int _interval = 0;

        void Awake()
        {
            //資源マネージャー取得(現在は仮)

        }

        // Use this for initialization
        void Start()
        {
            //指定時間ごとに資源があれば兵士生成
            Observable.Interval(System.TimeSpan.FromSeconds(_interval))
                    .Where(_ => resource >= 100)
                    .Subscribe(_ =>
                    {
                        Debug.Log("Spawn");
                    });
        }
    }
}
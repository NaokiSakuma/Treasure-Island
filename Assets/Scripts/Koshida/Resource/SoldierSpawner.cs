using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Konji
{
    public class SoldierSpawner : MonoBehaviour
    {
        //兵士生成間隔
        [SerializeField]
        private int _interval = 3;

        //ユニットのプレハブ
        public GameObject _allyPrefab;

        //拠点の島
        public GameObject _baseIsland;

        void Awake()
        {
            //資源マネージャー取得(現在は仮)

        }

        // Use this for initialization
        void Start()
        {
            //指定時間ごとに資源があれば兵士生成
            Observable.Interval(System.TimeSpan.FromSeconds(_interval))
                    .Where(_ => ResourceManager.Instance.Resource >= 100)
                    .Subscribe(_ =>
                    {
                        AllySpawn();
                    })
                    .AddTo(this);
        }

        //兵士生成
        void AllySpawn()
        {
            ResourceManager.Instance.Resource -= 100;
            GameObject a = Instantiate(_allyPrefab, this.transform.position + new Vector3(0,0,-10), Quaternion.identity);
            var ground = a.GetComponent<GucchiCS.Unit>();
            ground.Ground = _baseIsland.GetComponent<GucchiCS.Island>();
        }
    }
}
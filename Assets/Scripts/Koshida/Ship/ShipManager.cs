using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Konji
{
    public class ShipManager : MonoBehaviour
    {
        //船の情報
        [System.Serializable]
        class ShipInfo
        {
            public GameObject _ship = null;    //船のタイプ
            public Transform _spawn = null;    //スポーンポイント
            public Transform _target = null;   //行き先
            //public float _speed;             //速度
            public float _spawnTime = 0.0f;    //出現タイミング
        }

        //船情報のリスト
        [SerializeField]
        private List<ShipInfo> _shipInfo;

        //小数ソート用
        private int SortFloat(float a,float b)
        {
            if (a - b > 0)
                return 1;
            else if (a - b < 0)
                return -1;
            else
                return 0;
        }

        void Awake()
        {
            _shipInfo.Sort((a, b) => SortFloat(a._spawnTime,b._spawnTime));
        }

        // Use this for initialization
        void Start()
        {
            //小数一桁秒で指定した時間に船を生成
            Observable.Timer(System.TimeSpan.FromSeconds(0), System.TimeSpan.FromSeconds(0.1))
                .Where(_ => _shipInfo.Count > 0)
                .Select(ms => ms / 10.0f)
                .Where(time => _shipInfo[0]._spawnTime <= time)
                .Select(_ => _shipInfo[0])
                .Subscribe(info =>
                {
                    GameObject ship = Instantiate(info._ship, info._spawn.position, Quaternion.identity);
                    EnemyShip eShip = ship.GetComponent<EnemyShip>();
                    eShip._target = info._target;
                    _shipInfo.Remove(info);
                })
                .AddTo(this);
        }
    }
}
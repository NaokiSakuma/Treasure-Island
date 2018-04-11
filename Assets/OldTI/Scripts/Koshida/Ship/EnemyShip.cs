using UnityEngine;
using UniRx;

namespace Konji
{
    public class EnemyShip : Ship
    {
        //敵のプレハブ
        public GameObject _enemyPrefab;

        //砲撃中
        [SerializeField]
        private BoolReactiveProperty _atkRP = new BoolReactiveProperty(false);

        //敵の数
        public int _enemyNum = 0;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();

            //島に到着したらn秒毎に賊投入
            Observable.Timer(System.TimeSpan.Zero, System.TimeSpan.FromSeconds(1))
                .Where(_ => !_isRideRP.Value && _arrivalRP.Value)
                .Take(_enemyNum)
                .Subscribe(_ =>
                {
                    GameObject enemy = Instantiate(_enemyPrefab, gameObject.transform.position, Quaternion.identity);
                    LandingEnemy le = enemy.GetComponent<LandingEnemy>();
                    le._nearObj = _target;

                }, () => Debug.Log("投入完了"))
                .AddTo(this);
        }
    }
}
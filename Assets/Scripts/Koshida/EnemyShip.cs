using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using UniRx.Triggers;

namespace Konji
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyShip : MonoBehaviour
    {
        public GameObject _enemyPrefab;

        //行き先
        public Transform _target;

        //プレイヤーユニット乗り状態
        [SerializeField]
        private BoolReactiveProperty _isRideRP = new BoolReactiveProperty(false);

        //砲撃中
        [SerializeField]
        private BoolReactiveProperty _atkRP = new BoolReactiveProperty(false);

        //島に到着
        [SerializeField]
        private BoolReactiveProperty _arrivalRP = new BoolReactiveProperty(false);

        private NavMeshAgent _navAgent;

        //敵の数
        private int _enemyNum = 0;

        private void Awake()
        {
            _navAgent = GetComponent<NavMeshAgent>();
            _enemyNum = 6;
        }

        private void Start()
        {
            //向かう行き先を検索
            if (_target != null)
            {
                _navAgent.destination = _target.position;

            }

            //到着
            this.UpdateAsObservable()
                .Where(_ => _navAgent.velocity == Vector3.zero)
                .Where(_ => !_isRideRP.Value && !_arrivalRP.Value)
                .Skip(1)
                .Take(1)
                .Subscribe(_ =>
                {
                    _arrivalRP.Value = true;
                });

            //兵士が乗り込んだら船停止
            _isRideRP.Where(x => x)
                .Subscribe(x => { _navAgent.isStopped = x; });

            //島に到着したらn秒毎に賊投入
            Observable.Timer(System.TimeSpan.Zero, System.TimeSpan.FromSeconds(1))
                .Where(_ => !_isRideRP.Value && _arrivalRP.Value)
                .Take(_enemyNum)
                .Subscribe(_ =>
                {
                    Instantiate(_enemyPrefab, gameObject.transform.position, Quaternion.identity);
                }, () => Debug.Log("End!"))
                .AddTo(this);
        }
    }
}
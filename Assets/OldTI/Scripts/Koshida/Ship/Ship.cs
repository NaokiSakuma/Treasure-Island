using UnityEngine;
using UnityEngine.AI;
using UniRx;
using UniRx.Triggers;

namespace Konji
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Ship : MonoBehaviour
    {
        //行き先
        public Transform _target;

        //プレイヤーユニット乗り状態
        [SerializeField]
        protected BoolReactiveProperty _isRideRP = new BoolReactiveProperty(false);

        //行き先に到着
        [SerializeField]
        protected BoolReactiveProperty _arrivalRP = new BoolReactiveProperty(false);

        //占領されたかどうか
        [SerializeField]
        private BoolReactiveProperty _isOccupyRP = new BoolReactiveProperty(false);

        protected NavMeshAgent _navAgent;

        protected virtual void Awake()
        {
            _navAgent = GetComponent<NavMeshAgent>();
        }

        // Use this for initialization
        protected virtual void Start()
        {
            //向かう行き先を検索
            if (_target != null)
            {
                _navAgent.destination = _target.position;
            }

            //到着
            this.UpdateAsObservable()
                .Where(_ => _navAgent.remainingDistance < 1.0f)
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
        }
    }
}
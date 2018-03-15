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
        [SerializeField]
        private Transform _target;

        private NavMeshAgent _navAgent;

        private void Awake()
        {
            _navAgent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            if (_target != null)
            {
                _navAgent.destination = _target.position;
            }
        }
    }
}


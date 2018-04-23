using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Konji
{
    public class Spawn : MonoBehaviour
    {
        [SerializeField]
        private Vector3[] _spawnPoint;

        void Awake()
        {
            if (_spawnPoint.Length == 0)
            {
                return;
            }

            //スポーン地点の設定
            int rnd = Random.Range(0, _spawnPoint.Length);

            transform.position = _spawnPoint[rnd];
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Konji
{
    [RequireComponent(typeof(TimerEventTrigger))]
    public class ShipEvent : MonoBehaviour
    {
        private TimerEventTrigger _timerEvent = null;

        public GameObject _ship = null;    //船のタイプ
        public Transform _spawn = null;    //スポーンポイント
        public Transform _target = null;   //行き先

        // Use this for initialization
        void Start()
        {
            _timerEvent = this.gameObject.GetComponent<TimerEventTrigger>();

            _timerEvent.Count = true;
        }

        void Update()
        {
            //イベントスタート
            if(_timerEvent.StartEvent())
            {
                //船出現
                GameObject ship = Instantiate(_ship, _spawn.position, Quaternion.identity);
                Ship eShip = ship.GetComponent<Ship>();
                eShip._target = _target;

                //イベント終了
                _timerEvent.End = true;
            }
        }
    }
}
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
        public int _enemyNum = 0;

        // Use this for initialization
        void Start()
        {
            _timerEvent = this.gameObject.GetComponent<TimerEventTrigger>();

            AudioManager.Instance.PlaySE(AUDIO.SE_SHIP);

            _timerEvent.Count = true;
        }

        void Update()
        {
            //イベントスタート
            if(_timerEvent.StartEvent())
            {
                //船出現
                GameObject ship = Instantiate(_ship, _spawn.position, Quaternion.identity);
                EnemyShip eShip = ship.GetComponent<EnemyShip>();

                if(eShip)
                {
                    eShip._target = _target;
                    eShip._enemyNum = _enemyNum;
                }
                else
                {
                    CommercialShip cShip = ship.GetComponent<CommercialShip>();
                    cShip._target = _target;
                }

                //イベント終了
                _timerEvent.End = true;
            }
        }
    }
}
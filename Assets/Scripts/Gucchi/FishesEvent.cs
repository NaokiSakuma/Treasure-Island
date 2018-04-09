/*
 * @Date    18/04/08
 * @Create  Yuta Higuchi
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GucchiCS
{
    [RequireComponent(typeof(TimerEventTrigger), typeof(OccupationEventTrigger))]
    public class FishesEvent : MonoBehaviour
    {
        // タイマーイベント
        TimerEventTrigger _timerEvent = null;

        // 占領イベント
        OccupationEventTrigger _occupationEvent = null;

        // Use this for initialization
        void Start()
        {
            // タイマーイベントの準備
            _timerEvent = this.gameObject.GetComponent<TimerEventTrigger>();
            _timerEvent.Count = true;

            // 占領イベントの準備
            _occupationEvent = this.gameObject.GetComponent<OccupationEventTrigger>();
        }

        // Update is called once per frame
        void Update()
        {
            // イベント開始
            if (_timerEvent.StartEvent() || _occupationEvent.NowEvent())
            {
                // 消滅
                Destroy(this.gameObject);

                // イベント終了
                _timerEvent.End = true;
            }
        }

        // 占拠されたとき
        public void OccupationNotify()
        {
            _occupationEvent.SetOccupyIsland(0, true);
        }
    }
}
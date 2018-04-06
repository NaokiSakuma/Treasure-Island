using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Sakuma
{
    public class TestTimer : MonoBehaviour
    {
        [SerializeField]
        private TimerEventTrigger timerEvent = null;

        [SerializeField]
        private bool end = false;
        public bool start = false;
        // Use this for initialization
        void Start()
        {
            if (timerEvent == null)
            {
                Debug.LogError("イベントが設定されていません");
            }
        }

        // Update is called once per frame
        void Update()
        {
            timerEvent.Count = start;
            timerEvent.StartEvent();
            timerEvent.NowEvent();
            timerEvent.End = end;
            timerEvent.EndEvent();
        }
    }
}
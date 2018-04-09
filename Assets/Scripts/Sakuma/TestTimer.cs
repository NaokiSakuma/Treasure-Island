using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Sakuma
{
    public class TestTimer : MonoBehaviour
    {
        // イベントの実態(必須)
        [SerializeField]
        private TimerEventTrigger _timerEvent = null;
        // タイマーをスタートするフラグ(どっちでも) ※実装するときはprivate
        public bool _startTimer = false;
        // イベントが終了したことを伝えるフラグ(どっちでも) ※実装するときはprivate
        public bool _endEvent = false;

        // Use this for initialization
        void Start()
        {
            // 一応nullチェック
            if (_timerEvent == null)
            {
                Debug.LogError("イベントが設定されていません");
            }
            // 〇〇が起きてから一定時間後等の可能性を加味しているので、bossTimerとは別で時間を管理しています。
            // Countにtrueを入れると時間の計測を開始します
            _timerEvent.Count = _startTimer;
        }

        // Update is called once per frame
        void Update()
        {
            // StartEvent()でイベントの開始した瞬間、NowEvent()でイベント中、EndEvent()でイベントの終了が取得できます
            print("イベント開始か：" + _timerEvent.StartEvent());
            // イベント終了前にtrueを入れてください。　
            // イベントトリガーがそのイベントがいつ終わるのかを検知出来ないので実装
            // なんかいい方法あったら教えて＾０＾
            _timerEvent.End = _endEvent;
        }
    }
}
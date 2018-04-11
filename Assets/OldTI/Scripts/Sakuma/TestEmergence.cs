using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sakuma
{

    public class TestEmergence : MonoBehaviour
    {

        // イベントの実態(必須)
        [SerializeField]
        private EmergenceEventTrigger _emergenceEvent = null;
        // 島が出現しているかのフラグ(必須)　※実装するときはprivate
        public bool _islandEmergence = false;
        // イベントが終了したことを伝えるフラグ(どっちでも) ※実装するときはprivate
        public bool _endEvent = false;
        // Use this for initialization
        void Start()
        {
            // 一応nullチェック
            if (_emergenceEvent == null)
            {
                Debug.LogError("イベントが設定されていません");
            }

        }

        // Update is called once per frame
        void Update()
        {
            //　ボスが出現しているかを入れてください
            _emergenceEvent.Emergence = _islandEmergence;
            // StartEvent()でイベントの開始した瞬間、NowEvent()でイベント中、EndEvent()でイベントの終了が取得できます
            print("イベント開始か：" + _emergenceEvent.StartEvent());
            //// イベント終了前にtrueを入れてください
            //// イベントトリガーがそのイベントがいつ終わるのかを検知出来ないので実装
            //// なんかいい方法あったら教えて＾０＾
            _emergenceEvent.End = _endEvent;
        }
    }
}

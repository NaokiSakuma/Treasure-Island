using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sakuma
{

    public class TestOccupationIsland : MonoBehaviour
    {
        // イベントの実態(必須)
        [SerializeField]
        private OccupationEventTrigger _occupationIslandEvent = null;
        // イベントが終了したことを伝えるフラグ(どっちでも) ※実装するときはprivate
        public bool _endEvent = false;
        // Use this for initialization
        void Start()
        {
            // 一応nullチェック
            if (_occupationIslandEvent == null)
            {
                Debug.LogError("イベントが設定されていません");
            }
        }

        // Update is called once per frame
        void Update()
        {
            // 占拠しているオブジェクトの要素数を入れてください(占拠していないオブジェクトは入れる必要はないです)
            _occupationIslandEvent.SetOccupyIsland(2, true);
            // StartEvent()でイベントの開始した瞬間、NowEvent()でイベント中、EndEvent()でイベントの終了が取得できます
            print("イベント開始か：" + _occupationIslandEvent.StartEvent());
            //// イベント終了前にtrueを入れてください
            //// イベントトリガーがそのイベントがいつ終わるのかを検知出来ないので実装
            //// なんかいい方法あったら教えて＾０＾
            _occupationIslandEvent.End = _endEvent;
        }
    }
}

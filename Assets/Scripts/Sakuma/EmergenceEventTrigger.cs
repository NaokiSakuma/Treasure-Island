using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class EmergenceEventTrigger : EventTrigger {

    // 島の出現ステイト
    public bool Emergence
    {
        set { _eventDic["emergence"] = value; }
    }
    protected override void Awake()
    {
        // 登録
        _eventDic.Add("start", false);    // イベントの開始
        _eventDic.Add("end", false);      // イベントの終了
        _eventDic.Add("emergence", false);    // 島の出現 ※出現する島を管理するスクリプトあるならそっから持って来た方がいいと思う

    }

    void Start()
    {
        // イベントの開始した瞬間
        this.UpdateAsObservable()
            .Where(_ => _eventDic["emergence"] && !_eventDic["end"])
            .Take(1)
            .Subscribe(_ => StartEvent());

        // イベント中
        this.UpdateAsObservable()
            .Where(_ => _eventDic["start"] && !_eventDic["end"])
            .Subscribe(_ => NowEvent());

        // イベントの終了
        this.UpdateAsObservable()
            .Where(_ => _eventDic["end"])
            .Take(1)
            .Subscribe(_ => EndEvent());
    }

    /// <summary>
    /// イベントの開始した瞬間
    /// </summary>
    /// <returns>開始した瞬間：true, 開始した瞬間ではない：false</returns>
    public override bool StartEvent()
    {
        if (_eventDic["start"]) return false;
        if (_eventDic["emergence"] && !_eventDic["end"])
        {
            Debug.Log("イベント開始");
            _eventDic["start"] = true;
            return true;
        }
        return false;
    }
}

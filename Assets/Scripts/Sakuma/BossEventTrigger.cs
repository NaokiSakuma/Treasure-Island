using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class BossEventTrigger : MonoBehaviour , IEventTrigger{

    // ボスイベントを管理するdictionary
    private Dictionary<string, bool> _bossDic = new Dictionary<string, bool>();

    // イベント終了
    public bool End
    {
        set { _bossDic["end"] = value; }
    }

    // ボスのステイト
    public bool State
    {
        set { _bossDic["state"] = value; }
    }
    void Awake()
    {
        // 登録
        _bossDic.Add("start", false);    // イベントの開始
        _bossDic.Add("end", false);      // イベントの終了
        _bossDic.Add("finish", false);   // イベントの一連の流れの終了
        _bossDic.Add("state", false);    // ボスの生存状態 ※boss管理するスクリプトあるならそっから持って来た方がいいと思う
        
    }

    void Start()
    {
        // イベントの開始した瞬間
        this.UpdateAsObservable()
            .Where(_ => _bossDic["state"] && !_bossDic["end"])
            .Take(1)
            .Subscribe(_ => StartEvent());

        // イベント中
        this.UpdateAsObservable()
            .Where(_ => _bossDic["start"] && !_bossDic["end"])
            .Subscribe(_ => NowEvent());

        // イベントの終了
        this.UpdateAsObservable()
            .Where(_ => _bossDic["end"])
            .Take(1)
            .Subscribe(_ => EndEvent());
    }
    /// <summary>
    /// イベントの開始した瞬間
    /// </summary>
    /// <returns>開始した瞬間：true, 開始した瞬間ではない：false</returns>
    public bool StartEvent()
    {
        if (_bossDic["start"]) return false;
        if (_bossDic["state"] && !_bossDic["end"])
        {
            Debug.Log("イベント開始");
            _bossDic["start"] = true;
            return true;
        }
        return false;
    }

    /// <summary>
    /// イベント中
    /// </summary>
    /// <returns>イベント中：true, イベント中ではない：false</returns>
    public bool NowEvent()
    {
        if (_bossDic["start"] && !_bossDic["end"])
        {
            Debug.Log("イベント中");
            return true;
        }
        return false;
    }

    /// <summary>
    /// イベントの終了
    /// </summary>
    /// <returns>終了している：true, 終了していない：false</returns>
    public bool EndEvent()
    {
        if (_bossDic["end"])
        {
            Debug.Log("イベント終了");
            return true;
        }
        return false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour, IEventTrigger {

    // イベントを管理するdictionary
    protected Dictionary<string, bool> _eventDic = new Dictionary<string, bool>();

    // イベント終了
    public bool End
    {
        set { _eventDic["end"] = value; }
    }

    protected virtual void Awake()
    {
        _eventDic.Add("start", false);    // イベントの開始
        _eventDic.Add("end", false);      // イベントの終了
    }

    /// <summary>
    /// イベントの開始した瞬間
    /// </summary>
    /// <returns>開始した瞬間：true, 開始した瞬間ではない：false</returns>
    public virtual bool StartEvent()
    {
        return false;
    }

    ///// <summary>
    ///// イベント中
    ///// </summary>
    ///// <returns>イベント中：true, イベント中ではない：false</returns>
    public virtual bool NowEvent()
    {
        if (_eventDic["start"] && !_eventDic["end"])
        {
            Debug.Log("イベント中");
            return true;
        }
        return false;
    }

    ///// <summary>
    ///// イベントの終了
    ///// </summary>
    ///// <returns>終了している：true, 終了していない：false</returns>
    public virtual bool EndEvent()
    {
        if (_eventDic["end"])
        {
            Debug.Log("イベント終了");
            return true;
        }
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerEventTrigger : MonoBehaviour,IEventTrigger {

    // イベントが発火する時間
    [SerializeField]
    private int _ignitionTime = 0;
    // タイマー
    [SerializeField]
    private int _timer = 0;

    // イベントが始まった
    private bool _isStart = false;
    // イベントが終わった
    private bool _isEnd = false;
    public bool IsEnd
    {
        set { _isEnd = value; }
    }

    public bool StartEvent()
    {
        if (_isStart) return false;
        if(_timer == _ignitionTime)
        {
            Debug.Log("イベント開始");
            _isStart = true;
            return true;
        }
        return false;
    }

    public bool NowEvent()
    {
        if (_isStart && !_isEnd)
        {
            Debug.Log("イベント中");
            return true;
        }
        return false;
    }

    public bool EndEvent()
    {
        if (_isEnd)
        {
            Debug.Log("イベント終了");
            return true;
        }
        return false;
    }
}

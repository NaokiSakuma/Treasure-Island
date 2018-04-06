using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TimerEventTrigger : MonoBehaviour,IEventTrigger {

    // イベントが発火する時間(秒)
    [SerializeField]
    private int _ignitionTime = 0;
    // タイマー
    private int _timer = 0;
    
    // タイマーイベントを管理するdictionary
    private Dictionary<string, bool> _timeDic = new Dictionary<string, bool>();

    // イベント終了
    public bool End
    {
        set { _timeDic["end"] = value; }
    }

    // タイマーをスタート
    public bool Count
    {
        set { _timeDic["count"] = value; }
    }
    void Awake()
    {
        // 登録
        _timeDic.Add("start", false);    // イベントの開始
        _timeDic.Add("end", false);      // イベントの終了
        _timeDic.Add("count", false);    // タイマーを動かす
    }
    void Start()
    {
        // タイマーをカウント
        this.UpdateAsObservable()
            .Where(_ => _timeDic["count"])
            .First(_ => _timeDic["count"])
            .Subscribe(_ => StartCoroutine(AddTime()));

        // イベントの開始した瞬間
        this.UpdateAsObservable()
            .Where(_ => _timer == _ignitionTime)
            .Take(1)
            .Subscribe(_ => StartEvent());

        // イベント中
        this.UpdateAsObservable()
            .Where(_ => _timeDic["start"] && !_timeDic["end"])
            .Subscribe(_ => NowEvent());

        // イベントの終了
        this.UpdateAsObservable()
            .Where(_ => _timeDic["end"])
            .Take(1)
            .Subscribe(_ => EndEvent());

    }

    /// <summary>
    /// タイマーをカウント
    /// </summary>
    /// <returns>1秒毎</returns>
    IEnumerator AddTime()
    {
        while (!_timeDic["end"])
        {
            yield return new WaitForSeconds(1);
            _timer++;
        }
    }

    /// <summary>
    /// イベントの開始した瞬間
    /// </summary>
    /// <returns>開始した瞬間：true, 開始した瞬間ではない：false</returns>
    public bool StartEvent()
    {
        if (_timeDic["start"]) return false;
        if(_timer == _ignitionTime)
        {
            Debug.Log("イベント開始");
            _timeDic["start"] = true;
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
        if (_timeDic["start"] && !_timeDic["end"])
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
        if (_timeDic["end"])
        {
            Debug.Log("イベント終了");
            StopAllCoroutines();
            return true;
        }
        return false;
    }
}

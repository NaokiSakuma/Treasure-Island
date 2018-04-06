using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TimerEventTrigger : MonoBehaviour,IEventTrigger {

    // イベントが発火する時間
    [SerializeField]
    private int _ignitionTime = 0;
    // タイマー
    private int _timer = 0;
    
    // タイマーイベントを管理するdictionary
    private Dictionary<string, bool> timeDic = new Dictionary<string, bool>();

    // イベント終了
    public bool End
    {
        set { timeDic["end"] = value; }
    }

    // タイマーをスタート
    public bool Count
    {
        set { timeDic["count"] = value; }
    }
    void Awake()
    {
        // 登録
        timeDic.Add("start", false);
        timeDic.Add("end", false);
        timeDic.Add("count", false);
    }
    void Start()
    {
        // タイマーを動かしている時
        this.UpdateAsObservable()
            .Where(_ => timeDic["count"])
            .First(_ => timeDic["count"])
            .Subscribe(_ => StartCoroutine(AddTime()));
    }

    /// <summary>
    /// タイマーをカウント
    /// </summary>
    /// <returns>1秒毎</returns>
    IEnumerator AddTime()
    {
        while (!timeDic["end"])
        {
            yield return new WaitForSeconds(1);
            _timer++;
        }
    }

    /// <summary>
    /// イベントの開始
    /// </summary>
    /// <returns>開始している：true, 開始していない：false</returns>
    public bool StartEvent()
    {
        if (timeDic["start"]) return false;
        if(_timer == _ignitionTime)
        {
            Debug.Log("イベント開始");
            timeDic["start"] = true;
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
        if (timeDic["start"] && !timeDic["end"])
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
        if (timeDic["end"])
        {
            Debug.Log("イベント終了");
            StopAllCoroutines();
            return true;
        }
        return false;
    }
}

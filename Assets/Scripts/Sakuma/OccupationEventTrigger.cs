using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class OccupationEventTrigger : EventTrigger {

    // 占拠されるとイベントが発火するオブジェクト
    [SerializeField]
    private List<bool> _wantOccupy = new List<bool>();

    // 今の占拠状態
    [SerializeField]
    private List<bool> _occupy = new List<bool>();

    // 占拠されるとイベントが発火するオブジェクトと今の占拠状態で合致しているtrueが何個存在してほしいか
    [SerializeField]
    private int wantSameNum = 0;

    protected override void Awake()
    {
        // 登録
        _eventDic.Add("start", false);    // イベントの開始
        _eventDic.Add("end", false);      // イベントの終了
        _eventDic.Add("occupation", false);    // ボスの生存状態 ※占拠しているオブジェクトを管理するスクリプトあるならそっから持って来た方がいいと思う
        for(int i= 0; i< _wantOccupy.Count; i++)
        {
            _occupy.Add(false);
        }
    }

    /// <summary>
    /// 占拠しているオブジェクトのSetter
    /// </summary>
    /// <param name="i">要素数</param>
    /// <param name="value">true：占拠している, false：占拠していない</param>
    public void SetOccupyIsland(int i, bool value)
    {
        if (i >= _occupy.Count) return;
        _occupy[i] = value;
    }

    /// <summary>
    /// 占拠されるとイベントが発火するオブジェクトと今の占拠状態の合致しているtrueの数
    /// </summary>
    /// <returns>true：イベントを発火させる, false：させない</returns>
    private bool CheckSameNum()
    {
        int sameCount = 0;
        for(int i= 0; i<_occupy.Count; i++)
        {
            if (_occupy[i] && _wantOccupy[i]) sameCount++;
        }
        if (sameCount >= wantSameNum) return true;
        return false;
    }
    void Start()
    {
        // イベントを発火させる
        this.UpdateAsObservable()
            .Where(_ => CheckSameNum())
            .Subscribe(_ => _eventDic["occupation"] = true);

        // イベントの開始した瞬間
        this.UpdateAsObservable()
            .Where(_ => _eventDic["occupation"] && !_eventDic["end"])
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
        if (_eventDic["occupation"] && !_eventDic["end"])
        {
            Debug.Log("イベント開始");
            _eventDic["start"] = true;
            return true;
        }
        return false;
    }
}

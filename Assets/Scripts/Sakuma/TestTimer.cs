using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
public class TestTimer : MonoBehaviour {
    [SerializeField]
    private TimerEventTrigger timerEvent = null;

    [SerializeField]
    private bool end = false;

    // Use this for initialization
    void Start () {
		if(timerEvent == null)
        {
            Debug.LogError("イベントが設定されていません");
        }
	}
	
	// Update is called once per frame
	void Update () {
        timerEvent.StartEvent();
        timerEvent.NowEvent();
        timerEvent.IsEnd = end;
        timerEvent.EndEvent();
	}
}

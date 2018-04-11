using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
public class AudioTest : MonoBehaviour {
    [SerializeField]
    private bool _nextBgm = false;
    [SerializeField]
    private bool _nextSe1 = false;
    [SerializeField]
    private bool _nextSe2 = false;

    // Use this for initialization
    void Start () {
        AudioManager.Instance.PlayBGM(AUDIO.BGM_PLAYBGM);
        //this.UpdateAsObservable()
        //    .First()
        //    .Where(_ => _nextBgm)
        //    .Subscribe(_ => AudioManager.Instance.PlayBGM(AUDIO.BGM_TITLEBGM));
        //this.UpdateAsObservable()
        //    .Where(_ => _nextSe1)
        //    .Subscribe(_ => AudioManager.Instance.PlaySE(AUDIO.SE_BATTLE));
        //this.UpdateAsObservable()
        //    .Where(_ => _nextSe2)
        //    .Subscribe(_ => AudioManager.Instance.PlaySE(AUDIO.SE_OCCUPATION));
        //Observable.Interval(TimeSpan.FromMilliseconds(3000))
        //    .Subscribe(_ =>
        //    {
        //        AudioManager.Instance.PlaySE(AUDIO.SE_SHIP);
        //        //AudioManager.Instance.PlaySE(AUDIO.SE_BATTLE);
        //    }
        //    );
        //SoundManager.Instance.PlayBgm(AUDIO.BGM_PLAYBGM);
    }

    // Update is called once per frame
    void Update () {
		if(_nextBgm)
        {
            AudioManager.Instance.PlayBGM(AUDIO.BGM_TITLEBGM,0.5f);
            _nextBgm = false;
        }
        if (_nextSe1)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SHIP);
            _nextSe1 = false;
        }
        if (_nextSe2)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SEPARATE);
            _nextSe2 = false;
        }

    }
}

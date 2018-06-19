using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using System;
using UniRx.Triggers;
using UnityEngine.UI;

public class PauseBackToSelect : SimplePauseItem
{

    [SerializeField]
    private string _text = "ステージセレクトへ";
    //フェードスさせるオブジェクト
    //リセット機能
    [SerializeField]
    private GameObject pauseReset = null;
    //ポーズから戻る機能ゲームに
    [SerializeField]
    private GameObject pauseBackToGame;
    //シーン遷移をするか
    private bool isSceneTrans = false;

    void Start()
    {
        GetComponentInChildren<TextMesh>().text = _text;

    }


    public override void OnClick()
    {
        if (isSceneTrans)
            return;
        //フェードフェードモードをフェードアウトにする
        CirecleFade.Instance.Play(CirecleFade.FadeMode.Out);
        //シーン遷移の処理をおこなった
        isSceneTrans = true;
        //他の処理を停止させる
        pauseReset.GetComponent<PauseReset>().CanReset = false;
        pauseBackToGame.GetComponent<PauseBackToGame>().CanPauseBackToGame = false;
        //シーン遷移が終わった際シーン移行
        Observable.Timer(TimeSpan.FromSeconds(CirecleFade.Instance.FadeTime))
        .Subscribe(x =>
        {
            SceneManager.LoadScene("StageSelect");
        });

    }
}



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
    [SerializeField]
    private GameObject fade = null;
    //フェードの親にするキャンバス
    [SerializeField]
    private GameObject canvas = null;
    //リセット機能
    [SerializeField]
    private GameObject pauseReset = null;
    //ポーズから戻る機能ゲームに
    [SerializeField]
    private GameObject pauseBackToGame = null;
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
        //フェードオブジェクト生成
        Image fadeobj = Instantiate(fade).GetComponent<Image>();
        //キャンバスを親に設定する
        fadeobj.gameObject.transform.SetParent(canvas.transform, false);
        //フェードフェードモードをフェードアウトにする
        fadeobj.GetComponent<CirecleFade>().SetFadeMode(CirecleFade.FadeMode.Out);
        //シーン遷移の処理をおこなった
        isSceneTrans = true;
        //
        pauseReset.GetComponent<PauseReset>().CanReset = false;
        pauseBackToGame.GetComponent<PauseBackToGame>().CanPauseBackToGame = false;
        //シーン遷移が終わった際シーン移行
        Observable.Timer(TimeSpan.FromSeconds(fadeobj.GetComponent<CirecleFade>().FadeTime))
        .Subscribe(x =>
        {
            SceneManager.LoadScene("StageSelect");
        });
        // SEを鳴らす
        AudioManager.Instance.ChangeVolume(Pausable.Instance.pausing ? 1f : 0.2f, 1f);
        AudioManager.Instance.PlaySE(AUDIO.SE_POSE);
    }
}



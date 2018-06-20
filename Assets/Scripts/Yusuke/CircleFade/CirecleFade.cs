using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UniRx;
using System;


public class CirecleFade : SingletonMonoBehaviour<CirecleFade>
{

    //フェードにかける時間
    private  float fadeTime = 1.0f;
    public float FadeTime
    {
        set { fadeTime = value; }
        get { return fadeTime; }
    }

    [SerializeField]
    bool startActive =  false;

    //現在の時間
    private float time = 0.0f;

    //フェードの種類
    public enum FadeMode
    {
        In,//フェードイン
        Out,//フェードアウト
    }

    //音
    private AudioSource sound;


    // Use this for initialization
    void Start () {
        //シェーダーに現在の進行時間を渡す
        GetComponent<Image>().material.SetFloat("_FadeTime", 0);
        sound = GetComponent<AudioSource>();
        gameObject.SetActive(startActive);

        this.ObserveEveryValueChanged(x => gameObject.activeSelf)
       .Where(x => gameObject.activeSelf)
       .Subscribe(_ =>
      {
          //音再生
          sound.PlayOneShot(sound.clip);
          TimeReset();
      });
    }


    // Update is called once per frame
    void Update () {
        time += Time.deltaTime * 1.0f;
        //シェーダーに現在の進行時間を渡す
        GetComponent<Image>().material.SetFloat("_FadeTime", time  / fadeTime);

        const float offset = 0.1f;        //シーン切り替えをした際にフェードが消えると都合がよくない

        if (time / fadeTime >= 1.0f + offset)
            gameObject.SetActive(false);
    }

    /// <summary>
    /// 時間をリセット
    /// </summary>
    private void TimeReset()
    {
        time = 0.0f;
    }

    /// <summary>
    /// フェードモードを設定
    /// </summary>
    /// <param name="fade">フェードの種類</param>
    private void SetFadeMode(FadeMode fade)
    {
        GetComponent<Image>().material.SetFloat("_FadeMode", (float)fade);
    }

    /// <summary>
    /// 開始する
    /// </summary>
    /// <param name="fade">フェードの種類</param>
    public void Play(FadeMode fade)
    {

        gameObject.SetActive(true);
        SetFadeMode(fade);
        TimeReset();
        //シェーダーに現在の進行時間を渡す
        GetComponent<Image>().material.SetFloat("_FadeTime", 0);
    }

}

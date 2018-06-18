using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class CirecleFade : MonoBehaviour {

    //フェードにかける時間
    private  float fadeTime = 1.0f;
    public float FadeTime
    {
        set { fadeTime = value; }
        get { return fadeTime; }
    }

    //現在の時間
    private float time = 0.0f;

    //フェードの種類
    public enum FadeMode
    {
        In,
        Out
    }
    // Use this for initialization
    void Start () {
        //シェーダーに現在の進行時間を渡す
        GetComponent<Image>().material.SetFloat("_FadeTime", 0);
    }

    // Update is called once per frame
    void Update () {
        time += Time.deltaTime;
        //シェーダーに現在の進行時間を渡す
        GetComponent<Image>().material.SetFloat("_FadeTime", time  / fadeTime);
    }

    /// <summary>
    /// 時間をリセット
    /// </summary>
    public void TimeReset()
    {
        time = 0;
    }

    /// <summary>
    /// フェードモードを設定
    /// </summary>
    /// <param name="fade">フェードの種類</param>
    public void SetFadeMode(FadeMode fade)
    {
        GetComponent<Image>().material.SetFloat("_FadeMode", (float)fade);
    }
}

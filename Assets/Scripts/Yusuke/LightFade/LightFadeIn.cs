using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;
using System;

public class LightFadeIn : MonoBehaviour
{

    //フェード時間
    private float fadeTime = 1.0f;
    public  float FadeTime
    {
        get { return fadeTime; }
    }
    //カメラ移動
    private float cameraMoveZ = 0.5f;
    //カメラの初期座標
    private Vector3 cameraStartPos;

    Material fadePanelMaterial = null;
    Color changeColor = new Color(1, 1, 1, 1);
    //時間
    float time = 0;

    
    void Start()
    {
        time = 0;
        fadeTime = 1.0f;
        Color color = gameObject.GetComponent<Image>().color;
        gameObject.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1);
    }



    public void Play()
    {
        Image fadePanel = null;

        // カメラが次のステージに入るように向けてからステージ遷移
        Sequence seq = DOTween.Sequence()
        .OnStart(() =>
        {
            // SEを鳴らす
            AudioManager.Instance.PlaySE(AUDIO.SE_DOORZOOM);

            //// フェード用パネルの生成
            fadePanel = gameObject.GetComponent<Image>();
            fadePanelMaterial = gameObject.GetComponent<Image>().material;

            //パネル座標
            Vector3 panelPos = fadePanel.transform.position;
            fadePanel.transform.position = new Vector3(panelPos.x, panelPos.y, panelPos.z + cameraMoveZ);

        })
        .AppendCallback(() =>
        {
            // フェード用パネルのアルファ値を半分まで上げる
            DOTween.ToAlpha(
            () => changeColor,
            color => changeColor = color,
            0f,
            fadeTime)
            .OnComplete(() =>
            {
            });
        });
        seq.Play();

    }

    void Update()
    {
        Color color = gameObject.GetComponent<Image>().color;
        color.a = time / fadeTime;
        color.a = 1 - color.a;
        gameObject.GetComponent<Image>().material.SetColor("_Color", color);
        time+= Time.deltaTime;
    }
}

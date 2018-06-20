using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;
using System;

public class LightFadeIn : SingletonMonoBehaviour<LightFadeIn>
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
    //フェードするか
    bool isFead= false;

    public void Play()
    {
        gameObject.SetActive(true);
       // gameObject.transform.parent.gameObject.SetActive(true);

        Image fadePanel = null;

        // カメラが次のステージに入るように向けてからステージ遷移
        Sequence seq = DOTween.Sequence()
        .OnStart(() =>
        {
            // SEを鳴らす
            AudioManager.Instance.PlaySE(AUDIO.SE_DOORZOOM);

            // フェード用パネルの生成
            fadePanel = gameObject.GetComponent<Image>();
            fadePanelMaterial = gameObject.GetComponent<Image>().material;

            // フェード用パネルの透明度を1にする
            isFead = true;

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
                gameObject.SetActive(false);
                //gameObject.transform.parent.gameObject.SetActive(false);
                isFead = false;
                time = 0;
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

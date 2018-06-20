using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

public class LightFadeIn : SingletonMonoBehaviour<LightFadeIn>
{

    // フェード用パネル
    [SerializeField]
    Canvas _fadeInCanvas = null;

    //フェード時間
    private float fadeTime = 4.0f;
    public  float FadeTime
    {
        get { return fadeTime; }
    }
    //カメラ移動
    private float cameraMoveZ = 0.5f;
    //カメラの初期座標
    private Vector3 cameraStartPos;

    public void Play()
    {

                Canvas fadeInCanvas = null;
                Image fadePanel = null;

                // カメラが次のステージに入るように向けてからステージ遷移
                Sequence seq = DOTween.Sequence()
                .OnStart(() =>
                {
                    // SEを鳴らす
                    AudioManager.Instance.PlaySE(AUDIO.SE_DOORZOOM);

                    // フェード用パネルの生成
                    fadeInCanvas = Instantiate(_fadeInCanvas);
                    fadePanel = fadeInCanvas.GetComponentInChildren<Image>();

                    // フェード用パネルの透明度を０にする
                    Color fadePanelColor = fadePanel.material.color;
                    fadePanelColor.a = 1f;
                    fadePanel.material.color = fadePanelColor;

                    //パネル座標
                    Vector3 panelPos = fadePanel.transform.position;
                    fadePanel.transform.position = new Vector3(panelPos.x, panelPos.y, panelPos.z + cameraMoveZ);
                    //このエフェクトにより、カメラが移動する為、初期座標がずれる。これを防ぐために初期座標をずらしておく
                    cameraStartPos = Camera.main.transform.position;
                    Camera.main.transform.position = new Vector3(cameraStartPos.x, cameraStartPos.y, cameraStartPos.z + cameraMoveZ);
                })
                .AppendCallback(() =>
                {
                    // フェード用パネルのアルファ値を半分まで上げる
                    DOTween.ToAlpha(
                    () => fadePanel.material.color,
                    color => fadePanel.material.color = color,
                    0f,
                    fadeTime
                );
                })
                .Join(Camera.main.transform.DOMove(new Vector3(cameraStartPos.x, cameraStartPos.y, cameraStartPos.z), fadeTime));

                seq.Play();
    }
}

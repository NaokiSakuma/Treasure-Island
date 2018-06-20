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


    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
		
	}


    public void Play()
    {

        this.FixedUpdateAsObservable()
            .Take(1)
            .Subscribe(_ =>
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
                })
                .AppendCallback(() =>
                {
                    // フェード用パネルのアルファ値を半分まで上げる
                    DOTween.ToAlpha(
                    () => fadePanel.material.color,
                    color => fadePanel.material.color = color,
                    0f,
                    2f
                );
                })
                .Join(Camera.main.transform.DOMove(new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.5f), 2f));

                seq.Play();
            });

    }
}

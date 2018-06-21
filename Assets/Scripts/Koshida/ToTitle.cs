using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using System;

namespace Konji
{
    public class ToTitle : MonoBehaviour
    {
        public void OnClick()
        {
            //フェードを開始する
            FadeManager.Instance.OutPlay(FadeManager.FadeKind.Circle);
            Observable.Timer(TimeSpan.FromSeconds(FadeManager.Instance.FadeTime))
            .Subscribe(x =>
            {
                SceneManager.LoadScene("TitleScene");
                AudioManager.Instance.PlaySE(AUDIO.SE_POSE);
            });
        }
    }
}
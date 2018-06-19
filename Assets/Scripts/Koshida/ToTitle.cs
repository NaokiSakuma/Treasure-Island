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
            CirecleFade.Instance.Play(CirecleFade.FadeMode.Out);
            Observable.Timer(TimeSpan.FromSeconds(CirecleFade.Instance.GetComponent<CirecleFade>().FadeTime))
            .Subscribe(x =>
            {
                SceneManager.LoadScene("TitleScene");
            });

        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UniRx;

namespace GucchiCS
{
    public class ButtonOfGameover : MonoBehaviour
    {
        // シーン名
        [SerializeField]
        string _sceneName = "";

        // クリック時
        public void OnClickRetry()
        {
            // SEを鳴らす
            AudioManager.Instance.PlaySE(AUDIO.SE_BUTTON);

            // 現在のステージ番号を取得
            int stageNo = StageManager.Instance.StageNo;
            //フェード終了後ステージオブジェクトを有効にする
            FadeManager.Instance.OutPlay(FadeManager.FadeKind.Circle);
            Observable.Timer(TimeSpan.FromSeconds(FadeManager.Instance.FadeTime))
            .Subscribe(x =>
            {
                SceneManager.LoadScene(_sceneName + stageNo.ToString());
            });


        }

        // クリック時
        public void OnClickStageSelect()
        {
            // SEを鳴らす
            AudioManager.Instance.PlaySE(AUDIO.SE_BUTTON);
            //フェードモードを開始する
            FadeManager.Instance.OutPlay(FadeManager.FadeKind.Circle);
            Observable.Timer(TimeSpan.FromSeconds(1))
            .Subscribe(x =>
            {
                SceneManager.LoadScene(_sceneName);
            });

        }
    }
}
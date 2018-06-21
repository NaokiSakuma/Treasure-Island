using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GucchiCS
{
    public class ButtonOfClear : MonoBehaviour
    {
        // クリアオブジェクト
        Clear _clearObject = null;

        // Canvas
        [SerializeField]
        Canvas _fadeInCanvas = null;

        // クリアUI
        [SerializeField]
        Text _clearText = null;

        // 各種ボタン
        [SerializeField]
        Button _stageSelectButton = null;
        [SerializeField]
        Button _nextStageButton = null;

        // シーン名
        [SerializeField]
        string _sceneName = "";

        // ボタンがクリックされたときの処理
        public void OnClick()
        {
            // UIを無効化
            _clearText.text = "";
            _stageSelectButton.gameObject.SetActive(false);
            _nextStageButton.gameObject.SetActive(false);

            Canvas fadeInCanvas = null;
            Image fadePanel = null;

            //ライトのフェードアウトを行ったことを保存
            FadeManager.Instance.LastFadeout = FadeManager.FadeKind.Light;

            // カメラが次のステージに入るように向けてからステージ遷移
            Sequence seq = DOTween.Sequence()
                .OnStart(() =>
                {
                    // フェード用パネルの生成
                    fadeInCanvas = Instantiate(_fadeInCanvas);
                    fadePanel = fadeInCanvas.GetComponentInChildren<Image>();

                    // フェード用パネルの透明度を０にする
                    Color fadePanelColor = fadePanel.material.color;
                    fadePanelColor.a = 0f;
                    fadePanel.material.color = fadePanelColor;

                    // SEを鳴らす
                    AudioManager.Instance.PlaySE(AUDIO.SE_DOORZOOM);
                })
                .AppendCallback(() =>
                {
                    // フェード用パネルのアルファ値を半分まで上げる
                    DOTween.ToAlpha(
                        () => fadePanel.material.color,
                        color => fadePanel.material.color = color,
                        1f,
                        2f
                    );
                })
                .Join(Camera.main.transform.DOMove(new Vector3(_clearObject.transform.position.x, _clearObject.transform.position.y, _clearObject.transform.position.z - 0.5f), 2f))
                .AppendCallback(() =>
                {
                    if (_sceneName == SingletonName.STAGE_NAME)
                    {
                        // 現在のステージ番号を取得
                        int stageNo = StageManager.Instance.StageNo;

                        if (stageNo + 1 > StageManager.MAX_STAGE_NUM)
                            stageNo = 0;
                        //ライトのフェードアウトを行ったことを保存
                        FadeManager.Instance.LastFadeout = FadeManager.FadeKind.Light;

                        // 次のステージへ遷移
                        string stageName = _sceneName + (++stageNo).ToString();
                        SceneManager.LoadScene(stageName);
                    }
                    else
                    {
                        SceneManager.LoadScene(_sceneName);
                    }
                });

            seq.Play();
        }

        // クリアオブジェクトの設定
        public Clear ClearObject
        {
            set { _clearObject = value; }
        }
    }
}
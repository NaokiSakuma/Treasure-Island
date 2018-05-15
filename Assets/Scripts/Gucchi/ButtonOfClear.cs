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
            // 透明色
            Color noAlpha = new Color(1f, 1f, 1f, 0f);

            // UIを透明化
            _clearText.text = "";
            ColorBlock colors = new ColorBlock();
            colors.normalColor = noAlpha;
            colors.highlightedColor = noAlpha;
            _stageSelectButton.colors = colors;
            _stageSelectButton.GetComponentInChildren<Text>().text = "";
            _nextStageButton.colors = colors;
            _nextStageButton.GetComponentInChildren<Text>().text = "";

            Canvas fadeInCanvas = null;
            Image fadePanel = null;

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
                    if (_sceneName == "stage")
                    {
                        // 現在のステージ番号を取得
                        int stageNo = GameManagerKakkoKari.Instance.StageNo;

                        if (stageNo >= GameManagerKakkoKari.MAX_STAGE_NUM)
                            stageNo = 0;

                        // 次のステージへ遷移
                        //string stageName = _sceneName + (++stageNo).ToString();
                        //SceneManager.LoadScene(stageName);
                        SceneManager.LoadScene("Scenes/ShadowProto");
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
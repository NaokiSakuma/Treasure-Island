using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GucchiCS
{
    public class ButtonNextStage : MonoBehaviour
    {
        // クリアオブジェクト
        Clear _clearObject = null;

        // Canvas
        [SerializeField]
        Canvas _fadeInCanvas = null;

        // クリアUI
        [SerializeField]
        Text clearText = null;
        [SerializeField]
        Button stageSelectButton = null;

        // ボタンがクリックされたときの処理
        public void OnClick()
        {
            // 透明色
            Color noAlpha = new Color(1f, 1f, 1f, 0f);

            // UIを透明化
            clearText.text = "";
            ColorBlock colors = new ColorBlock();
            colors.normalColor = noAlpha;
            colors.highlightedColor = noAlpha;
            stageSelectButton.colors = colors;
            stageSelectButton.GetComponentInChildren<Text>().text = "";
            transform.GetComponent<Button>().colors = colors;
            transform.GetComponentInChildren<Text>().text = "";

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
                    // 現在のステージ番号を取得
                    int stageNo = GameManagerKakkoKari.Instance.StageNo;

                    if (stageNo >= GameManagerKakkoKari.MAX_STAGE_NUM)
                        stageNo = 0;

                    // 次のステージへ遷移
                    //string stageName = "stage" + (++stageNo).ToString();
                    //SceneManager.LoadScene(stageName);
                    SceneManager.LoadScene("Scenes/ShadowProto");
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
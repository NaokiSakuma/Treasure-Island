using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GucchiCS
{
    public class Door : MonoBehaviour
    {
        // 扉ID
        int _doorID = 0;

        // ガード
        [SerializeField]
        GameObject _guard = null;

        // フェード用パネル
        [SerializeField]
        Canvas _fadeInCanvas = null;

        // クリックされたらシーン遷移
        public void OnClick()
        {
            Debug.Log("click!");

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
                            // ガードの削除
                            Destroy(_guard);

                            // SEを鳴らす
                            AudioManager.Instance.PlaySE(AUDIO.SE_DOORZOOM);

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
                        .Join(Camera.main.transform.DOMove(new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.5f), 2f))
                        .AppendCallback(() =>
                        {
                            // ステージへ遷移
                            string stageSceneName = SingletonName.STAGE_NAME + (_doorID + 1).ToString();
                            SceneManager.LoadScene(stageSceneName);
                        });

                    seq.Play();
                });
        }

        // 扉IDを設定
        public int DoorID
        {
            get { return _doorID; }
            set { _doorID = value; }
        }
    }
}
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

        // 通常のガードスプライト
        [SerializeField, Tooltip("ガード")]
        Sprite _stageDoor = null;

        // クリア済み用ガードスプライト
        [SerializeField, Tooltip("クリア済み扉")]
        Sprite _correctStageDoor = null;

        // フェード用パネル
        [SerializeField]
        Canvas _fadeInCanvas = null;

        void Start()
        {
            // クリア情報をリセット
            this.LateUpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.C))
                .Subscribe(_ =>
                {
                    PlayerPrefs.SetString("stage" + (_doorID + 1).ToString(), "");
                    _guard.GetComponent<SpriteRenderer>().sprite = _stageDoor;
                });
        }

        // クリックされたらシーン遷移
        public void OnClick()
        {
            // ライト変更ボタンの削除
            StageSelectManager.Instance.DisposeLightChangeButton();

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
                            _guard.SetActive(false);

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

        // マウスポインタが触れたらガードを消す
        public void OnSelectEnter()
        {
            _guard.SetActive(false);
        }

        // マウスポインタが離れたらガードを出す
        public void OnSelectExit()
        {
            _guard.SetActive(true);
        }

        // クリア済みのときの処理
        public void SetClearState()
        {
            if (PlayerPrefs.GetString("stage" + (_doorID + 1).ToString()) == "Clear")
            {
                _guard.GetComponent<SpriteRenderer>().sprite = _correctStageDoor;
            }
        }

        // 扉IDを設定
        public int DoorID
        {
            get { return _doorID; }
            set { _doorID = value; }
        }
    }
}
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;


namespace GucchiCS
{
    public class StageInitializer : MonoBehaviour
    {
        // プレイヤー
        [SerializeField]
        Transform _player = null;

        // クリアオブジェクト
        [SerializeField]
        GameObject _clear = null;

        // ライト
        [SerializeField]
        GameObject _light = null;

        // ライト
        [SerializeField]
        GameObject _lightSpot = null;

        // 点滅時間
        [SerializeField]
        float _blinkTime = 0.1f;

        // 点滅回数
        [SerializeField]
        int _blinkNum = 3;

        // 最後長めに暗くする時間
        [SerializeField]
        float _lastLight = 0.5f;

        // 開始時のカメラ設定
        [SerializeField]
        Camera _defaultCamera = null;

        // ゴール地点
        [SerializeField]
        Vector3 _goalPos = new Vector3(0f, 0f, 8.9f);

        //最初のタイトルか
        static bool isFirstTitle = true;
        // シーケンス
        enum SEQUENCE : int
        {
            START,
            LIGHTED,
            CAMERA_SETTED,
            CORRECTED
        }
        SEQUENCE _sequence = SEQUENCE.START;

        // スキップボタン
        [SerializeField]
        Button _skipButton = null;

        // スキップは１回だけ呼べるようにする
        bool _isSkiped = false;

        // Use this for initialization
        void Awake()
        {
            _player.localScale = Vector3.zero;
            _clear.transform.localScale = Vector3.zero;
            _clear.transform.position = new Vector3(_player.position.x, _player.position.y, 8.9f);

            // カメラの初期化
            Camera.main.transform.position = _defaultCamera.transform.position;
            Camera.main.transform.localRotation = _defaultCamera.transform.localRotation;
         
        }

        void Start()
        {
            Color skipCol = _skipButton.GetComponent<Image>().color;
            _skipButton.GetComponent<Image>().color = new Color(skipCol.r, skipCol.g, skipCol.b, 0);

            Color skipCol3 = _skipButton.GetComponent<Image>().GetComponentInChildren<Image>().GetComponentInChildren<Text>().color;
            _skipButton.GetComponent<Image>().GetComponentInChildren<Image>().GetComponentInChildren<Text>().color = new Color(skipCol3.r, skipCol3.g, skipCol3.b, 0);

            Color skipCol2 = _skipButton.GetComponent<Image>().GetComponentInChildren<Image>().GetComponentInChildren<Text>().transform.parent.GetComponent<Image>().color;
            _skipButton.GetComponent<Image>().GetComponentInChildren<Image>().GetComponentInChildren<Text>().transform.parent.GetComponent<Image>().color = new Color(skipCol2.r, skipCol2.g, skipCol2.b, 0);

            Debug.Log(_skipButton.GetComponent<Image>().GetComponentInChildren<Image>().GetComponentInChildren<Text>().transform.parent.GetComponent<Image>().gameObject.name);

            // BGMを再生
            PlaybackBGM();
            if(!isFirstTitle)
            {
                ////フェードフェードモードをフェードアウトにする
                FadeManager.Instance.InPlay();
                Debug.Log("initFade");
            }
            isFirstTitle = false;
            Observable.Timer(TimeSpan.FromSeconds(FadeManager.Instance.FadeTime))
         .Subscribe(x =>
         {
                // ライトの点滅でスタート
                _light.gameObject.SetActive(false);
             this.UpdateAsObservable()
                 .Take(1)
                 .Subscribe(_ => Blink());

                // ライト点灯後カメラを移動させる
                this.ObserveEveryValueChanged(_ => _sequence)
                 .Where(_ => _sequence == SEQUENCE.LIGHTED)
                 .Take(1)
                 .Subscribe(_ => SetCamera());

                // カメラ移動後プレイヤーとゴールを出現させる
                this.ObserveEveryValueChanged(_ => _sequence)
                 .Where(_ => _sequence == SEQUENCE.CAMERA_SETTED)
                 .Take(1)
                 .Subscribe(_ => SetGame());

                // Xキーが押されたらスキップ
                this.LateUpdateAsObservable()
                 .Where(_ => Input.GetKeyDown(KeyCode.X))
                 .Take(1)
                 .Subscribe(_ => Skip());
             _skipButton.GetComponent<Image>().color = new Color(skipCol.r, skipCol.g, skipCol.b, 1);

             _skipButton.GetComponent<Image>().GetComponentInChildren<Image>().GetComponentInChildren<Text>().transform.parent.GetComponent<Image>().color = new Color(skipCol2.r, skipCol2.g, skipCol2.b, 1);

             _skipButton.GetComponent<Image>().GetComponentInChildren<Image>().GetComponentInChildren<Text>().color = new Color(skipCol3.r, skipCol3.g, skipCol3.b, 1);

         });

            // ボリュームを最大にする
            AudioManager.Instance.ChangeVolume(1f, 1f);
        }

        // 点滅
        void Blink()
        {
            Observable.FromCoroutine(BlinkCoroutine)
                .DelayFrame(10)
                .Where(_ => _sequence == SEQUENCE.START)        // スキップ時の遅延実行防止
                .Subscribe(_ =>
                {
                    _sequence = SEQUENCE.LIGHTED;
                });
        }

        // 点滅コルーチン
        IEnumerator BlinkCoroutine()
        {
            // 指定回数点滅する
            for (int i = 0; i < _blinkNum; i++)
            {
                _light.gameObject.SetActive(true);
                yield return new WaitForSeconds(_blinkTime);

                // スキップ時の遅延実行防止
                if (_sequence != SEQUENCE.START)
                {
                    break;
                }

                _light.gameObject.SetActive(false);
                yield return new WaitForSeconds(_blinkTime);
            }

            // 最後に長めに暗くする
            yield return new WaitForSeconds(_lastLight);
            _light.gameObject.SetActive(true);
        }

        // カメラ移動
        void SetCamera()
        {
            // オブジェクトの正面に寄ってからゲーム画面の正面に寄る
            Sequence seq = DOTween.Sequence()
                .OnStart(() => { })
                .Append(Camera.main.transform.DOLocalRotate(Vector3.zero, 2f))
                .Join(Camera.main.transform.DOMove(new Vector3(_light.transform.position.x, _light.transform.position.y, _light.transform.position.z - 9f), 2f).SetEase(Ease.InSine))
                .Append(Camera.main.transform.DOMove(ModeChanger.Instance.GetGameModeCameraPos, 2f))
                .AppendCallback(() => _sequence = SEQUENCE.CAMERA_SETTED);

            seq.Play();
        }

        // プレイヤーとゴールを出現させる
        void SetGame()
        {
            // スキップボタンを削除
            _isSkiped = true;
            Destroy(_skipButton.gameObject);

            // オブジェクトの影設定
            StageManager.Instance.SetObjectShadowMode();

            Sequence seq = DOTween.Sequence()
                .OnStart(() =>
                {
                    // プレイヤーの透明度を０にする
                    Color playerColor = _player.GetComponent<SpriteRenderer>().material.color;
                    playerColor.a = 0f;
                    _player.GetComponent<SpriteRenderer>().material.color = playerColor;

                    // クリアガードを非表示にする
                    _clear.transform.Find("ClearGuard").gameObject.SetActive(false);
                })
                .Append(_clear.transform.DOScale(Vector3.one, 1f))
                .AppendCallback(() =>
                {
                    // プレイヤーの透明度を最大にする
                    DOTween.ToAlpha(
                        () => _player.GetComponent<SpriteRenderer>().material.color,
                        color => _player.GetComponent<SpriteRenderer>().material.color = color,
                        1f,
                        1f
                    );
                })
                .Join(_player.DOScale(Vector3.one, 1f))
                .AppendCallback(() =>
                {
                    StageManager.Instance.IsPlay = true;
                })
                .Append(_clear.transform.DOScale(Vector3.zero, 1f))
                .AppendCallback(() =>
                {
                    _clear.transform.position = _goalPos;
                    _clear.transform.Find("ClearGuard").gameObject.SetActive(true);
                })
                .Join(_clear.transform.DOScale(Vector3.one, 1f))
                .AppendCallback(() =>
                {
                    _clear.transform.GetComponentInChildren<BoxCollider>().isTrigger = true;
                    _sequence = SEQUENCE.CORRECTED;
                })
                .OnComplete(() => _lightSpot.GetComponent<LightSpot>().IsStart = true);


            seq.Play();
        }

        // スタート演出スキップ
        public void Skip()
        {
            if (_isSkiped)
                return;

            // SEを鳴らす
            AudioManager.Instance.PlaySE(AUDIO.SE_BUTTON);

            // アニメーション中であればアニメーションをやめる
            DOTween.KillAll(true);

            // スキップボタンを削除
            Destroy(_skipButton.gameObject);

            // オブジェクトの影設定
            StageManager.Instance.SetObjectShadowMode();

            // ライト点灯
            _light.gameObject.SetActive(true);

            // 各オブジェクト、プレイヤーの初期化
            _player.localScale = Vector3.one;
            _clear.transform.localScale = Vector3.one;
            _clear.transform.position = _goalPos;
            _clear.transform.GetComponentInChildren<BoxCollider>().isTrigger = true;

            // ゲームモード用にカメラを合わせる
            Camera.main.transform.position = ModeChanger.Instance.GetGameModeCameraPos;
            Camera.main.transform.localRotation = Quaternion.identity;

            // ゴールオブジェクトのアニメーションスタート
            _lightSpot.GetComponent<LightSpot>().IsStart = true;

            // シーケンス更新
            StageManager.Instance.IsPlay = true;
            _sequence = SEQUENCE.CORRECTED;

            _isSkiped = true;
        }

        // BGMを再生
        void PlaybackBGM()
        {
            if (SceneManager.GetActiveScene().name == SingletonName.TITLE_SCENE)
            {
                AudioManager.Instance.PlayBGM(AUDIO.BGM_TITLE, AudioManager.BGM_FADE_SPEED_RATE_HIGH);
                return;
            }

            AudioManager.Instance.PlayBGM(AUDIO.BGM_STAGE, AudioManager.BGM_FADE_SPEED_RATE_HIGH);
        }
    }
}
﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GucchiCS
{
    public class Clear : MonoBehaviour
    {
        // プレイヤー
        [SerializeField]
        Transform _player = null;

        // クリアガード
        [SerializeField]
        GameObject _guard = null;

        // プレイヤーが中に入っていくまでの時間
        [SerializeField]
        float _goNextTime = 2f;

        // アニメーション待機時間
        [SerializeField]
        float _animationTime = 4f;

        // クリアUI
        [SerializeField]
        GameObject _clearUI = null;

        // ステージセレクトするボタン
        [SerializeField]
        ButtonOfClear _stageSelectButton = null;

        // 扉に入るアニメーションのときのカメラの距離
        [SerializeField]
        float _clearOutDistance = 0.8f;

        void Awake()
        {
            // クリアUIを始めは非表示にする
            _clearUI.SetActive(false);

            // Colliderバグ防止用
            transform.GetComponent<BoxCollider>().isTrigger = false;

            // プレイヤーがクリアオブジェクトに触れたとき
            this.OnTriggerEnterAsObservable()
                .Where(_ => StageManager.Instance.IsPlay)
                .Where(_ => !_player.GetComponent<Konji.PlayerControl>().IsDead)
                .Subscribe(col =>
                {
                    // クリア状態にする
                    ModeChanger.Instance.Mode = ModeChanger.MODE.CLEAR;
                    StageNoReader.Instance.Cleared();

                    // クリアを保存
                    PlayerPrefs.SetString("stage" + StageManager.Instance.StageNo.ToString(), "Clear");

                    // Colliderバグ防止用
                    transform.GetComponent<BoxCollider>().isTrigger = false;
                });

            // クリアロゴを表示
            this.UpdateAsObservable()
                .Where(mode => ModeChanger.Instance.Mode == ModeChanger.MODE.CLEAR)
                .Take(1)
                .Delay(System.TimeSpan.FromSeconds(_goNextTime))
                .Subscribe(_ =>
                {
                    // 中に入っていくように見せる（z軸とスケールを変える）
                    Vector3 clearPos = new Vector3(transform.position.x, transform.position.y, _player.transform.position.z);
                    Vector3 endPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + _clearOutDistance);
                    Sequence seq = DOTween.Sequence()
                        .OnStart(() =>
                        {
                            // クリアガードの削除
                            Destroy(_guard);
                        })
                        .Append(_player.DOMove(clearPos, _animationTime / 2))
                        .AppendCallback(() =>
                        {
                            // プレイヤーの透明度を０にする
                            DOTween.ToAlpha(
                                () => _player.GetComponent<SpriteRenderer>().material.color,
                                color => _player.GetComponent<SpriteRenderer>().material.color = color,
                                0f,
                                _animationTime / 2
                            );
                        })
                        .Join(_player.DOScale(0.8f, _animationTime / 2))
                        .Join(_player.DOMove(endPos, _animationTime / 2))
                        .AppendCallback(() => AppearClearUI());

                    seq.Play();
                });
        }

        // クリア時のUIを出す
        void AppearClearUI()
        {
            // タイトルシーンだった場合
            if (SceneManager.GetActiveScene().name == SingletonName.TITLE_SCENE)
            {
                _stageSelectButton.ClearObject = this;
                _stageSelectButton.OnClick();
            }
            else
            {
                Debug.Log("ここでクリアUI出すよ");

                // SEを鳴らす
                AudioManager.Instance.StopBGM();
                AudioManager.Instance.PlaySE(AUDIO.SE_CLEAR);

                // クリアUIの生成
                //GameObject clearUI = Instantiate(_clearUI);
                //clearUI.transform.SetParent(_canvas.transform, false);
                //foreach (Transform button in clearUI.transform)
                //{
                //    if (button.GetComponent<ButtonOfClear>() != null)
                //    {
                //        button.GetComponent<ButtonOfClear>().ClearObject = this;
                //    }
                //}

                // クリアUIの表示
                _clearUI.SetActive(true);
                foreach (Transform button in _clearUI.transform)
                {
                    if (button.GetComponent<ButtonOfClear>() != null)
                    {
                        button.GetComponent<ButtonOfClear>().ClearObject = this;
                    }
                }
            }
        }
    }
}
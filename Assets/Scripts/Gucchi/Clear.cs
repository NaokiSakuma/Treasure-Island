using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

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

        // Canvas
        [SerializeField]
        Canvas _canvas = null;

        // クリアUI
        [SerializeField]
        GameObject _clearUI = null;

        void Awake()
        {
            // プレイヤーがクリアオブジェクトに触れたとき
            this.OnCollisionEnterAsObservable()
                .Where(_ => GameManagerKakkoKari.Instance.IsPlay)
                .Subscribe(col =>
                {
                    Debug.Log("Clear enter!");

                    ModeChanger.Instance.Mode = ModeChanger.MODE.CLEAR;

                    GameManagerKakkoKari.Instance.IsPlay = false;
                });

            // クリアロゴを表示
            this.UpdateAsObservable()
                .Where(mode => ModeChanger.Instance.Mode == ModeChanger.MODE.CLEAR)
                .Take(1)
                .Delay(System.TimeSpan.FromSeconds(_goNextTime))
                .Subscribe(_ =>
                {
                    Debug.Log("クリアしました～");

                    // プレイヤーの画像を後ろ姿に変える

                    // 中に入っていくように見せる（z軸とスケールを変える）
                    Vector3 clearPos = new Vector3(transform.position.x, transform.position.y, _player.transform.position.z);
                    Vector3 endPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.3f);
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
                });
        }

        // クリア時のUIを出す
        void AppearClearUI()
        {
            Debug.Log("ここでクリアUI出すよ");
        }
    }
}
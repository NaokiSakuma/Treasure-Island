using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        // ゴール地点
        [SerializeField]
        Vector3 _goalPos = new Vector3(0f, 0f, 8.9f);

        // Use this for initialization
        void Awake()
        {
            _player.localScale = Vector3.zero;

            // プレイヤーとゴールを出現させる
            _clear.transform.position = new Vector3(_player.position.x, _player.position.y, 8.9f);
            Sequence seq = DOTween.Sequence()
                .OnStart(() =>
                {
                    // クリアオブジェクトのスケールを０にする
                    _clear.transform.localScale = Vector3.zero;

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
                .Append(_clear.transform.DOScale(Vector3.zero, 1f))
                .AppendCallback(() => 
                {
                    _clear.transform.position = _goalPos;
                    _clear.transform.Find("ClearGuard").gameObject.SetActive(true);
                })
                .Join(_clear.transform.DOScale(Vector3.one, 1f))
                .AppendCallback(() => 
                {
                    StageManager.Instance.IsPlay = true;
                });

            seq.Play();
        }
    }
}
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
        Transform _player;

        // クリアガード
        [SerializeField]
        GameObject _guard = null;

        // プレイヤーが中に入っていくまでの時間
        [SerializeField]
        float _goNextTime = 2f;

        // アニメーション待機時間
        [SerializeField]
        float _animationTime = 4f;

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
                            Destroy(_guard);
                        })
                        .Append(_player.DOMove(clearPos, _animationTime / 2))                        
                        .Append(_player.DOScale(0f, _animationTime / 2))
                        .Join(_player.DOMove(endPos, _animationTime / 2));
                });
        }

        // プレイヤー設定
        public Transform Player
        {
            set { _player = value; }
        }
    }
}

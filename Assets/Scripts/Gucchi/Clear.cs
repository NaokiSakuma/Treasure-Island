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

        // クリアエフェクトなどを出すまでの時間
        [SerializeField]
        float _adventLogoTime = 1f;

        void Awake()
        {
            // プレイヤーがクリアオブジェクトに触れたとき
            this.OnCollisionEnterAsObservable()
                .Subscribe(col =>
                {
                    Debug.Log("Clear enter!");

                    // プレイヤーをマスターに設定
                    ModeChanger.Instance.Player = _player;

                    ModeChanger.Instance.Mode = ModeChanger.MODE.CLEAR;
                });

            // クリアロゴを表示
            this.UpdateAsObservable()
                .Where(mode => ModeChanger.Instance.Mode == ModeChanger.MODE.CLEAR)
                .Take(1)
                .Delay(System.TimeSpan.FromSeconds(_adventLogoTime))
                .Subscribe(_ =>
                {
                    Debug.Log("クリアしました～");
                });
        }

        /* プロパティ */

        // プレイヤーを設定
        public Transform Player
        {
            set { _player = value; }
        }
    }
}

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
        Transform _player;

        // ロゴを出すまでの時間
        [SerializeField]
        float _adventLogoTime = 1f;

        void Awake()
        {
            // プレイヤーがクリアオブジェクトに触れたとき
            this.OnCollisionEnterAsObservable()
                .Subscribe(col =>
                {
                    Debug.Log("Clear enter!");
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
    }
}

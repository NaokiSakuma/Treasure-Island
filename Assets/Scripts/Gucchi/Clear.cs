using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GucchiCS
{
    public class Clear : MonoBehaviour
    {
        // クリアロゴ
        public SpriteRenderer _clearLogo;

        // プレイヤー
        public Transform _player;

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
                .First()
                .Delay(System.TimeSpan.FromSeconds(2f))
                .Subscribe(_ =>
                {
                    Instantiate(_clearLogo, new Vector3(_player.position.x, _player.position.y, _player.position.z - 1f), Quaternion.identity);
                });
        }
    }
}

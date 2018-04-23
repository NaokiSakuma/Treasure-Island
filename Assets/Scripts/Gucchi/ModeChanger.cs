using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace GucchiCS
{
    public class ModeChanger : MonoBehaviour        // 後々シングルトンにしたほうがよさげ
    {
        // モード
        public enum MODE
        {
            GAME,
            CONTROL
        }
        MODE _mode = MODE.GAME;

        // カメラ
        public Camera _camera;

        // ゲームスクリーン
        public Transform _gameScreen;

        // ハイブリッドスポットライト
        public Transform _spotlight;

        // ゲームスクリーンまでの距離
        [SerializeField]
        float _gameScreenDistance = 5f;

        // ハイブリッドスポットライトまでの距離
        [SerializeField]
        float _spotlightDistance = 1f;

        // モードを変えるときの変更時間
        [SerializeField]
        float _changeTime = 2f;

        void Awake()
        {
            // ゲームモード時のカメラの座標
            Vector3 gameModePos = new Vector3(_camera.transform.position.x, _camera.transform.position.y, _gameScreen.position.z - _gameScreenDistance);

            // コントロールモード時のカメラの座標
            Vector3 controlModePos = new Vector3(_camera.transform.position.x, _camera.transform.position.y, _spotlight.position.z - _spotlightDistance);

            this.ObserveEveryValueChanged(_ => _mode)
                .Subscribe(_ =>
                {
                    // 移動中ならアニメーションをやめる
                    _camera.transform.DOComplete();

                    // 移動
                    switch (_mode)
                    {
                        // ゲームモード
                        case MODE.GAME:
                            _camera.transform.DOMove(gameModePos, _changeTime);
                            break;

                        // コントロールモード
                        case MODE.CONTROL:
                            _camera.transform.DOMove(controlModePos, _changeTime);
                            break;

                        default:
                            break;
                    }
                });
        }

        // モード変更
        public void ChangeMode()
        {
            _mode = _mode == MODE.GAME ? MODE.CONTROL : MODE.GAME;
        }

        /* プロパティ */

        // モード
        public MODE Mode
        {
            get { return _mode; }
        }
    }
}
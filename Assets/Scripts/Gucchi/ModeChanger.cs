using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace GucchiCS
{
    public class ModeChanger : SingletonMonoBehaviour<ModeChanger>
    {
        // モード
        public enum MODE
        {
            GAME,
            OBJECT_CONTROL,
            SPOTLIGHT_CONTROL
        }
        MODE _mode = MODE.GAME;

        // カメラ
        public Camera _camera;

        // ゲームスクリーン
        public Transform _gameScreen;

        // オブジェクトスクリーン
        public Transform _objectScreen;

        // ハイブリッドスポットライト
        public Transform _spotlight;

        // ゲームスクリーンまでの距離
        [SerializeField]
        float _gameScreenDistance = 5f;

        // オブジェクトスクリーンまでの距離
        [SerializeField]
        float _objectScreenDistance = 1f;

        // ハイブリッドスポットライトまでの距離
        [SerializeField]
        float _spotlightDistance = 1f;

        // モードを変えるときの変更時間
        [SerializeField]
        float _changeTime = 2f;

        void Start()
        {
            this.ObserveEveryValueChanged(_ => _mode)
                .Subscribe(_ =>
                {
                    // 移動中ならアニメーションをやめる
                    _camera.transform.DOComplete();

                    // z軸補正
                    var newPos = _camera.transform.position;
                    switch (_mode)
                    {
                        case MODE.GAME:                 newPos.z = _gameScreen.position.z + -_gameScreenDistance;       break;      // ゲームモード
                        case MODE.OBJECT_CONTROL:       newPos.z = _objectScreen.position.z + -_objectScreenDistance;   break;      // オブジェクトコントロールモード
                        case MODE.SPOTLIGHT_CONTROL:    newPos.z = _spotlight.position.z + -_spotlightDistance;         break;      // スポットライトコントロールモード
                        default: break;
                    }

                    // 移動
                    _camera.transform.DOMove(newPos, _changeTime);
                });
        }

        /* プロパティ */

        // モード
        public MODE Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }
    }
}
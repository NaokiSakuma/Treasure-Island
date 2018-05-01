﻿using DG.Tweening;
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
            OBJECT_CONTROL_SELECTED,
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

        // 選択中のオブジェクト
        GameObject _selectedObject = null;

        // sakuma
        [SerializeField]
        private RotateManager _isRotate = null;
        void Start()
        {
            // デフォルトカメラ座標
            var defaultPos = _camera.transform.position;

            // モード変更
            this.ObserveEveryValueChanged(_ => _mode)
                .Subscribe(_ =>
                {
                    ChangeMode(defaultPos);
                });

            // 選択オブジェクトの変更
            this.ObserveEveryValueChanged(_ => _selectedObject)
                .Where(_ => _selectedObject != null)
                .Where(_ => !_isRotate.IsRotate)
                .Subscribe(_ =>
                {
                    print("回転します");
                    ChangeSelectedObject();
                });
        }

        // モード変更処理
        void ChangeMode(Vector3 defaultPos)
        {
            if (_mode == MODE.OBJECT_CONTROL_SELECTED)
                return;

            // 移動中ならアニメーションをやめる
            _camera.transform.DOComplete();

            // 軸を安定させる
            var newPos = _camera.transform.position;
            newPos.x = defaultPos.x;
            newPos.y = defaultPos.y;

            // z軸補正
            switch (_mode)
            {
                case MODE.GAME:                         // ゲームモード
                    newPos.z = _gameScreen.position.z + -_gameScreenDistance;
                    break;

                case MODE.OBJECT_CONTROL:                // オブジェクトコントロールモード
                    newPos.z = _objectScreen.position.z + -_objectScreenDistance;
                    break;

                case MODE.SPOTLIGHT_CONTROL:            // スポットライトコントロールモード
                    newPos.z = _spotlight.position.z + -_spotlightDistance;
                    break;

                default:
                    break;
            }

            // 移動
            _camera.transform.DOMove(newPos, _changeTime);
        }

        // 選択オブジェクト変更処理
        void ChangeSelectedObject()
        {
            // 移動中ならアニメーションをやめる
            _camera.transform.DOComplete();

            // 選択したオブジェクトの手前に座標を設定
            var newPos = _camera.transform.position;
            newPos = new Vector3(_selectedObject.transform.position.x, _selectedObject.transform.position.y, _objectScreen.position.z - _objectScreenDistance);

            // 移動
            _camera.transform.DOMove(newPos, _changeTime);
        }

        /* プロパティ */

        // モード
        public MODE Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        // 選択したオブジェクトを設定
        public GameObject SelectedObject
        {
            set { _selectedObject = value; }
        }
    }
}
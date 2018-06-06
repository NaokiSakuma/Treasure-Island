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
            NONE,
            GAME,
            OBJECT_CONTROL,
            OBJECT_CONTROL_SELECTED,
            CLEAR
        }
        MODE _mode = MODE.GAME;

        // モードアイコン
        [SerializeField]
        ModeIcons _modeIcon = null;

        // ゲームスクリーン
        [SerializeField]
        Transform _gameScreen = null;

        //オブジェクト回転モードのカメラプレビュー
        [SerializeField]
        Transform _objRotateCamera = null;

        //// オブジェクトスクリーン
        //[SerializeField]
        //Transform _objectScreen = null;

        // プレイヤー
        [SerializeField]
        Transform _player = null;

        // ゲームスクリーンまでの距離
        [SerializeField]
        float _gameScreenDistance = 5f;

        //// オブジェクトスクリーンまでの距離
        //[SerializeField]
        //float _objectScreenDistance = 1f;

        // プレイヤーまでの距離
        [SerializeField]
        float _playerDistance = 3f;

        // モード変更中かどうか
        bool _isChanging = false;

        // モードを変えるときの変更時間
        [SerializeField]
        float _changeTime = 2f;

        // 選択中のオブジェクト
        GameObject _selectedObject = null;

        // オブジェクト選択時のカメラのx補正値
        [SerializeField]
        float _correctionXPos = 10f;

        // sakuma
        [SerializeField]
        private RotateManager _isRotate = null;

        void Start()
        {
            // モード変更
            this.ObserveEveryValueChanged(_ => _mode)
                .Where(_ => StageManager.Instance.IsPlay)
                .Subscribe(_ =>
                {
                    ChangeMode();
                });

            // 選択オブジェクトの変更
            this.ObserveEveryValueChanged(_ => _selectedObject)
                .Where(_ => _selectedObject != null)
                .Where(_ => !_isRotate.IsRotate)
                .Subscribe(_ =>
                {
                    ChangeSelectedObject();
                });
        }

        // モード変更処理
        void ChangeMode()
        {
            if (_mode == MODE.OBJECT_CONTROL_SELECTED)
                return;

            // 移動中ならアニメーションをやめる
            Camera.main.transform.DOComplete();

            // カメラのNearを戻す
            Camera.main.nearClipPlane = 5f;

            // オブジェクトの選択を解除する
            _selectedObject = null;

            // 軸を安定させる
            Vector3 newPos = Camera.main.transform.position;
            newPos.x = 0f;
            newPos.y = 0f;

            //カメラの角度
            var newRot = Vector3.zero;

            // z軸補正
            switch (_mode)
            {
                case MODE.GAME:                         // ゲームモード
                    newPos.z = _gameScreen.position.z + -_gameScreenDistance;
                    _modeIcon.CurrentMode = ModeIcons.Mode.Character;
                    break;

                case MODE.OBJECT_CONTROL:               // オブジェクトコントロールモード
                    newPos = _objRotateCamera.position;
                    newRot = _objRotateCamera.localEulerAngles;
                    _modeIcon.CurrentMode = ModeIcons.Mode.Object;
                    break;

                case MODE.CLEAR:                        // クリアモード
                    StageManager.Instance.IsPlay = false;
                    newPos = new Vector3(_player.position.x, _player.position.y, _gameScreen.position.z + -_playerDistance);
                    Camera.main.nearClipPlane = 0.3f;
                    _modeIcon.CurrentMode = ModeIcons.Mode.None;
                    break;

                default:
                    break;
            }

            // モード変更開始
            _isChanging = true;

            // 移動
            Camera.main.transform.DOMove(newPos, _changeTime)
                .OnComplete(() => { _isChanging = false; });

            // 回転
            Camera.main.transform.DOLocalRotate(newRot, _changeTime);
        }

        // 選択オブジェクト変更処理
        void ChangeSelectedObject()
        {
            // 移動中ならアニメーションをやめる
            Camera.main.transform.DOComplete();

            // 選択したオブジェクトのx座標を設定
            var newPos = new Vector3(_selectedObject.transform.position.x + _correctionXPos, Camera.main.transform.position.y, Camera.main.transform.position.z);

            // モード変更開始
            _isChanging = true;

            // 移動
            Camera.main.transform.DOMove(newPos, _changeTime)
                .OnComplete(() => { _isChanging = false; });
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

        // モード変更中かどうか
        public bool IsChanging
        {
            get { return _isChanging; }
        }

        // オブジェクト回転中かどうか
        public bool IsRotate
        {
            get { return _isRotate.IsRotate; }
        }

        // ゲームモードのカメラ座標
        public Vector3 GetGameModeCameraPos
        {
            get { return new Vector3(0f, 0f, _gameScreen.position.z + -_gameScreenDistance); }
        }

        // オブジェクトコントロールモードのカメラ座標
        //public Vector3 GetObjectControlModeCameraPos
        //{
        //    get { return new Vector3(0f, 0f, _objectScreen.position.z + -_objectScreenDistance); }
        //}
    }
}
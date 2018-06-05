using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GucchiCS
{
    public class StageManager : SingletonMonoBehaviour<StageManager>
    {
        // ステージ数
        [SerializeField]
        public static readonly int MAX_STAGE_NUM = 20;

        // ステージ番号
        [SerializeField]
        int _stageNo = 0;

        // ゲームプレイ状態
        bool _isPlay = false;

        // 操作モード（マウスならtrue、キーボードならfalse）
        bool _isStateMouse = false;

        // 起動設定
        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeMethodLoad()
        {
            Screen.SetResolution(1024, 600, false);
        }

        // Use this for initialization
        void Start()
        {
            // モード
            ModeChanger.MODE mode = ModeChanger.Instance.Mode;

            // 始めはマウスカーソルを隠す
            Cursor.visible = false;

            // ゲームモード
            this.LateUpdateAsObservable()
                .Where(_ => CheckState())
                .Where(_ => Input.GetKeyDown(KeyCode.Alpha1))
                .Subscribe(_ => mode = ModeChanger.MODE.GAME);

            // コントロールモード（オブジェクト選択なし）
            this.LateUpdateAsObservable()
                .Where(_ => CheckState())
                .Where(_ => Input.GetKeyDown(KeyCode.Alpha2))
                .Subscribe(_ => mode = ModeChanger.MODE.OBJECT_CONTROL);

            // モード変更時通知
            this.ObserveEveryValueChanged(newMode => mode)
                .Subscribe(newMode => ModeChanger.Instance.Mode = newMode);

            // 何らかのキーが押されたら
            this.UpdateAsObservable()
                .Where(_ => _isStateMouse)
                .Where(_ => Input.anyKeyDown && !Input.GetMouseButtonDown(0))
                .Subscribe(_ =>
                {
                    Cursor.visible = false;
                    _isStateMouse = false;
                });

            // マウスが動かされたらマウスステートにする
            this.UpdateAsObservable()
                .Where(_ => !_isStateMouse)
                .Where(_ => Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                .Subscribe(_ =>
                {
                    Cursor.visible = true;
                    _isStateMouse = true;
                });
        }

        // ゲーム操作可能状態チェック
        bool CheckState()
        {
            // クリア状態かどうか
            bool isClear = ModeChanger.Instance.Mode == ModeChanger.MODE.CLEAR;

            // モード変更中かどうか
            bool isChanging = ModeChanger.Instance.IsChanging;

            // オブジェクト回転中かどうか
            bool isRotate = ModeChanger.Instance.IsRotate;

            return IsPlay && !_isStateMouse && !isClear && !isChanging && !isRotate;
        }

        // ゲームプレイ状態かどうか
        public bool IsPlay
        {
            get { return _isPlay; }
            set { _isPlay = value; }
        }

        // マウス操作状態かどうか
        public bool IsStateMouse
        {
            get { return _isStateMouse; }
        }

        // 現在のステージ番号の取得
        public int StageNo
        {
            get { return _stageNo; }
        }

        // 死亡判定
        public void GameoverEnter()
        {
            _isPlay = false;
        }
    }
}
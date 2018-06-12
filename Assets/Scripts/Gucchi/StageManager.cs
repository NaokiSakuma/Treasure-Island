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

        // ステージオブジェクト
        [SerializeField]
        Transform _stageObjects = null;

        // 起動設定
        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeMethodLoad()
        {
            Screen.SetResolution(1024, 600, false);
            QualitySettings.SetQualityLevel(3);
        }

        // Use this for initialization
        void Start()
        {
            // モード
            ModeChanger.MODE mode = ModeChanger.Instance.Mode;

            // ステージ番号をリーダーに設定
            StageNoReader.Instance.ChangeStageNo = _stageNo;

            // 始めはマウスカーソルを隠す
            Cursor.visible = false;

            // モード切り替え
            this.LateUpdateAsObservable()
                .Where(_ => CheckState())
                .Where(_ => Input.GetKeyDown(KeyCode.Space))
                .Subscribe(_ =>
                {
                    // 現在のモード
                    mode = ModeChanger.Instance.Mode;

                    // ゲームモード　⇒　コントロールモード（オブジェクト選択なし）
                    if (mode == ModeChanger.MODE.GAME)
                    {
                        mode = ModeChanger.MODE.OBJECT_CONTROL;
                    }
                    else if (mode == ModeChanger.MODE.OBJECT_CONTROL || mode == ModeChanger.MODE.OBJECT_CONTROL_SELECTED)
                    {
                        mode = ModeChanger.MODE.GAME;
                    }
                });

            // モード変更時通知
            this.ObserveEveryValueChanged(newMode => mode)
                .Subscribe(newMode => ModeChanger.Instance.Mode = newMode);
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

            return IsPlay && !ControlState.Instance.IsStateMouse && !isClear && !isChanging && !isRotate;
        }

        // オブジェクトの影モード設定
        public void SetObjectShadowMode()
        {
            foreach (Transform obj in _stageObjects)
            {
                // ゲームモードならShadowOnlyにし、それ以外はOnにする
                if (ModeChanger.Instance.Mode == ModeChanger.MODE.GAME)
                    obj.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                else
                    obj.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            }
        }

        /* プロパティ */

        // ゲームプレイ状態かどうか
        public bool IsPlay
        {
            get { return _isPlay; }
            set { _isPlay = value; }
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
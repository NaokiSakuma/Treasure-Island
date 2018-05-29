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

        // Use this for initialization
        void Start()
        {
            // モード
            ModeChanger.MODE mode = ModeChanger.Instance.Mode;

            // ゲームモード
            this.LateUpdateAsObservable()
                .Where(_ => CheckState())
                .Where(_ => Input.GetKeyDown(KeyCode.Q))
                .Subscribe(_ => mode = ModeChanger.MODE.GAME);

            // コントロールモード（オブジェクト選択なし）
            this.LateUpdateAsObservable()
                .Where(_ => CheckState())
                .Where(_ => Input.GetKeyDown(KeyCode.W))
                .Subscribe(_ => mode = ModeChanger.MODE.OBJECT_CONTROL);

            // ライトモード
            this.LateUpdateAsObservable()
                .Where(_ => CheckState())
                .Where(_ => Input.GetKeyDown(KeyCode.E))
                .Where(_ => SceneManager.GetActiveScene().name != SingletonName.TITLE_SCENE)
                .Subscribe(_ => mode = ModeChanger.MODE.SPOTLIGHT_CONTROL);

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

            return IsPlay && !isClear && !isChanging && !isRotate;
        }

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
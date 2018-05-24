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
            // ゲーム状態のストリーム
            IObservable<long> state = Observable
                .EveryUpdate()
                .Where(_ => _isPlay)
                .Where(_ => ModeChanger.Instance.Mode != ModeChanger.MODE.CLEAR);

            // プレイ中の操作
            state.Subscribe(_ =>
            {
                // モード
                ModeChanger.MODE mode = ModeChanger.Instance.Mode;

                // ゲームモード
                this.LateUpdateAsObservable()
                    .Where(__ => Input.GetKeyDown(KeyCode.Q))
                    .Subscribe(__ => mode = ModeChanger.MODE.GAME);

                // コントロールモード（オブジェクト選択なし）
                this.LateUpdateAsObservable()
                    .Where(__ => Input.GetKeyDown(KeyCode.W))
                    .Subscribe(__ => mode = ModeChanger.MODE.OBJECT_CONTROL);

                // ライトモード
                this.LateUpdateAsObservable()
                    .Where(__ => Input.GetKeyDown(KeyCode.E))
                    .Where(__ => SceneManager.GetActiveScene().name != SingletonName.TITLE_SCENE)
                    .Subscribe(__ => mode = ModeChanger.MODE.SPOTLIGHT_CONTROL);

                // コントロールモード（オブジェクト選択時）
                this.LateUpdateAsObservable()
                    .Where(__ => Input.GetMouseButtonDown(0))
                    .Where(__ => (mode == ModeChanger.MODE.OBJECT_CONTROL || mode == ModeChanger.MODE.OBJECT_CONTROL_SELECTED))
                    .Subscribe(__ => mode = ModeChanger.MODE.OBJECT_CONTROL_SELECTED);

                // モード変更時通知
                this.ObserveEveryValueChanged(newMode => mode)
                    .Subscribe(newMode => ModeChanger.Instance.Mode = newMode);
            });
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
    }
}
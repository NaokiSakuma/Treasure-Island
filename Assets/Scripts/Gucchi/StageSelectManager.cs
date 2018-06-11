using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GucchiCS
{
    public class StageSelectManager : SingletonMonoBehaviour<StageSelectManager>
    {
        // ブロックリスト
        [SerializeField]
        List<SelectLight> _blockList = new List<SelectLight>();

        // 位置の間隔
        [SerializeField]
        float _posInterval = 20f;

        // ライト変更中かどうか
        bool _isChanging = false;

        // 扉
        List<Door> _doors = new List<Door>();

        // 仮選択中の扉（初期選択を指定）
        [SerializeField]
        Door _selectedDoor = null;

        // Use this for initialization
        void Start()
        {
            // ブロック数
            int numBlock = _blockList.Count;

            // 配置
            if (numBlock > 0)
            {
                for (int i = 0; i < numBlock; i++)
                {
                    _blockList[i].BlockID = i;

                    // 位置設定
                    float angle = 360f / numBlock * i;
                    foreach (Transform child in _blockList[i].transform)
                    {
                        child.position = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * _posInterval, Mathf.Sin(Mathf.Deg2Rad * angle) * _posInterval, -10f);
                        child.LookAt(child.root);
                        child.rotation = Quaternion.Euler(-angle, -angle, 0f);

                        _doors.Add(child.GetComponent<Door>());
                    }
                }
            }

            // 初期の仮選択を設定
            _selectedDoor.OnSelectEnter();

            // 仮選択
            this.FixedUpdateAsObservable()
                .Subscribe(_ =>
                {
                    // マウスステート
                    if (ControlState.Instance.IsStateMouse)
                    {
                        // マウスの位置からrayを飛ばす
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit = new RaycastHit();

                        if (Physics.Raycast(ray, out hit, LayerMask.NameToLayer("Door")))
                        {
                            Door door = hit.collider.GetComponent<Door>();

                            if (door != null && door != _selectedDoor)
                            {
                                _selectedDoor.OnSelectExit();
                                _selectedDoor = door;
                                _selectedDoor.OnSelectEnter();
                            }
                        }
                    }
                    // キーステート
                    else
                    {
                        //// 選択中の扉番号
                        //int doorID = _selectedDoor.DoorID;

                        //// ブロック内の扉数
                        //int doorNum = _blockList[0].transform.childCount;

                        //// 横列の扉数
                        //int doorColumnNum = _blockList[0].transform.childCount / 2;

                        //if (Input.GetKeyDown(KeyCode.A))
                        //{
                            
                        //}
                    }
                });

            // ホイール操作（手前）でライト変更
            this.UpdateAsObservable()
                .Where(_ => Input.GetAxis("Mouse ScrollWheel") < 0)
                .Where(_ => ControlState.Instance.IsStateMouse)
                .Where(_ => !_isChanging)
                .Subscribe(_ =>
                {
                    ChangeLightAction(numBlock);
                    return;
                });

            // ホイール操作（奥）でライト変更
            this.UpdateAsObservable()
                .Where(_ => Input.GetAxis("Mouse ScrollWheel") > 0)
                .Where(_ => ControlState.Instance.IsStateMouse)
                .Where(_ => !_isChanging)
                .Subscribe(_ =>
                {
                    ChangeLightAction(numBlock);
                    return;
                });

            // WSキーでライト変更
            this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
                .Where(_ => !ControlState.Instance.IsStateMouse)
                .Where(_ => !_isChanging)
                .Subscribe(_ =>
                {
                    ChangeLightAction(numBlock);
                    return;
                }); 

            // BGMを再生
            AudioManager.Instance.PlayBGM(AUDIO.BGM_STAGESELECT, AudioManager.BGM_FADE_SPEED_RATE_HIGH);
        }

        // ライト回転
        void ChangeLightAction(int numBlock)
        {
            _isChanging = true;

            // SEを鳴らす
            AudioManager.Instance.PlaySE(AUDIO.SE_OBJECTROTATE);

            // すべてのブロックを移動
            foreach (SelectLight block in _blockList)
            {
                block.ChangeLightAction(numBlock);
            }
        }

        // 回転終了通知を受け取る
        public void AnimationCompleteNotify()
        {
            _isChanging = false;
        }
    }
}
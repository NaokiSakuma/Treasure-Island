using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

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

        // 切り替えボタン
        [SerializeField]
        Button _changeButton = null;

        // 選択済みかどうか
        bool _stageSelected = false;

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

                        // 子のドアを取り出す
                        foreach (Transform doorObj in child.GetChild(1).transform)
                        {
                            Door door = doorObj.GetComponent<Door>();

                            // クリア済みならチェックをつける
                            door.SetClearState();

                            _doors.Add(door);
                        }
                    }
                }
            }

            // 前回遊んだステージの次のステージを仮選択しておく
            int beforeStageNo = StageNoReader._stageNo % _doors.Count;
            if (beforeStageNo == 0)
                beforeStageNo = 1;

            // 初期の仮選択を設定（クリアした後なら次のステージ、そうでないなら前回のステージまたは１ステージ）
            if (StageNoReader._isClear)
                _selectedDoor = _doors[beforeStageNo];
            else
                _selectedDoor = _doors[beforeStageNo - 1];
            _selectedDoor.OnSelectEnter();

            // 仮選択ステージによってブロック位置を変更
            int blockNo = beforeStageNo / (_doors.Count / _blockList.Count);
            if (blockNo > 0)
            {
                _isChanging = true;

                // すべてのブロックを移動
                foreach (SelectLight block in _blockList)
                {
                    block.SetBeforeLightPos(blockNo, _blockList.Count);
                }
            }

            // 仮選択（マウス）
            this.LateUpdateAsObservable()
                .Where(_ => ControlState.Instance.IsStateMouse)
                .Where(_ => !_stageSelected)
                .Subscribe(_ =>
                {
                    // マウスの位置からrayを飛ばす
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit = new RaycastHit();
                    LayerMask layermask = 1 << LayerMask.NameToLayer("Door");

                    if (Physics.Raycast(ray, out hit, layermask))
                    {
                        Door door = hit.collider.GetComponent<Door>();

                        // なぜかレイヤー範囲外のClearObjectを指すときがあるのでそのときは親のDoorを使う
                        if (door == null)
                        {
                            door = hit.collider.transform.parent.GetComponent<Door>();

                            _selectedDoor.OnSelectExit();
                            _selectedDoor = door;
                            _selectedDoor.OnSelectEnter();
                        }
                        else if (door != _selectedDoor)
                        {
                            _selectedDoor.OnSelectExit();
                            _selectedDoor = door;
                            _selectedDoor.OnSelectEnter();
                        }
                    }
                });

            // 仮選択（キー）
            this.LateUpdateAsObservable()
                .Where(_ => !ControlState.Instance.IsStateMouse)
                .Where(_ => Input.anyKeyDown)
                .Where(_ => !_stageSelected)
                .Subscribe(_ =>
                {
                    // 選択中の扉ID
                    int doorID = _selectedDoor.DoorID;

                    // ブロックの扉数
                    int doorNum = _doors.Count / _blockList.Count;

                    // 現在の行
                    int row = doorID / (doorNum / 2);

                    // キーチェック
                    bool check = false;

                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        doorID--;
                        if (doorID < 0)
                            doorID = doorNum * _blockList.Count - 1;

                        // 同じ行内でラップ
                        doorID = ((doorID % (doorNum / 2)) + (doorNum / 2) * row);

                        check = true;
                    }
                    else if (Input.GetKeyDown(KeyCode.D))
                    {
                        doorID++;
                        if (doorID >= doorNum * _blockList.Count)
                            doorID = 0;

                        // 同じ行内でラップ
                        doorID = ((doorID % (doorNum / 2)) + (doorNum / 2) * row);

                        check = true;
                    }

                    // 選択扉を変更
                    if (check)
                    {
                        _selectedDoor.OnSelectExit();
                        _selectedDoor = _doors[doorID];
                        _selectedDoor.OnSelectEnter();
                    }
                });

            // ホイール操作（手前）でライト変更
            this.LateUpdateAsObservable()
                .Where(_ => !_stageSelected)
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
                .Where(_ => !_stageSelected)
                .Where(_ => !_isChanging)
                .Subscribe(_ =>
                {
                    ChangeLightAction(numBlock);
                    return;
                });

            // WSキーでライト変更
            this.LateUpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
                .Where(_ => !ControlState.Instance.IsStateMouse)
                .Where(_ => !_stageSelected)
                .Where(_ => !_isChanging)
                .Subscribe(_ =>
                {
                    // 選択中の扉ID
                    int doorID = _selectedDoor.DoorID;

                    // ブロックの扉数
                    int doorNum = _doors.Count / _blockList.Count;

                    // 現在の行
                    int row = 0;

                    // 上方向
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        doorID -= doorNum / 2;
                        if (doorID < 0)
                            doorID = doorNum * _blockList.Count + doorID;

                        row = doorID / (doorNum / 2);

                        if (row % 2 == 1)
                            ChangeLightAction(numBlock);
                    }
                    // 下方向
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        doorID += doorNum / 2;
                        if (doorID >= doorNum * _blockList.Count)
                            doorID = doorID - doorNum * _blockList.Count;

                        row = doorID / (doorNum / 2);

                        if (row % 2 == 0)
                            ChangeLightAction(numBlock);
                    }

                    _selectedDoor.OnSelectExit();
                    _selectedDoor = _doors[doorID];
                    _selectedDoor.OnSelectEnter();

                    return;
                });

            // Spaceキーで扉決定
            this.LateUpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.Space))
                .Where(_ => !ControlState.Instance.IsStateMouse)
                .Where(_ => !_stageSelected)
                .Where(_ => !_isChanging)
                .Take(1)
                .Subscribe(_ =>
                {
                    // ボタンを消す
                    DisposeLightChangeButton();

                    _selectedDoor.OnClick();
                });

            // BGMを再生
            AudioManager.Instance.PlayBGM(AUDIO.BGM_STAGESELECT, AudioManager.BGM_FADE_SPEED_RATE_HIGH);
        }

        // ライト回転
        public void ChangeLightAction(int numBlock, bool buttonClick = false)
        {
            _isChanging = true;

            // SEを鳴らす
            AudioManager.Instance.PlaySE(AUDIO.SE_OBJECTROTATE);

            // すべてのブロックを移動
            foreach (SelectLight block in _blockList)
            {
                block.ChangeLightAction(numBlock, buttonClick);
            }

            // ボタンの移動
            _changeButton.GetComponent<ButtonOfStageSelect>().ChangeLightRotate(buttonClick);
        }

        // 回転終了通知を受け取る
        public void AnimationCompleteNotify()
        {
            _isChanging = false;
        }

        // ステージ選択通知
        public void StageSelectNotify()
        {
            _stageSelected = true;
        }

        // ライト変更ボタンの削除
        public void DisposeLightChangeButton()
        {
            if (_changeButton != null)
            {
                Destroy(_changeButton.gameObject);
                _changeButton = null;
            }
        }

        /* プロパティ */

        // 選択した扉
        public Door SelectedDoor
        {
            set { _selectedDoor = value; }
        }

        // 回転中かどうか
        public bool IsChanging
        {
            get { return _isChanging; }
        }

        // ブロック数
        public int BlockNum
        {
            get { return _blockList.Count; }
        }
    }
}
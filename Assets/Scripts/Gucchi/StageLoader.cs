using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace GucchiCS
{
    public class StageLoader : MonoBehaviour
    {
        // ステージリスト
        [SerializeField]
        List<Stage> _stageList = new List<Stage>();

        // 選択したステージID
        int _stageNo = -1;

        void Awake()
        {
            this.ObserveEveryValueChanged(_ => _stageNo)
                .Where(_ => _stageNo >= 0)
                .Where(_ => _stageList.Count > 0)
                .Subscribe(_ =>
                {
                    Instantiate(_stageList[_stageNo], Vector3.zero, Quaternion.identity);
                });
        }

        // ステージ番号を設定
        public int StageNo
        {
            get { return _stageNo; }
            set { _stageNo = value; }
        }

        // 現在のステージのプレイヤーを取得
        public Transform Player
        {
            get { return _stageList[_stageNo].Player.transform; }
        }
    }
}
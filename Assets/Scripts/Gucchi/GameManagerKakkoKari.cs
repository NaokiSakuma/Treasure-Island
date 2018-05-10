using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GucchiCS
{
    public class GameManagerKakkoKari : SingletonMonoBehaviour<GameManagerKakkoKari>
    {
        // 最大ステージ数
        public const int MAX_STAGE_NUM = 20;

        // ステージ番号
        [SerializeField]
        int _stageNo = 0;

        // たぶんへっちーのほうでこういう管理を作ってると思うので仮で
        bool _isPlay = false;

        public bool IsPlay
        {
            get { return _isPlay; }
            set { _isPlay = value; }
        }

        // ステージ番号の取得
        public int StageNo
        {
            get { return _stageNo; }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GucchiCS
{
    public class StageNoReader : SingletonMonoBehaviour<StageNoReader>
    {
        // ステージ番号
        public static int _stageNo;

        // クリアフラグ
        public static bool _isClear;

        // Use this for initialization
        void Start()
        {
            DontDestroyOnLoad(this);
            _isClear = false;
        }

        // クリア時
        public void Cleared()
        {
            _isClear = true;
        }

        /* プロパティ */

        // ステージ変更時
        public int ChangeStageNo
        {
            set { _stageNo = value; }
        }
    }
}
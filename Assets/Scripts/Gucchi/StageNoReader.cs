using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GucchiCS
{
    public class StageNoReader : SingletonMonoBehaviour<StageNoReader>
    {
        public static int _stageNo;

        // Use this for initialization
        void Start()
        {
            DontDestroyOnLoad(this);
        }

        // ステージ変更時
        public int ChangeStageNo
        {
            set { _stageNo = value; }
        }
    }
}
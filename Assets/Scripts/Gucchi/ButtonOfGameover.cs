using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GucchiCS
{
    public class ButtonOfGameover : MonoBehaviour
    {
        // シーン名
        [SerializeField]
        string _sceneName = "";

        // クリック時
        public void OnClickRetry()
        {
            // 現在のステージ番号を取得
            int stageNo = StageManager.Instance.StageNo;

            SceneManager.LoadScene(_sceneName + stageNo.ToString());
        }

        // クリック時
        public void OnClickStageSelect()
        {
            SceneManager.LoadScene(_sceneName);
        }
    }
}
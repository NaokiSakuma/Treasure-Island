using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Konji
{
    public class ToTitle : MonoBehaviour
    {
        public void OnClick()
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_POSE);
            SceneManager.LoadScene("TitleScene");
        }
    }
}
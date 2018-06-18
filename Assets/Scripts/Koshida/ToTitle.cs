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
            SceneManager.LoadScene("TitleScene");
        }
    }
}
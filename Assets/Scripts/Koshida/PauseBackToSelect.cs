using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseBackToSelect : SimplePauseItem
{

    [SerializeField]
    private string _text = "ステージセレクトへ";

    void Start()
    {
        GetComponentInChildren<TextMesh>().text = _text;
    }

    public override void OnClick()
    {
        SceneManager.LoadScene("StageSelect");
    }
}

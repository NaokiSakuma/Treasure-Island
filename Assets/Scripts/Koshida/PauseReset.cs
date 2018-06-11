using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseReset : SimplePauseItem
{

    [SerializeField]
    private string _text = "リセット";

    void Start()
    {
        GetComponentInChildren<TextMesh>().text = _text;
    }

    public override void OnClick()
    {
        Pausable.Instance.pausing = false;

        //ステージリセット
        GetComponent<GucchiCS.ResetManager>().ResetObjects();
    }
}

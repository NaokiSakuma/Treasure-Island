using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using System;

public class PauseReset : SimplePauseItem
{

    [SerializeField]
    private string _text = "リセット";
    //シーン遷移をするか
    private bool canReset = true;
    public bool CanReset
    {
        set { canReset = value; }
        get { return canReset; }
    }


    void Start()
    {
        GetComponentInChildren<TextMesh>().text = _text;
    }

    public override void OnClick()
    {
        if (canReset)
        {
            Pausable.Instance.pausing = false;
            //ステージリセット
            GetComponent<GucchiCS.ResetManager>().ResetObjects();
            // SEを鳴らす
            AudioManager.Instance.PlaySE(AUDIO.SE_BUTTON);
        }
    }
}

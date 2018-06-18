using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBackToGame : SimplePauseItem {

	[SerializeField]
	private string _text = "ゲームに戻る";
    //ゲームに戻るか
    private bool canPauseBackToGame = true;
    public bool CanPauseBackToGame
    {
        set { canPauseBackToGame = value; }
        get { return canPauseBackToGame; }
    }


    void Start () {
		GetComponentInChildren<TextMesh>().text = _text;
	}

	public override void OnClick(){
        if (canPauseBackToGame)
        {
            // SEを鳴らす
            AudioManager.Instance.PlaySE(AUDIO.SE_POSE);

            Pausable.Instance.pausing = false;
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AudioManager.Instance.PlayBGM(AUDIO.BGM_1);
    }

    // Update is called once per frame
    void Update () {
		if(Input.GetKeyDown(KeyCode.A))
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_A);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_B);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AudioManager.Instance.FadeOutBGM();
        }

    }
}

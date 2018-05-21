using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestModeChange : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        if (GucchiCS.GameManagerKakkoKari.Instance.IsPlay)
        {
            if (GucchiCS.ModeChanger.Instance.Mode != GucchiCS.ModeChanger.MODE.CLEAR)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    GucchiCS.ModeChanger.Instance.Mode = GucchiCS.ModeChanger.MODE.GAME;
                }
                if (Input.GetKeyDown(KeyCode.W))
                {
                    GucchiCS.ModeChanger.Instance.Mode = GucchiCS.ModeChanger.MODE.OBJECT_CONTROL;
                }
                // タイトルシーンではスポットライトを移動できないようにする　さくま
                if (Input.GetKeyDown(KeyCode.E) && SceneManager.GetActiveScene().name != "TitleScene")
                {
                    GucchiCS.ModeChanger.Instance.Mode = GucchiCS.ModeChanger.MODE.SPOTLIGHT_CONTROL;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    GucchiCS.ModeChanger.MODE mode = GucchiCS.ModeChanger.Instance.Mode;
                    if (mode == GucchiCS.ModeChanger.MODE.OBJECT_CONTROL || mode == GucchiCS.ModeChanger.MODE.OBJECT_CONTROL_SELECTED)
                    {
                        // とりあえずコメントアウト　さくま
                        // GucchiCS.ModeChanger.Instance.SelectedObject = hit.collider.gameObject;
                        GucchiCS.ModeChanger.Instance.Mode = GucchiCS.ModeChanger.MODE.OBJECT_CONTROL_SELECTED;
                    }
                }
            }
        }
    }
}
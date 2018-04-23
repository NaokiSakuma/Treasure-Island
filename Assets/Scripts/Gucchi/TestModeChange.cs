using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestModeChange : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q))
        {
            GetComponent<GucchiCS.ModeChanger>().Mode = GucchiCS.ModeChanger.MODE.GAME;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            GetComponent<GucchiCS.ModeChanger>().Mode = GucchiCS.ModeChanger.MODE.OBJECT_CONTROL;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            GetComponent<GucchiCS.ModeChanger>().Mode = GucchiCS.ModeChanger.MODE.SPOTLIGHT_CONTROL;
        }
    }
}

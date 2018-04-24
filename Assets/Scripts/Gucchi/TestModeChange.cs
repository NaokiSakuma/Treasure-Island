using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestModeChange : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
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
            if (Input.GetKeyDown(KeyCode.E))
            {
                GucchiCS.ModeChanger.Instance.Mode = GucchiCS.ModeChanger.MODE.SPOTLIGHT_CONTROL;
            }
            if (Input.GetMouseButtonDown(0))
            {
                GucchiCS.ModeChanger.MODE mode = GucchiCS.ModeChanger.Instance.Mode;
                if (mode == GucchiCS.ModeChanger.MODE.OBJECT_CONTROL || mode == GucchiCS.ModeChanger.MODE.OBJECT_CONTROL_SELECTED)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        GucchiCS.ModeChanger.Instance.SelectedObject = hit.collider.gameObject;
                        GucchiCS.ModeChanger.Instance.Mode = GucchiCS.ModeChanger.MODE.OBJECT_CONTROL_SELECTED;
                    }
                }
            }
        }
    }
}

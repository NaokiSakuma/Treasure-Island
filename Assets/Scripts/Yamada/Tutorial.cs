using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {
    private bool isViewChange = false;
    private bool isCtrlChange = false;

    [SerializeField]
    private GameObject objectViewTutorial;
    [SerializeField]
    private GameObject controlTutorial;
    [SerializeField]
    private GameObject clickTutorial;

    // Use this for initialization
    void Start () {
        isViewChange = false;
        isCtrlChange = false;
        objectViewTutorial.SetActive(false);
        controlTutorial.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        isCtrlChange = CheckObjectControlView();
        CtrlChange();
        if (isViewChange) return;
        Debug.Log("aaa");

        if (GucchiCS.StageManager.Instance.IsPlay)
        {
            objectViewTutorial.SetActive(true);
            isViewChange = true;
        }

    }

    bool CheckObjectControlView()
    {
        if (GucchiCS.ModeChanger.Instance.Mode == GucchiCS.ModeChanger.MODE.OBJECT_CONTROL_SELECTED)
        {
            return true;
        }

        return false;
    }

    void CtrlChange()
    {
        if (isCtrlChange)
        {
            controlTutorial.SetActive(true);
            clickTutorial.SetActive(false);
        }
        else
        {
            controlTutorial.SetActive(false);
            clickTutorial.SetActive(true);
        }
    }
}

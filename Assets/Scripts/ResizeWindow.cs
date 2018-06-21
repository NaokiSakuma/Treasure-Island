using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

public class ResizeWindow : SingletonMonoBehaviour<ResizeWindow> {

    int lastWidth = Screen.width;
    int lastHeight = Screen.height;

    float defaultAspect = 16.0f / 9.0f;
    bool isReseting = false;

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this.gameObject);

        this.LateUpdateAsObservable()
            .Subscribe(_ =>
            {
                float as100 = Camera.main.aspect * 100;
                float cameraAspect = Mathf.Floor(as100) / 100;

                as100 = defaultAspect * 100;
                defaultAspect = Mathf.Floor(as100) / 100;

                if (cameraAspect != defaultAspect && !isReseting)
                {
                    StartCoroutine(SetResolution());
                }
            });
    }

    IEnumerator SetResolution()
    {
        isReseting = true;

        if (Screen.width != lastWidth)
        {
            // user is resizing width
            int height = (int)(Screen.width / defaultAspect);

            Screen.SetResolution(Screen.width, height, false);

            lastWidth = Screen.width;
            lastHeight = height;
        }
        else if (Screen.height != lastHeight)
        {

            // user is resizing height
            int width = (int)(Screen.height * defaultAspect);

            Screen.SetResolution(width, Screen.height, false);

            lastWidth = width;
            lastHeight = Screen.height;
        }

        yield return new WaitForSeconds(1.0f);
        isReseting = false;
    }
}

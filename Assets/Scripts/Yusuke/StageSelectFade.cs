﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class StageSelectFade : MonoBehaviour {
    //ドアたちたち
    [SerializeField]
    private GameObject[] doorss = null;
	// Use this for initialization
	void Start () {
        //フェード終了後ステージオブジェクトを有効にする
        FadeManager.Instance.InPlay();
        Observable.Timer(TimeSpan.FromSeconds(FadeManager.Instance.FadeTime))
        .Subscribe(x =>
        {
            foreach(GameObject doors in doorss)
            foreach (Transform child in doors.transform)
            {
                child.GetComponent<GucchiCS.Door>().CanSceneTrance = true;
            }
        });


    }

	// Update is called once per frame
	void Update () {

    }
}

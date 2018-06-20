using System.Collections;
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
        GetComponent<CirecleFade>().SetFadeMode(CirecleFade.FadeMode.In);
        Observable.Timer(TimeSpan.FromSeconds(GetComponent<CirecleFade>().FadeTime))
        .Subscribe(x =>
        {
            foreach(GameObject doors in doorss)
            foreach (Transform child in doors.transform)
            {
                child.GetComponent<GucchiCS.Door>().CanSceneTrance = true;
            }
            Destroy(gameObject.transform.parent.gameObject);
            Destroy(this.gameObject);
        });
    }

	// Update is called once per frame
	void Update () {

	}
}

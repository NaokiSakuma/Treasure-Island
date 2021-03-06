﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LightSpot : MonoBehaviour {
    //移動領域
    [SerializeField]
    float moveDistance = 3.0f;
    //移動時間
    [SerializeField]
    float moveTime = 60f;

    //初期Z座標
    float startPosZ = 0.0f;
    //経過時間
    int time = 0;

    //開始されたか
    [SerializeField]
    bool isStart = false;

    //タイムを加算するか
    bool isAddTime = true;
    public bool IsStart
    {
        set { isStart = value; }
        get { return isStart; }
    }

    // Use this for initialization
    void Start () {
        startPosZ = transform.position.z;
    }
	
	// Update is called once per frame
	void Update () {
        if (!isStart)
            return;
        //Z座標をラープ
        ZPosLearp();
        //時間を更新
        TimeUpdate();
        //時間を加算するか
        ChangeIsTimeUp();
        //アニメーションを停止
        StopAnimation();
    }

    /// <summary>
    /// Z座標をラープにより移動させる
    /// </summary>
    void ZPosLearp()
    {
        Vector3 pos = transform.position;
        pos = new Vector3(pos.x, pos.y, Mathf.Lerp((startPosZ - moveDistance), (startPosZ - moveDistance / 2), (time / moveTime)));
        transform.position = pos;
    }

    /// <summary>
    /// 時間を更新
    /// </summary>
    void TimeUpdate()
    {
        if (isAddTime)
        {
            time++;
        }
        else
        {
            time--;
        }
    }

    /// <summary>
    /// 時間更新フラグを変更する
    /// </summary>
    void ChangeIsTimeUp()
    {
        if (!isAddTime && time <= 0)
        {
            isAddTime = true;
        }
        if (isAddTime && time >= moveTime)
        {
            isAddTime = false;
        }
    }

    /// <summary>
    /// アニメーションを停止させる
    /// </summary>
    void StopAnimation()
    {
        // ステージ選択画面なら使わないので無視
        if (SceneManager.GetActiveScene().name == "StageSelect")
            return;
        //クリアしたら停止
        if(GucchiCS.ModeChanger.Instance.Mode == GucchiCS.ModeChanger.MODE.CLEAR)
        {
            IsStart = false;
            Vector3 pos = transform.position;
            pos = new Vector3(pos.x, pos.y, startPosZ - moveDistance);
            transform.position = pos;
        }

    }
}

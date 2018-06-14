using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObjects : MonoBehaviour {

    //子供として登録されているステーオブジェクト
    private List<StageObject> stageObjects = new List<StageObject>();
    //前のステーオブジェクト
    private StageObject lastZMoveStageObject = null;

    //Z座標移動させる定数
    const float Z_Move_Distance = -0.1f;
         
	// Use this for initialization
	void Start () {
        foreach (Transform child in gameObject.transform)
        {
            stageObjects.Add(child.GetComponent<StageObject>());
        }
    }

    // Update is called once per frame
    void Update () {
        ZFaitingCancell();
    }

    void ZFaitingCancell()
    {
        Vector3 pos = Vector3.zero;

        //選択状態のステーオブジェクトを代入
        if (ZAddMinute(SearchSelectObj()))
            return;

        //仮選択状態のステーオブジェクトを代入
        if (ZAddMinute(SearchSelectObj()))
            return;

        //最後に
        ZAddMinute(lastZMoveStageObject);
    }


    /// <summary>
    /// 選択状態のステージオブジェクトを返す
    /// </summary>
    /// <returns>選択状態のステーオブジェクト</returns>
    StageObject SearchSelectObj()
    {
        foreach(StageObject stageObj in stageObjects)
        {
            if (stageObj.IsSelect)
            {
                lastZMoveStageObject = stageObj;
                return stageObj;
            }
        }
        return null;
    }


    /// <summary>
    /// 選択状態のステージオブジェクトを返す
    /// </summary>
    /// <returns>選択状態のステーオブジェクト</returns>
    StageObject SearchTemporaryObj()
    {
        foreach (StageObject stageObj in stageObjects)
        {
            if (stageObj.IsTemporary)
            {
                lastZMoveStageObject = stageObj;
                return stageObj;
            }
        }
        return null;
    }

    /// <summary>
    /// Z座標に微小な定数を加える
    /// </summary>
    /// <param name="zChangeObj">Z座標を加算するオブジェクト</param>
    bool ZAddMinute(StageObject zChangeObj)
    {
        if (!zChangeObj)
            return false;

        Vector3 pos  = zChangeObj.transform.position;
         
        pos.z = Z_Move_Distance;

        zChangeObj.transform.position = pos;

        return true;
    }



}

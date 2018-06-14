using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSpot : MonoBehaviour {

    [SerializeField]
    float moveDistance = 3f;
    [SerializeField]
    float moveTime = 60f;

    float startPosZ;
    int time = 0;
    bool isTimeUp;

    [SerializeField]
    bool isStart = false;
    public bool IsStart
    {
        set { isStart = value; }
        get { return isStart; }
    }

    // Use this for initialization
    void Start () {
        startPosZ = transform.position.z;
        isTimeUp = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (!isStart)
            return;

        Vector3 pos = transform.position;
        pos = new Vector3(pos.x, pos.y, Mathf.Lerp((startPosZ - moveDistance) , (startPosZ - moveDistance / 2) ,(time / moveTime)));
        transform.position = pos;

        TimeUpdate();
        IsTimeUp();
    }


    void TimeUpdate()
    {
        if (isTimeUp)
        {
            time++;
        }
        else
        {
            time--;
        }
    }


    void IsTimeUp()
    {
        if (!isTimeUp && time <= 0)
        {
            isTimeUp = true;
        }
        if (isTimeUp && time >= moveTime)
        {
            isTimeUp = false;
        }
    }
}

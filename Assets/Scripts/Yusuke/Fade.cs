using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField]
    int moveTime = 120;

    float startPosZ = 0.0f;
    int time = 0;
    bool isTimeUp = false;


    // Use this for initialization
    void Start () {
    }

    void Update()
    {
        gameObject.GetComponent<Text>().color = new Color(gameObject.GetComponent<Text>().color.r, gameObject.GetComponent<Text>().color.g, gameObject.GetComponent<Text>().color.b, (float)time / moveTime);
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

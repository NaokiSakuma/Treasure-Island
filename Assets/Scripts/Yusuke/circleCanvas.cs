using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circleCanvas : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 pos = GetComponent<RectTransform>().position;
        GetComponent<RectTransform>().position = new Vector3(pos.x, pos.y, pos.z + 50);
    }
}

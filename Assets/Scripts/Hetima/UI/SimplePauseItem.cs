using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePauseItem : MonoBehaviour, IPauseItem {

	// Use this for initialization
	void Start () {
		
	}

	public void OnClick(){
	}
	public void OnEnter(){
		transform.GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
	}
	public void OnExit(){
		transform.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	}
}

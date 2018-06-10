using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuButton : MonoBehaviour {

	void Start () {
		
	}

	public void ToggleMenu(){
		Pausable.Instance.pausing = !Pausable.Instance.pausing;
	}
}

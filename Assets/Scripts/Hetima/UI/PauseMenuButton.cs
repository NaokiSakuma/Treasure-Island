using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class PauseMenuButton : MonoBehaviour {

	void Start () {
		
	}

	public void ToggleMenu() {
        if(GucchiCS.StageManager.Instance.IsPlay && !GucchiCS.ModeChanger.Instance.IsChanging){
            Pausable.Instance.pausing = !Pausable.Instance.pausing;
        }
	}
}

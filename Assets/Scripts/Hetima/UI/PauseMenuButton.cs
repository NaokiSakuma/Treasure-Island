using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class PauseMenuButton : MonoBehaviour {

	void Start () {
		
	}

	public void ToggleMenu() {
        this.UpdateAsObservable()
            .Where(_ => GucchiCS.StageManager.Instance.IsPlay)
            .Where(_ => !GucchiCS.ModeChanger.Instance.IsChanging)
            .Subscribe(_ => Pausable.Instance.pausing = !Pausable.Instance.pausing);
	}
}

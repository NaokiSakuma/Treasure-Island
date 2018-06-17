using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PauseRotate : MonoBehaviour {

	// Use this for initialization
	void Start () {

        // ポーズしたときにオブジェクトを消す
        this.ObserveEveryValueChanged(x => Pausable.Instance.pausing)
            .Where(_ => (GucchiCS.ModeChanger.Instance.Mode == GucchiCS.ModeChanger.MODE.OBJECT_CONTROL) || (GucchiCS.ModeChanger.Instance.Mode == GucchiCS.ModeChanger.MODE.OBJECT_CONTROL_SELECTED))
            .Subscribe(x => RotateManager.Instance.Pause(!x));
	}

}

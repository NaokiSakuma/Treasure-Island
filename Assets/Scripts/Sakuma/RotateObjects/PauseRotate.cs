using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PauseRotate : MonoBehaviour {

	// Use this for initialization
	void Start () {

        // ポーズしたときにオブジェクトを消す
        this.UpdateAsObservable()
            .Where(_ => Pausable.Instance.pausing)
            .Subscribe(_ => RotateManager.Instance.HideObject());
	}

}

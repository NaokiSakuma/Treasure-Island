using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PausableController : MonoBehaviour {

	void Start () {
		// Escape押下でポーズ切り替え
		this.UpdateAsObservable()
            .Where(_ => GucchiCS.StageManager.Instance.IsPlay)
            .Where(_ => !GucchiCS.ModeChanger.Instance.IsChanging)
			.Where(_ => Input.GetKeyDown(KeyCode.Escape))
			.Subscribe(_ => {
                // SEを鳴らす
                AudioManager.Instance.PlaySE(AUDIO.SE_POSE);

                Pausable.Instance.pausing = !Pausable.Instance.pausing;
			});
	}
}

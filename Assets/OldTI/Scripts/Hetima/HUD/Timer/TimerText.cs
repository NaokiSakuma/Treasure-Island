using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class TimerText : MonoBehaviour {
	// Use this for initialization
	void Start () {
		var text = GetComponentInChildren<Text>();
		var timer = GetComponent<BaseTimer>();

		// テキスト表示の更新
		this.ObserveEveryValueChanged(x => timer.RemainingTime)
			.Subscribe(x => {
				text.text = x / 60 + ":" + (x % 60 >= 10 ? "" : "0") + x % 60;
			});
	}
}

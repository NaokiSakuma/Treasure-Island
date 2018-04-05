using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class TimerText : MonoBehaviour {

	[SerializeField]
	public BaseTimer _timer;

	// Use this for initialization
	void Start () {

		var text = GetComponentInChildren<Text>();

		// テキスト表示の更新
		_timer.RemainingTime
		      .Subscribe(x => {
				text.text = x / 60 + ":" + (x % 60 >= 10 ? "" : "0") + x % 60;
			});
	}
}

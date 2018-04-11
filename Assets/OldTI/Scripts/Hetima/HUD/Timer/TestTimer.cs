using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TestTimer : BaseTimer{
	
	// Use this for initialization
	void Start () {
		this.UpdateAsObservable()
		    .ThrottleFirst(System.TimeSpan.FromSeconds(1))
			.Subscribe(_ => {
				RemainingTime--;
			});
	}
}

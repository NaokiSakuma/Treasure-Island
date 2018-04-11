using UnityEngine;
using UniRx;

public class BaseTimer : MonoBehaviour {
	// 残り時間
	[SerializeField, Range(0, 60*5)]
	private int _remainingTime = 60;
	public int RemainingTime {
		get { return _remainingTime; }
		set {
			_remainingTime = value;
			if (_remainingTime < 0){
				_remainingTime = 0;
			}}
	}
}

using UnityEngine;
using UniRx;

public class BaseTimer : MonoBehaviour {
	// 残り時間
	protected IntReactiveProperty _remainingTime = new IntReactiveProperty(60);
	public IntReactiveProperty RemainingTime {
		get { return _remainingTime; }
	}
}

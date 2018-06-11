using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PauseMenuUI : MonoBehaviour {

	// ボタンを表示するキャンバス
	[SerializeField]
	private Canvas _canvas;

	// キャンバスに表示するボタン
	[SerializeField]
	private GameObject _pauseButton;

	// 監視対象のボタン
	[SerializeField]
	private GameObject _skipButton;

	void Start () {
		// 監視対象のボタンが削除されたら
		_skipButton.OnDestroyAsObservable()
			.Subscribe(_ => {
				// ボタンを生成
				var obj = Instantiate(_pauseButton, _canvas.transform);
                GameOverManager.Instance.AddBlinkObject(obj);
			});
	}

}

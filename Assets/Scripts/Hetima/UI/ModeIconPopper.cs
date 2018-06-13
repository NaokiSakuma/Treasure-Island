using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class ModeIconPopper : MonoBehaviour {

	// ボタンを表示するキャンバス
	private Canvas _canvas;

	// キャンバスに表示するボタン
	[SerializeField]
	private GameObject _button;

	// 監視対象のボタン
	private GameObject _skipButton;

	void Start () {
		var menu = GetComponent<PauseMenuUI>();
		_skipButton = menu.SkipButton;
		_canvas = menu.ScreenCanvas;

		// 監視対象のボタンが削除されたら
		_skipButton.OnDestroyAsObservable()
			.Subscribe(_ => {
				// ボタンを生成
				var obj = Instantiate(_button, _canvas.transform);
                GameOverManager.Instance.AddBlinkObject(obj);
			});
	}

}

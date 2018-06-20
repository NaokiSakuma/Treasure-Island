using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class PauseMenuUI : MonoBehaviour {

	// ボタンを表示するキャンバス
	[SerializeField]
	private Canvas _canvas;
	public Canvas ScreenCanvas{
		get { return _canvas; }
	}

	// キャンバスに表示するボタン
	[SerializeField]
	private GameObject _pauseButton;

    private bool isPlay = true;
	// 監視対象のボタン
	[SerializeField]
	private GameObject _skipButton;
	public GameObject SkipButton{
		get { return _skipButton; }
	}

	void Start () {

        // GameOverManagerのInstance
        var instance = GameOverManager.Instance;
        this.UpdateAsObservable()
            .Subscribe(_ => 
            {
                instance = GameOverManager.Instance;
            });

		// 監視対象のボタンが削除されたら
		_skipButton.OnDestroyAsObservable()
            .Where(_ => instance != null)
            .Where(_ => isPlay == true)
			.Subscribe(_ => {
				// ボタンを生成
				//Instantiate(_pauseButton, _canvas.transform);
                GameOverManager.Instance.AddBlinkObject(Instantiate(_pauseButton, _canvas.transform));
            });
	}

    private void OnApplicationQuit()
    {
        isPlay = false;
    }
}

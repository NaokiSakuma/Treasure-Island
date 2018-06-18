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

	// 監視対象のボタン
	[SerializeField]
	private GameObject _skipButton;
	public GameObject SkipButton{
		get { return _skipButton; }
	}

	void Start () {
		// 監視対象のボタンが削除されたら
		_skipButton.OnDestroyAsObservable()
			.Subscribe(_ => {
				// ボタンを生成
				var obj = Instantiate(_pauseButton, _canvas.transform);
                var instance = GameOverManager.Instance;
                if(instance) GameOverManager.Instance.AddBlinkObject(obj);
                //// 生成したオブジェクトをgameovermanagerに登録
                //try
                //{
                //    GameOverManager.Instance.AddBlinkObject(obj);
                //}
                //catch (NullReferenceException ex) { print("myLight was not set in the inspector"); }
            });
	}

}

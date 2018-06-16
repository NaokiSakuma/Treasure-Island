using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonExitGame : MonoBehaviour {

	private CanvasGroup _group;

	[SerializeField, Range(0.0f, 2.0f)]
	private float _fadeTime;

	void Start () {
		_group = GetComponent<CanvasGroup>();

		// はじめは完全に透明にしておく
		_group.alpha = 0.0f;
		// フェードイン
		_group.DOFade(1.0f, _fadeTime);

		// 生成時にポーズ
		Pausable.Instance.pausing = true;
	}
	
	public void ExitGame(){
		// ゲーム終了
		Debug.Log("ここにゲーム終了処理");
		// 念の為の記述、フェードアウト後ゲームに戻る
		_group.DOFade(0.0f, _fadeTime).OnComplete(() => Destroy(this.gameObject));
	}

	public void BackToGame(){
		// フェードアウト後ゲームに戻る
		_group.DOFade(0.0f, _fadeTime)
			.OnComplete(() => Destroy(this.gameObject));
	}

	void OnDestroy() {
		Pausable.Instance.pausing = false;	
	}
}

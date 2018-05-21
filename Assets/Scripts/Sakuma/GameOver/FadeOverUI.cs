using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;

public class FadeOverUI : MonoBehaviour {

    // 増加するアルファ値
    [SerializeField]
    private float _addAlpha = 0;
    // canvas group
    private CanvasGroup _canvasGroup = null;

	// Use this for initialization
	void Start () {
        // アタッチ
        _canvasGroup = GetComponent<CanvasGroup>();
        
        // アルファ値を増加させる
        this.UpdateAsObservable()
            .Where(_ => _canvasGroup.alpha < 1)
            .Subscribe(_  =>
            {
                _canvasGroup.alpha += _addAlpha;
            });
	}

}

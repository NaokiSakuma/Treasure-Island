using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

public class UIPopper : MonoBehaviour{

	private Vector3 _startScale;
	private Vector3 _endScale;
	private float _duration = 0.5f;

	private Tweener _tweener;

	void Start () {
		_startScale = transform.localScale;
		_endScale = _startScale * 1.3f;
		_tweener = transform.DOScale(_startScale, _duration);

		var image = GetComponent<Image>();

		// フォーカスが入ったら大きくする
		image.OnSelectAsObservable()
			.Subscribe(_ => {
				StartScaling(_endScale);
			});

		// マウスが乗ったら大きくする
		image.OnPointerEnterAsObservable()
			.Subscribe(_ => {
				StartScaling(_endScale);
				// フォーカスを入れる
				EventSystem.current.SetSelectedGameObject(gameObject);
			});

		// フォーカスが外れたら小さくする
		image.OnDeselectAsObservable()
			.Subscribe(_ => {
				StartScaling(_startScale);
			});

		// マウスが離れたら小さく
		image.OnPointerExitAsObservable()
			.Subscribe(_ => {
				StartScaling(_startScale);
				// フォーカスを外す
				EventSystem.current.SetSelectedGameObject(null);
			});
	}

	void StartScaling(Vector3 value){
		_tweener.Kill();
		_tweener = transform.DOScale(value, _duration);
	}
}

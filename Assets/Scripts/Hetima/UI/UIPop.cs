using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using UnityEngine.EventSystems;

public class UIPop : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler{

	private Vector3 _startScale;
	private Vector3 _endScale;
	private float _duration = 0.5f;

	private Tweener _tweener;

	void Start () {
		_startScale = transform.localScale;
		_endScale = _startScale * 1.3f;
		_tweener = transform.DOScale(_startScale, _duration);
	}

	public void OnPointerEnter(PointerEventData e){
		_tweener.Kill();
		_tweener = transform.DOScale(_endScale, _duration);
	}

	public void OnPointerExit(PointerEventData e){
		_tweener.Kill();
		_tweener = transform.DOScale(_startScale, _duration);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;

public class ButtonHover : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler{

	private Image _image;

	private Subject<PointerEventData> enterSubject = new Subject<PointerEventData>();
	public IObservable<PointerEventData> OnPointerEnterAsObservable {
		get { return enterSubject; }
	}

	private Subject<PointerEventData> exitSubject = new Subject<PointerEventData>();
	public IObservable<PointerEventData> OnPointerExitAsObservable {
		get { return exitSubject; }
	}

	// Use this for initialization
	void Start() {
		_image = GetComponent<Image>();
	}

	/// <summary>
	/// ポインターが触れた時を検知
	/// </summary>
	/// <param name="pointerEventData">Pointer event data.</param>
	public void OnPointerEnter(PointerEventData pointerEventData) {
		enterSubject.OnNext(pointerEventData);
	}


	/// <summary>
	/// ポインターが離れた時を検知
	/// </summary>
	/// <param name="pointerEventData">Pointer event data.</param>
	public void OnPointerExit(PointerEventData pointerEventData) {
		//this.transform.position += (Vector3)pointerEventData.delta;
		exitSubject.OnNext(pointerEventData);
	}
}

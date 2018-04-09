using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;

public class ButtonClicker : MonoBehaviour, IPointerClickHandler {

	private Subject<PointerEventData> clickSubject = new Subject<PointerEventData>();
	public IObservable<PointerEventData> OnPointerClickAsObservable {
		get { return clickSubject; }
	}

	/// <summary>
	/// クリックを検知
	/// </summary>
	/// <param name="pointerEventData">Pointer event data.</param>
	public void OnPointerClick(PointerEventData pointerEventData) {
		clickSubject.OnNext(pointerEventData);
	}
}

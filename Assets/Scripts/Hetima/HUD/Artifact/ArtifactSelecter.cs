using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ArtifactSelecter : MonoBehaviour {

	[SerializeField]
	ButtonHover[] _buttons;

	// Use this for initialization
	void Start () {
		_buttons = GetComponentsInChildren<ButtonHover>();

		foreach(var button in _buttons){
			button.OnPointerEnterAsObservable
			      .Subscribe(_ => {
						button.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.0f, 1.0f);
			});

			button.OnPointerExitAsObservable
				  .Subscribe(_ => {
					  button.GetComponent<Image>().color = new Color(0,0,0,0);
				  });
		}
	}
}

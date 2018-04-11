using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using UnityEngine.SceneManagement;

public class ClearScene : MonoBehaviour {

	private FadeImage _fadeImage;
	[SerializeField, Range(0.0f, 10.0f)]
	private float _fadeSpeed;

	public UnitCore _boss;

	// Use this for initialization
	void Start () {
		var button = GetComponentInChildren<Button>();
		_fadeImage = GetComponentInChildren<FadeImage>();

		this.ObserveEveryValueChanged(x => _boss.Health)
			.Where(x => x <= 0)
			.Subscribe(_ => {
				StartCoroutine(FadeIn());
			});

		button.onClick.AsObservable()
			  .First()
		      .Subscribe(_ =>{
				StartCoroutine(FadeIn());
		});
	}
	
	private IEnumerator FadeIn(){
		_fadeImage.Range -= _fadeSpeed;
		if (_fadeImage.Range >= 0.0f){
			yield return null;
			yield return StartCoroutine(FadeIn());
		}
		yield break;
	}

	private IEnumerator FadeOut(){
		_fadeImage.Range += _fadeSpeed;
		if (_fadeImage.Range <= 1.0f) {
			yield return null;
			yield return StartCoroutine(FadeOut());
		}
		yield break;
	}

	public void ChangeScene(string title){
		SceneManager.LoadScene(title);
	}
}

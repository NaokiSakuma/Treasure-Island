using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class ExitGame : MonoBehaviour {

	[SerializeField]
	private GameObject _exitUI;

	[SerializeField]
	private Canvas _canvas;

	void Start () {
		// プレイヤーが衝突したら
		this.OnCollisionEnterAsObservable()
			.Where(col => col.transform.GetComponent<Konji.PlayerControl>())
			.Subscribe(col => {
				// UIの生成
				Instantiate(_exitUI, _canvas.transform);
			});
	}
}

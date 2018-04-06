using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class RelicCollection : MonoBehaviour {

	[SerializeField]
	private Konji.RelicManager _manager;

	private RelicIcon[] _icons;

	// Use this for initialization
	void Start() {
		var rm = _manager;

		_icons = GetComponentsInChildren<RelicIcon>();

		// 背景を透明にする
		foreach(var icon in _icons){
			var back = icon.transform.parent.GetComponent<Image>();
			back.color -= new Color(0, 0, 0, 1);
		}

		// ボタンにホバーした時の処理
		foreach (var icon in _icons) {
			var hover = icon.GetComponentInParent<ButtonHover>();

			hover.OnPointerEnterAsObservable
				 .Subscribe(_ => {
					hover.GetComponent<Image>().color += new Color(0, 0, 0, 1);
				 });

			hover.OnPointerExitAsObservable
				 .Subscribe(_ => {
					hover.GetComponent<Image>().color -= new Color(0, 0, 0, 1);
				 });
		}

		// ひとまず全部更新
		this.ObserveEveryValueChanged(_ => rm._relicList)
			.Subscribe(x => {
				SetAllRelicInfo();
			});
	}

	void SetAllRelicInfo(){
		// TODO: レリックの最大数を取れるようにしてもらう
		for (int i = 0; i < _manager._relicList.Count; i++){
			if(_manager._relicList[i]._image){
				_icons[i].GetComponent<Image>().sprite = _manager._relicList[i]._image;
			}
		}
	}
}

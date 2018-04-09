using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Konji;

public class RelicCollection : MonoBehaviour {

	[SerializeField]
	private RelicIcon[] _icons;

	// Use this for initialization
	void Start() {
		_icons = GetComponentsInChildren<RelicIcon>();

		// 背景を透明にする
		foreach(var icon in _icons){
			var back = icon.transform.parent.GetComponent<Image>();
			back.color -= new Color(0, 0, 0, 1);
		}

		// ひとまず全部更新
		this.ObserveEveryValueChanged(_ => RelicManager.Instance.RelicList)
			.Subscribe(x => {
				UpdateAllRelicInfo();
			});
	}

	void UpdateAllRelicInfo(){
		for (int i = 0; i < RelicManager.Instance.RelicList.Count; i++){
			_icons[i].RelicInfo = RelicManager.Instance.RelicList[i];
		}
	}
}

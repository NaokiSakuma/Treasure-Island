using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class RelicIcon : ButtonClicker {

	//[SerializeField]
	private Konji.CO.RelicInfo _relicInfo;
	public Konji.CO.RelicInfo RelicInfo{
		get { return _relicInfo; }
		set { _relicInfo = value; }
	}

	// Use this for initialization
	void Start () {

		var hover = GetComponentInParent<ButtonHover>();

		// 情報が更新されたらspriteを更新する
		this.ObserveEveryValueChanged(x => _relicInfo)
		    .Where(x => x != null)
		    .Subscribe(info =>{
				this.GetComponent<Image>().sprite = info._image;
		});

		// === ボタン背景の上にマウスオーバーした際の処理 ===
		hover.OnPointerEnterAsObservable
			 .Subscribe(_ => {
				 hover.GetComponent<Image>().color += new Color(0, 0, 0, 1);
			 });

		hover.OnPointerExitAsObservable
			 .Subscribe(_ => {
				 hover.GetComponent<Image>().color -= new Color(0, 0, 0, 1);
			 });

		// 選択、非選択時の透明度の変更
		this.OnPointerClickAsObservable
			.Subscribe(_ => {
				_relicInfo._isSelect = !_relicInfo._isSelect;
				var c = GetComponent<Image>().color;
				GetComponent<Image>().color = new Color(c.r, c.g, c.b, _relicInfo._isSelect ? 0.5f : 1.0f);
			});
	}
}

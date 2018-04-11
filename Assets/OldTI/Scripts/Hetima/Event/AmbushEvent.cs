using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using GucchiCS;

public class AmbushEvent : MonoBehaviour {
	private Island _island;

	[SerializeField]
	private GameObject _enemy;

	// Use this for initialization
	void Start () {
		_island = GetComponent<Island>();
		var trigger = GetComponent<OccupationEventTrigger>();

		// 占領状態が変わったら
		this.ObserveEveryValueChanged(x => _island.IslandState)
		    .Where(x => x == Island.ISLAND_OCCUPATION.UNIT)
		    .First()
		    .Subscribe(_ => {
				trigger.SetOccupyIsland(0, true);
		});

		this.UpdateAsObservable()
		    .Where(x => trigger.NowEvent())
		    .First()
		    .Subscribe(_ =>{
				PopAmbush();
				trigger.End = true;
		});
	}

	void PopAmbush(){
		// 10体敵を湧かせる
		for (int i = 0; i < 10; i++){
			var enemy = Instantiate(_enemy);
			enemy.transform.position = transform.position + new Vector3(i, transform.position.y + 15, 0);
		}
	}
}

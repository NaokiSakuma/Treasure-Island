using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UnitCore : MonoBehaviour {

	// 体力
	private IntReactiveProperty _health = new IntReactiveProperty(1);
	public IntReactiveProperty Health {
		get { return _health; }
		set {
			_health = value;
			if (_health.Value <= 0) {
				_health.Value = 0;
			}
		}
	}

	// 攻撃力
	private int _attack = 1;
	public int Attack{
		get { return _attack; }
	}

	// 所属陣営
	// TODO: ひとまずint
	[SerializeField]
	private int _team = 0;
	public int Team{
		get { return _team; }
	}

	// Use this for initialization
	void Start () {
	}
}

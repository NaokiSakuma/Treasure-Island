using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicDebug : MonoBehaviour {

    public Konji.RelicManager _relicManager;

	// Use this for initialization
	void Awake () {
        _relicManager.AddRelic(Konji.CO.RelicType.WarDrum);
	}
}

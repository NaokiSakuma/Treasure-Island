using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shadowFlagObj : MonoBehaviour {

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject shadowTutorial;

    // Use this for initialization
    void Start () {
        shadowTutorial.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        if (shadowTutorial.activeSelf) return;

        if (col.gameObject == player)
        {
            shadowTutorial.SetActive(true);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject == player)
        {
            shadowTutorial.SetActive(false);
        }
    }

}

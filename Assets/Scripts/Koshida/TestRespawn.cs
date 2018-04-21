using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRespawn : MonoBehaviour {

    [SerializeField]
    private Transform _respawnPoint;

    void OnTriggerEnter2D(Collider2D col2D)
    {
        if(col2D.tag == "Player")
        {
            col2D.transform.position = _respawnPoint.position;
        }
    }

}

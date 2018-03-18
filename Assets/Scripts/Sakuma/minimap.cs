using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimap : MonoBehaviour {

    // ミニマップカメラ
    [SerializeField]
    private Camera minimapCamera = null;


	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        // あとで修正（継承かインターフェイス）
        var mousePos = Input.mousePosition;
        const int mouseZpos = 10;
        mousePos.z = mouseZpos;
        var mouseWorldPos = minimapCamera.ScreenToWorldPoint(mousePos);
        var mouseviewPos = minimapCamera.WorldToViewportPoint(mouseWorldPos);

        MoveMainCamera(mouseviewPos);
    }

    private void MoveMainCamera(Vector3 viewPos)
    {
        // ここもやばい
        if (viewPos.x < 0 || viewPos.x > 1.0f || viewPos.y < 0 || viewPos.y > 1)
            return;

        if(Input.GetMouseButton(0))
        {
            Camera.main.transform.position = minimapCamera.ViewportToWorldPoint(viewPos);
        }
    }
}

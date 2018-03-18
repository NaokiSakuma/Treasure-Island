using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMap : MonoBehaviour {

    // カメラの移動速度
    [SerializeField]
    private float moveSpeed = 0;

    // カメラの移動制限
    [SerializeField]
    private float moveLimit = 0;

    // カメラのスクロール
    [SerializeField]
    private Vector2 scrollLimit = Vector2.zero;

    // カメラのスクロール速度
    [SerializeField]
    private float scrollSpeed = 0;	
	// Update is called once per frame
	void Update () {
        // マウスのビューポート座標
        var mousePos = Input.mousePosition;
        const int mouseZpos = 10;
        mousePos.z = mouseZpos;
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        var mouseviewPos = Camera.main.WorldToViewportPoint(mouseWorldPos);
        print("左下のワールド座標" + Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 10)));
        print("右上のワールド座標" + Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 10)));

        // 拡縮
        float scroll = -Input.GetAxis("Mouse ScrollWheel");
        transform.position += new Vector3(0, scroll * scrollSpeed, 0);

        // ここやばいからあとで修正 カメラの移動
        if (mouseviewPos.y >= 0.9f)
        {
            transform.position += (new Vector3(0, 0, moveSpeed * Time.deltaTime));
        }
        if (mouseviewPos.y <= 0.1f)
        {
            transform.position += (new Vector3(0, 0, -moveSpeed * Time.deltaTime));
        }
        if (mouseviewPos.x >= 0.9f)
        {
            transform.position += (new Vector3(moveSpeed * Time.deltaTime, 0, 0));
        }
        if (mouseviewPos.x <= 0.1f)
        {
            transform.position += (new Vector3(-moveSpeed * Time.deltaTime, 0, 0));
        }
        // ここまで



        // カメラの移動制限
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x,-moveLimit, moveLimit),
            Mathf.Clamp(transform.position.y,scrollLimit.x,scrollLimit.y), 
            Mathf.Clamp(transform.position.z, -moveLimit, moveLimit));
    }
}

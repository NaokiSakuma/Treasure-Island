using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMap : MonoBehaviour {

    // カメラの移動速度
    [SerializeField]
    private float moveSpeed = 0;

    // カメラの移動制限
    [SerializeField]
    private Vector2 moveLimit = Vector2.zero;
    public Vector2 MoveLimit
    {
        get { return moveLimit; }
    }

    // カメラのスクロール
    [SerializeField]
    private Vector2 scrollLimit = Vector2.zero;

    // カメラのスクロール速度
    [SerializeField]
    private float scrollSpeed = 0;

	// カメラの移動方向
	private Vector3 moveDirection;
	public Vector3 MoveDirection{
		get { return moveDirection; }
	}

	// Update is called once per frame
	void Update () {
        // マウスのビューポート座標
        var mousePos = Input.mousePosition;
        float mouseZpos = transform.position.y;
        mousePos.z = mouseZpos;
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        var mouseviewPos = Camera.main.WorldToViewportPoint(mouseWorldPos);
        // 拡縮
        float scroll = -Input.GetAxis("Mouse ScrollWheel");
        transform.position += new Vector3(0, scroll * scrollSpeed, 0);

        // test用
        if (!Input.GetMouseButton(0))
        {
			Vector3 direction = Vector3.zero;

            // ここやばいからあとで修正 カメラの移動
            if (mouseviewPos.y >= 0.9f)
            {
				direction += new Vector3(0, 0, moveSpeed * Time.deltaTime);
                //transform.position += (new Vector3(0, 0, moveSpeed * Time.deltaTime));
            }
            if (mouseviewPos.y <= 0.1f)
            {
				direction += new Vector3(0, 0, -moveSpeed * Time.deltaTime);
                //transform.position += (new Vector3(0, 0, -moveSpeed * Time.deltaTime));
            }
            if (mouseviewPos.x >= 0.9f)
            {
				direction += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
                //transform.position += (new Vector3(moveSpeed * Time.deltaTime, 0, 0));
            }
            if (mouseviewPos.x <= 0.1f)
            {
				direction += new Vector3(-moveSpeed * Time.deltaTime, 0, 0);
                //transform.position += (new Vector3(-moveSpeed * Time.deltaTime, 0, 0));
            }

			transform.position += direction;
			moveDirection = direction;
        }
        // ここまで



        // カメラの移動制限
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x,-moveLimit.x, moveLimit.x),
            Mathf.Clamp(transform.position.y,scrollLimit.x,scrollLimit.y), 
            Mathf.Clamp(transform.position.z, -moveLimit.y, moveLimit.y));
    }
}

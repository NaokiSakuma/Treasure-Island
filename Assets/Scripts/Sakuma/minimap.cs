using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimap : MonoBehaviour {

    // ミニマップカメラ
    [SerializeField]
    private Camera minimapCamera = null;

    // メインカメラ
    [SerializeField]
    private MoveMap moveMap = null;

    // メインカメラのビューポート座標のZ
    float cameraViewPosZ = 0f;

    // メインカメラの今のY座標
    float cameraNowY = 0f;
    // メインカメラの1フレーム前のY座標
    float cameraOldY = 0f;

    // Use this for initialization
    void Start () {
        transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, transform.position.z);
        cameraOldY = cameraNowY = Camera.main.transform.position.y;
        cameraViewPosZ = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.transform.position.y)).z;
    }

    // Update is called once per frame
    void Update () {
        // あとで修正（継承かインターフェイス）
        var mousePos = Input.mousePosition;
        mousePos.z = Camera.main.transform.position.y;
        var mouseWorldPos = minimapCamera.ScreenToWorldPoint(mousePos);
        var mouseviewPos = minimapCamera.WorldToViewportPoint(mouseWorldPos);

        cameraNowY = Camera.main.transform.position.y;
        // カメラのY座標が変わったら
        if (cameraNowY != cameraOldY)
        {
            // ビューポート座標を取得
            cameraViewPosZ = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.transform.position.y)).z;
            cameraViewPosZ -= Camera.main.transform.position.z;
        }
        // メインカメラの移動
        MoveMainCamera(mouseviewPos);
        cameraOldY = cameraNowY;
    }

    /// <summary>
    /// メインカメラの移動
    /// </summary>
    /// <param name="viewPos">ビューポート座標</param>
    private void MoveMainCamera(Vector3 viewPos)
    {
        // ここもやばい
        if (viewPos.x < 0 || viewPos.x > 1.0f || viewPos.y < 0 || viewPos.y > 1.0f)
            return;
        // マウスの左クリック
        if (Input.GetMouseButton(0))
        {
            // カメラの移動
            viewPos.z = Camera.main.transform.position.y;
            Camera.main.transform.position = new Vector3(minimapCamera.ViewportToWorldPoint(viewPos).x,
                Camera.main.transform.position.y,
                minimapCamera.ViewportToWorldPoint(viewPos).z - cameraViewPosZ);

            // カメラの移動制限
            Camera.main.transform.position = new Vector3(
                Mathf.Clamp(Camera.main.transform.position.x, -moveMap.MoveLimit.x, moveMap.MoveLimit.x),
                Camera.main.transform.position.y,
                Mathf.Clamp(Camera.main.transform.position.z, -moveMap.MoveLimit.y, moveMap.MoveLimit.y));
        }
    }
}

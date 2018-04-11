using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapLine : MonoBehaviour {
    // ライン
    [SerializeField]
    private LineRenderer lineRenderer;
    // ラインの数
    private const int lineNum = 4;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var worldDownLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.transform.position.y));
        var worldDownRight = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.transform.position.y));
        var worldTopLeft = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Camera.main.transform.position.y));
        var worldTopRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.transform.position.y));

        //print("左下のワールド座標" + camera.ViewportToWorldPoint(new Vector3(0, 0, 0)));
        //print("右上のワールド座標" + camera.ViewportToWorldPoint(new Vector3(1, 1, 0)));

        //var worldDownLeft = new Vector3(0, 5, 0);
        //var worldDownRight = new Vector3(0, 5, 10);
        //var worldTopLeft = new Vector3(10, 5, 0);
        //var worldTopRight = new Vector3(10, 5, 10);

        Vector3[] points = new Vector3[lineNum];
        points[0] = worldDownLeft;
        points[1] = worldDownRight;
        points[2] = worldTopRight;
        points[3] = worldTopLeft;
        for(int i=0; i<lineNum; i++)
        {
            points[i].y = points[1].y;
        }
        lineRenderer.SetPositions(points);
    }
}

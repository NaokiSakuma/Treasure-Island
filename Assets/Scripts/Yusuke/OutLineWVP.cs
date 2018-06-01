using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutLineWVP : MonoBehaviour {

    [SerializeField]
    private const float scale = 0.8f; 

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        MeshNormalAverage(GetComponent<MeshFilter>().mesh);
    }


    //カメラからオブジェクトが可視状態の場合、各カメラごとに一回呼び出される。
    public void SetWVP()
    {
        ////親のマテリアル
        Material material = GetComponent<MeshRenderer>().material;
        if (material == null)
            return;

        Camera renderCamera = Camera.main;
        Matrix4x4 WVP,view,proj,mworld,mtran, mscale, mrot;
        //移動行列
        mtran = Matrix4x4.Translate(new Vector3(transform.position.x,- transform.position.y, transform.position.z));

        //スケール行列
        mscale = Matrix4x4.Scale(transform.localScale);
        //回転行列
        mrot = Matrix4x4.Rotate(transform.rotation);
        //ビュー行列
        view = renderCamera.worldToCameraMatrix;
        //プロジェクション行列
        proj = renderCamera.cameraType == CameraType.SceneView ?
          GL.GetGPUProjectionMatrix(renderCamera.projectionMatrix, true) : renderCamera.projectionMatrix;
        //ワールド行列
        mworld = mtran * mrot * mscale;


        //ワールド・ビュー・プロジェクション行列
        WVP = proj *  view * mworld;


        //マテリアにワールド・ビュー・プロジェクション行列を渡す
        material.SetMatrix("WVP", WVP);
        Debug.Log("mat");



    }

    public static void MeshNormalAverage(Mesh mesh)
    {
        Dictionary<Vector3, List<int>> map = new Dictionary<Vector3, List<int>>();

        #region build the map of vertex and triangles' relation
        for (int v = 0; v < mesh.vertexCount; ++v)
        {
            if (!map.ContainsKey(mesh.vertices[v]))
            {
                map.Add(mesh.vertices[v], new List<int>());
            }

            map[mesh.vertices[v]].Add(v);
        }
        #endregion

        Vector3[] normals = mesh.normals;
        Vector3 normal;

        #region the same vertex use the same normal(average)
        foreach (var p in map)
        {
            normal = Vector3.zero;

            foreach (var n in p.Value)
            {
                normal += mesh.normals[n];
            }

            normal /= p.Value.Count;

            foreach (var n in p.Value)
            {
                normals[n] = normal;
            }
        }
        #endregion

        mesh.normals = normals;
    }
}






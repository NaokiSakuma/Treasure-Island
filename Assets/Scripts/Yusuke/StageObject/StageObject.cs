using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObject : MonoBehaviour {

    //マテリアル
    private Material material = null;

    //選択されているか
    private bool isSelect;
    public bool IsSelect
    {
        set { isSelect = value; }
        get { return isSelect; }
    }

    //仮選択されているか
    private bool isTemporary;
    public bool IsTemporary
    {
        set { isTemporary = value; }
        get { return isTemporary; }
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        MeshNormalAverage(GetComponent<MeshFilter>().mesh);
        material = GetComponent<MeshRenderer>().material;

        IfNeededEffect();
        ChangeColor();

    }

    /// <summary>
    /// 選択状態または仮選択状態の場合エフェクトをエフェクトをONにする
    /// </summary>
    private void IfNeededEffect()
    {
        if (isEffectOn())
        {
            material.SetFloat("_IsSelectEffect", 0.0f);
        }
        else
        {
            material.SetFloat("_IsSelectEffect", 1.0f);
        }
    }
    /// <summary>
    /// エフェクトを付けるか取得
    /// </summary>
    /// <returns>エフェクトを付けるか</returns>
    private bool isEffectOn()
    {
        return isSelect || isTemporary;
    }

    /// <summary>
    /// 色を変更する
    /// </summary>
    private void ChangeColor()
    {
        if(isSelect)
           material.SetColor("_SelectEffectColor", Color.yellow);
        else if(IsTemporary)
            material.SetColor("_SelectEffectColor", Color.blue);
    }

    /// <summary>
    /// 同じ座標の頂点の法線を平均値に揃える
    /// </summary>
    /// <param name="mesh"></param>
    private void MeshNormalAverage(Mesh mesh)
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






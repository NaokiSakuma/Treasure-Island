/*
 * @Date    18/04/06
 * @Create  Yuta Higuchi
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GucchiCS
{
    public class OccupationFlag : MonoBehaviour
    {
        // マテリアル
        public List<Material> _flagMaterial;

        // 占領状況に応じてマテリアルを変更
        public void ChangeMaterial(Island.ISLAND_OCCUPATION occupation)
        {
            transform.GetComponentInChildren<Cloth>().GetComponent<Renderer>().material = _flagMaterial[(int)occupation];
        }
    }   
}
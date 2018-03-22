using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Konji
{
    public class LandingEnemy : MonoBehaviour
    {
        public Transform _nearObj;

        //void Awake()
        //{
        //    _nearObj = SerchTag(gameObject, "LandingPoint");
        //}

        // Use this for initialization
        void Start()
        {
            if (_nearObj)
            {
                gameObject.transform.DOJump(_nearObj.position, 5, 1, 0.8f);
            }
        }

        GameObject SerchTag(GameObject nowObj, string tagName)
        {
            float tmpDis = 0;
            float nearDis = 0;
            GameObject targetObj = null;

            //タグ指定されたオブジェクトを配列で取得する
            foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagName))
            {
                //自身と取得したオブジェクトの距離を取得
                tmpDis = Vector3.Distance(obs.transform.position, nowObj.transform.position);

                if (nearDis == 0 || nearDis > tmpDis)
                {
                    nearDis = tmpDis;
                    targetObj = obs;
                }
            }
            return targetObj;
        }
    }
}
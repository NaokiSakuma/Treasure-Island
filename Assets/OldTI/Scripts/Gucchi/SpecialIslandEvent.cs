/*
 * @Date    18/04/08
 * @Create  Yuta Higuchi
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GucchiCS
{
    [RequireComponent(typeof(OccupationEventTrigger), typeof(BossEventTrigger))]
    public class SpecialIslandEvent : MonoBehaviour
    {
        // 占領イベント
        OccupationEventTrigger _occupationEvent = null;

        // ボスイベント
        BossEventTrigger _bossEvent = null;

        // 島の出現
        public GameObject _ground;

        // 出現位置
        public Vector3 _pos;

        // Use this for initialization
        void Start()
        {
            // 占領イベントの準備
            _occupationEvent = this.gameObject.GetComponent<OccupationEventTrigger>();

            // ボスイベントの準備
            _bossEvent = this.gameObject.GetComponent<BossEventTrigger>();
        }

        // Update is called once per frame
        void Update()
        {
            // イベント開始
            if (_occupationEvent.StartEvent() && !_bossEvent.StartEvent())
            {
                // 島を出現させる
                if (_ground.GetComponent<Island>() != null)
                {
                    Fishes fishes = Instantiate(_ground.GetComponent<Fishes>(), _pos, Quaternion.identity);
                    fishes.transform.SetParent(GameObject.Find("Field").transform, false);
                }
            }
        }
    }
}
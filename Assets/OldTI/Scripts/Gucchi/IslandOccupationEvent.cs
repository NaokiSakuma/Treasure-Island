/*
 * @Date    18/04/08
 * @Create  Yuta Higuchi
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GucchiCS
{
    [RequireComponent(typeof(OccupationEventTrigger))]
    public class IslandOccupationEvent : MonoBehaviour
    {
        // 占領イベント
        OccupationEventTrigger _occupationEvent = null;

        // 島の出現
        public GameObject _ground;

        // 出現位置
        public Vector3 _pos;

        // 初回のみ発生させるかどうか
        [SerializeField]
        bool _ontimeEvent = false;

        // すでにイベントを行ったかどうか（初回のみ発生させる場合のみ使用）
        bool _alreadyEvent = false;

        // Use this for initialization
        void Start()
        {
            // タイマーイベントの準備
            _occupationEvent = this.gameObject.GetComponent<OccupationEventTrigger>();
        }

        // Update is called once per frame
        void Update()
        {
            // イベント開始
            if (_occupationEvent.NowEvent())
            {
                // 島を出現させる
                if (_ground.GetComponent<Fishes>() != null)
                {
                    Fishes fishes = Instantiate(_ground.GetComponent<Fishes>(), _pos, Quaternion.identity);
                    fishes.transform.SetParent(GameObject.Find("Field").transform, false);
                }

                // イベント終了
                _occupationEvent.End = true;
            }
        }

        // 占領されたとき
        public void OccupationNotify()
        {
            if (!_alreadyEvent)
                _occupationEvent.SetOccupyIsland(0, true);

            if (_ontimeEvent)
                _alreadyEvent = true;
        }
    }
}
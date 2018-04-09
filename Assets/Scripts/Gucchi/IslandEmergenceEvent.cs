/*
 * @Date    18/04/08
 * @Create  Yuta Higuchi
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GucchiCS
{
    [RequireComponent(typeof(EmergenceEventTrigger))]
    public class IslandEmergenceEvent : MonoBehaviour
    {
        // タイマーイベント
        EmergenceEventTrigger _emergenceEvent = null;

        // 島の出現
        public GameObject _ground;

        // 出現位置
        public Vector3 _pos;

        // Use this for initialization
        void Start()
        {
            // タイマーイベントの準備
            _emergenceEvent = this.gameObject.GetComponent<EmergenceEventTrigger>();
        }

        // Update is called once per frame
        void Update()
        {
            // イベント開始
            if (_emergenceEvent.StartEvent())
            {
                // 島を出現させる
                if (_ground.GetComponent<Fishes>() != null)
                {
                    Fishes fishes = Instantiate(_ground.GetComponent<Fishes>(), _pos, Quaternion.identity);
                    fishes.transform.SetParent(GameObject.Find("Field").transform, false);
                }

                // イベント終了
                _emergenceEvent.End = true;
            }
        }

        // 出現したとき
        public void EmergenceNotify()
        {
            _emergenceEvent.Emergence = true;
        }
    }
}
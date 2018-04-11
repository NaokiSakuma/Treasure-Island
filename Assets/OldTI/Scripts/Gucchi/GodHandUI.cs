/*
 * @Date    18/04/09
 * @Create  Yuta Higuchi
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Konji;

namespace GucchiCS
{
    public class GodHandUI : MonoBehaviour
    {
        // 神の手
        public GameObject _godHand;

        // 遺物管理
        public RelicManager _relicManager;

        Relic _relic = null;

        // ID
        [SerializeField]
        int _id = 0;

		// 所持している遺物を選択した場合
		public void CatchHavingRelic()
        {
            if (transform.GetComponent<RelicIcon>().RelicInfo != null)
            {
                // 生成
                Relic relic = Instantiate(RelicManager.Instance._relicPrefab[_id]);

                _godHand.GetComponent<GodHand>().CatchHavingRelic(relic);
                _relic = relic;

                // 解放
                _relicManager.RemoveRelic(transform.GetComponent<RelicIcon>().RelicInfo);
                transform.GetComponent<RelicIcon>().RelicInfo = null;
                //Destroy(transform.GetComponent<RelicIcon>().RelicInfo._image);
            }
        }
    }
}
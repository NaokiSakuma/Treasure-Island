/*
 * @Date    18/04/09
 * @Create  Yuta Higuchi
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GucchiCS
{
    public class RelicFoundation : MonoBehaviour
    {
        // 設置されてる遺物
        Konji.Relic _relic;

        // 最初から設置したい遺物があればRelicSetterを生成した段階でこれを呼ぶ
        public void SetFirstRelic(Konji.Relic relic)
        {
            _relic = relic;
        }

        // 現在設置されている遺物
        public Konji.Relic LandingRelic
        {
            get { return _relic; }
            set { _relic = value; }
        }
    }   
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Konji
{
    //シングルトンにします
    public class RelicManager : MonoBehaviour
    {
        //Relicリストの最大数
        private const int MAX_RELIC_NUM = 6;

        //レリック管理リスト
        public List<CO.RelicInfo> _relicList = new List<CO.RelicInfo>(MAX_RELIC_NUM);

        //index番目のRelicを削除
        public void RemoveRelic(int index)
        {
            _relicList.RemoveAt(index);
        }

        //最後にRelic追加
        public void AddRelic(CO.RelicType type)
        {
            //追加後の数
            int relicNum = _relicList.Count + 1;

            //6個以上だったら先頭を削除
            if(relicNum > MAX_RELIC_NUM)
            {
                _relicList.RemoveAt(0);
            }

            CO.RelicInfo info = new CO.RelicInfo(type);

            //最後に追加
            _relicList.Add(info);
        }

        //Relicを選択状態にする
        public void SelectRelic(int index)
        {
            //範囲外を選ばないように
            if(index >= CO.RELIC_LIST.Count)
            {
                return;
            }

            //選択状態に
            _relicList[index]._isSelect = true;

            //選択状態の画像に変更
            _relicList[index]._image = CO.RELIC_IMAGE_LIST[CO.RELIC_LIST.Count];
        }

        //Relicを非選択状態にする
        public void CancelRelic(int index)
        {
            //範囲外を選ばないように
            if (index >= CO.RELIC_LIST.Count)
            {
                return;
            }

            //非選択状態に
            _relicList[index]._isSelect = false;

            //遺物の画像に変更
            _relicList[index]._image = CO.RELIC_IMAGE_LIST[(int)_relicList[index]._type];
        }
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Konji
{
    //シングルトンにします
    public class RelicManager : SingletonMonoBehaviour<RelicManager>
    {
        //Relicリストの最大数
        public const int MAX_RELIC_NUM = 6;
       
        // 遺物プレハブ
        public List<Relic> _relicPrefab = new List<Relic>();

        //レリック管理リスト
        private List<CO.RelicInfo> _relicList = new List<CO.RelicInfo>();
        public List<CO.RelicInfo> RelicList
        {
            get { return _relicList; }
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

        //RelicInffoの取得
        public CO.RelicInfo GetRelicInfo(int index)
        {
            //リストが0個、indexが0未満,indexがリスト個数以上
            if (_relicList.Count == 0 || index < 0 || index >= _relicList.Count)
            {
                return null;
            }

            return _relicList[index];
        }

        //index番目のRelicを削除
        //public void RemoveRelic(int index)
        //{
        //    //リストが0個、indexが0未満,indexがリスト個数以上
        //    if (_relicList.Count == 0 || index < 0 || index >= _relicList.Count)
        //    {
        //        return;
        //    }

        //    _relicList.RemoveAt(index);
        //}
        public void RemoveRelic(CO.RelicInfo info)
        {
            _relicList.Remove(info);
        }

        //Relicを選択状態にする
        public void SelectRelic(int index)
        {
            //リストが0個、indexが0未満,indexがリスト個数以上
            if (_relicList.Count == 0 || index < 0 || index >= _relicList.Count)
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
            //リストが0個、indexが0未満,indexがリスト個数以上
            if (_relicList.Count == 0 || index < 0 || index >= _relicList.Count)
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
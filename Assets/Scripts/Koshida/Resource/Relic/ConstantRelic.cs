using System.Collections.ObjectModel;
using System;
using UnityEngine;
using UnityEngine.UI;

//定数のように使うやつ
//いい方法あれば変えたい

namespace Konji
{
    public static class CO
    {
        //マネージャーと共用すれば楽そう
        public enum RelicType
        {
            Cannon,         //大砲
            BlessingForest, //恵みの森
            MoneyTree,      //金のなる木
            WarDrum,        //陣太鼓
        }

        //名前情報
        public static readonly ReadOnlyCollection<string> RELIC_LIST =
                       Array.AsReadOnly(new string[]
                       {
                           "大砲",
                           "恵みの森",
                           "金のなる木",
                           "陣太鼓"
                       });

        public static readonly ReadOnlyCollection<Sprite> RELIC_IMAGE_LIST =
                        Array.AsReadOnly(new Sprite[]
                        {
                            Resources.Load<Sprite>("Images/Relic/cannon"),
                            Resources.Load<Sprite>("Images/Relic/bless"),
                            Resources.Load<Sprite>("Images/Relic/money"),
                            Resources.Load<Sprite>("Images/Relic/drum"),
                            Resources.Load<Sprite>("Images/Relic/selected")
                        });

        //遺物情報
        [System.Serializable]
        public class RelicInfo
        {
            public RelicType _type;
            public string _name;
            public bool _isSelect;
            public Sprite _image;

            public RelicInfo()
            {
                _type = RelicType.Cannon;
                _name = RELIC_LIST[(int)_type];
                _isSelect = false;
                _image = RELIC_IMAGE_LIST[(int)_type];
            }

            public RelicInfo(RelicType type)
            {
                _type = type;
                _name = RELIC_LIST[(int)type];
                _isSelect = false;
                _image = RELIC_IMAGE_LIST[(int)type];
            }
        }
    }
}
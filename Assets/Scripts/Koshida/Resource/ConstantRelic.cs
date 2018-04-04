using System.Collections.ObjectModel;
using System;

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
            WarDrum         //陣太鼓
        }

        public static readonly ReadOnlyCollection<string> RELIC_LIST =
                       Array.AsReadOnly(new string[]
                       {
                           "大砲",
                           "恵みの森",
                           "金のなる木",
                           "陣太鼓"
                       });
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Konji
{
    public abstract class Relic : MonoBehaviour
    {
        //名前
        protected string _name;
        public string Name
        {
            get { return _name; }
        }

        //ID
        protected CO.RelicType _type;   //もうちょいいい方法あるやろ
        public CO.RelicType Type
        {
            get { return _type; }
        }

        //置かれているかどうか
        private BoolReactiveProperty _isPut = new BoolReactiveProperty(false);
        public bool IsPut
        {
            get { return _isPut.Value; }
            set { _isPut.Value = value; }
        }

        protected virtual void Start()
        {
            _isPut.Where(put => put == true)
                .Subscribe(_ =>
                {
                    //置かれたときの処理
                    Debug.Log("Put[" + Name + "]");
            });
        }
    }
}
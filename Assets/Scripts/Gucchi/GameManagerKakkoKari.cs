using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GucchiCS
{
    public class GameManagerKakkoKari : SingletonMonoBehaviour<GameManagerKakkoKari>
    {
        // たぶんへっちーのほうでこういう管理を作ってると思うので仮で
        bool _isPlay = false;

        public bool IsPlay
        {
            get { return _isPlay; }
            set { _isPlay = value; }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Konji
{
    public class ResourceManager : SingletonMonoBehaviour<ResourceManager>
    {
        //資源
        private int _resource = 0;
        public int Resource
        {
            get { return _resource; }
            set
            {
                //0未満にならないように丸める
                _resource = value > 0 ? value : 0;
            }
        }

        //補間
        [SerializeField]
        private AnimationCurve _curve;

        //スコアテキスト
        public Text _scoreText;

        //スコア
        private int _score = 0;
        public int Score
        {
            get { return _score; }
            set
            {
                //スコアを徐々に更新
                int tmpScore = _score;
                _score = value;
                DOTween.To(
                    () => tmpScore,
                    num => _scoreText.text = num.ToString(),
                    _score,
                    2.0f)
                    .SetEase(_curve);
            }
        }

        // Use this for initialization
        void Start()
        {
            //資源の初期化
            _resource = 0;
            //スコアの初期化
            _score = 0;
        }
    }
}
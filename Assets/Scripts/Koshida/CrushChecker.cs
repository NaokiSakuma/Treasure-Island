using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Konji
{
    public class CrushChecker : MonoBehaviour
    {
        [SerializeField]
        private bool _crush = false;
        public bool IsCrush
        {
            get { return _crush; }
        }

        void Start()
        {
            //壁にめり込んだ
            this.OnTriggerEnterAsObservable()
                .Subscribe(col =>
                {
                    Debug.Log("Enter" + name);
                    _crush = true;
                });

            //壁から離れた
            this.OnTriggerExitAsObservable()
                .Subscribe(col =>
                {
                    Debug.Log("Exit" + name);
                    _crush = false;
                });
        }
    }
}
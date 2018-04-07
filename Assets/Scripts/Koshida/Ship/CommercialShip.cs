using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Konji
{
    public class CommercialShip : Ship
    {
        //次の行き先(画面外ってどこ)
        private Vector3 _nextTarget;

        //(できれば使いたくない)
        private float _startTimer;

        //再出発時間
        private float _startTime;

        protected override void Awake()
        {
            base.Awake();

            //フィールド内に適当に帰る(治さないと相当やばい)
            _nextTarget = new Vector3(Random.Range(-800, 800), 0, Random.Range(-450, 450));

            //暫定タイム
            _startTime = 5;
        }

        protected override void Start()
        {
            base.Start();

            //設定秒後に次の行き先へ
            this.UpdateAsObservable()
                .Where(_ => _startTimer >= _startTime)
                .Take(1)
                .Subscribe(_ => _navAgent.destination = _nextTarget);

            //停泊後の時間計測
            this.UpdateAsObservable()
                .Where(_ => _arrivalRP.Value)
                .Subscribe(_ =>
                {
                    _startTimer += Time.deltaTime;
                });

            //次の行き先に到着
            this.UpdateAsObservable()
                .Where(_ => (this.transform.position.x == _nextTarget.x)
                         && (this.transform.position.z == _nextTarget.z))    //思いつきませんでした
                .Subscribe(_ => Destroy(this.gameObject));
        }
    }
}
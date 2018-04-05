using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Konji
{
    public class Cannon : Relic
    {
        [SerializeField]
        private float _range = 300;     //射程

        public int _team = -1;          //所属チーム(-1:未所属、0:プレイヤー、*:それ以外) 仮のものだからいつかユニット側と統一

        [SerializeField]
        private int _interval = 6;      //発射間隔

        [SerializeField]
        private GameObject _ammunition; //砲弾のプレハブ

        protected override void Start()
        {
            base.Start();

            //種類の設定
            _type = CO.RelicType.Cannon;

            //名前の設定
            _name = CO.RELIC_LIST[(int)_type];

            //デバック用
            IsPut = true;

            //置かれているときn秒間隔ごとに大砲発射
            Observable.Interval(System.TimeSpan.FromSeconds(_interval))
                .Where(_ => IsPut)
                .Select(_ => SearchUnitPosition(_range))        //最近のユニットを検索
                .Where(target => target != null)                //ユニットが見つからない
                .Subscribe(point =>
                {
                    Shoot(point.position);
                })
                .AddTo(this);
        }


        //砲弾発射シーケンス
        private void Shoot(Vector3 targetPosition)
        {
            ShootFixedAngle(targetPosition, 60.0f);
        }

        //発射角度の計算
        private void ShootFixedAngle(Vector3 targetPosition, float i_angle)
        {
            float speedVec = ComputeVectorFromAngle(targetPosition, i_angle);
            if (speedVec <= 0.0f)
            {
                return;
            }

            Vector3 vec = ConvertVectorToVector3(speedVec, i_angle, targetPosition);
            InstantiateShootObject(vec);
        }

        //距離と角度の計算
        private float ComputeVectorFromAngle(Vector3 targetPosition, float i_angle)
        {
            // xz平面の距離を計算。
            Vector2 startPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
            Vector2 targetPos = new Vector2(targetPosition.x, targetPosition.z);
            float distance = Vector2.Distance(targetPos, startPos);

            float x = distance;
            float g = Physics.gravity.y;
            float y0 = gameObject.transform.position.y;
            float y = targetPosition.y;

            //ラジアンに変換
            float rad = i_angle * Mathf.Deg2Rad;

            float cos = Mathf.Cos(rad);
            float tan = Mathf.Tan(rad);

            float v0Square = g * x * x / (2 * cos * cos * (y - y0 - x * tan));

            //虚数の排除
            if (v0Square <= 0.0f)
            {
                return 0.0f;
            }

            float v0 = Mathf.Sqrt(v0Square);
            return v0;
        }

        //場所と角度の変換
        private Vector3 ConvertVectorToVector3(float i_v0, float i_angle, Vector3 targetPosition)
        {
            Vector3 startPos = gameObject.transform.position;
            Vector3 targetPos = targetPosition;
            startPos.y = 0.0f;
            targetPos.y = 0.0f;

            Vector3 dir = (targetPos - startPos).normalized;
            Quaternion yawRot = Quaternion.FromToRotation(Vector3.right, dir);
            Vector3 vec = i_v0 * Vector3.right;

            vec = yawRot * Quaternion.AngleAxis(i_angle, Vector3.forward) * vec;

            return vec;
        }

        //砲弾発射
        private void InstantiateShootObject(Vector3 i_shootVector)
        {
            if (_ammunition == null)
            {
                throw new System.NullReferenceException("Nossing Prefab");
            }

            var obj = Instantiate<GameObject>(_ammunition, gameObject.transform.position, Quaternion.identity);
            var rigidbody = obj.AddComponent<Rigidbody>();

            //この辺やばい
            {
                Ammunition amm = obj.GetComponent<Ammunition>();
                amm._team = _team;
            }
            Vector3 force = i_shootVector * rigidbody.mass;

            rigidbody.AddForce(force, ForceMode.Impulse);
        }

        //最近の敵対ユニット検索
        private Transform SearchUnitPosition(float range)
        {
            Transform point = null;

            //最短距離
            float minDis = range;

            //範囲内のユニットを検索
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);

            foreach(Collider target in hitColliders)
            {
                //ユニットのステータス
                UnitCore core = target.GetComponent<UnitCore>();

                //ユニットの掴み情報
                GucchiCS.Unit unitInfo = target.GetComponent<GucchiCS.Unit>();

                //ユニットが見つからないか死亡済み、もしくは所属チームが同じ 掴まれている場合
                if((core == null || core.Health <= 0 || core.Team == _team ) || (unitInfo == null || unitInfo.IsClutched))
                {
                    continue;
                }

                //対象との距離を求める
                float dis = Vector3.Distance(transform.position, target.transform.position);

                //最短距離の更新
                if(dis < minDis)
                {
                    minDis = dis;
                    point = target.transform;
                }
            }
            return point;
        }
    }
}
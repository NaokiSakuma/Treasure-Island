using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

public class Ammunition : MonoBehaviour
{
    [SerializeField]
    private int _damage = 1;        //ダメージ

    [SerializeField]
    private float _range = 10;      //爆発範囲

    public int _team = -1;          //所属

    void OnTriggerEnter(Collider col)
    {
        GucchiCS.Island island = null;

        island = col.gameObject.GetComponent<GucchiCS.Island>();

        //島に当たったら
        if (island != null)
        {
            //着弾地点の算出
            Vector3 hitPoint = col.ClosestPointOnBounds(this.transform.position);

            //Resourceフォルダからパーティクルを検索
            GameObject blast = (GameObject)Resources.Load("Particles/Blast");

            //生成(この辺もやばい)
            Instantiate(blast, this.transform.position, Quaternion.identity);

            //ダメージ
            DamageUnit();

            //弾消去
            Destroy(this.gameObject);
        }
    }

    //範囲内の敵対ユニットにダメージ
    void DamageUnit()
    {
        //範囲内のユニットを検索
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _range);

        //範囲内のユニットにダメージを与える
        foreach (Collider target in hitColliders)
        {
            //ユニットのステータス
            UnitCore core = target.GetComponent<UnitCore>();

            //ユニットの掴み情報
            GucchiCS.Unit unitInfo = target.GetComponent<GucchiCS.Unit>();

            //ユニットが見つからないか死亡済み、もしくは所属チームが同じ 掴まれている場合
            if ((core == null || core.Health <= 0 || core.Team == _team) || (unitInfo == null || unitInfo.IsClutched))
            {
                continue;
            }

            //ダメージを与える
            core.Health -= _damage;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(UnitCore))]
public class UnitMover : MonoBehaviour {

	UnitCore _core;

	// 近づく限界距離
	[SerializeField]
	private float _limitDistance = 2.0f;

	private Vector3 _velocity = Vector3.zero;

	// 移動中か
	private BoolReactiveProperty _isMoving = new BoolReactiveProperty(false);

    // 拠点の位置
    private Vector3 _basePos = Vector3.zero;

    public BoolReactiveProperty IsMoving{
		get { return _isMoving; }
	}

	// Use this for initialization
	void Start() {
		_core = GetComponent<UnitCore>();

        // inspecterでやった方がいいかもしれん　baseが動く可能性有なら修正しないと
        _basePos = GameObject.Find("base").transform.position;

		// 移動中かどうかを更新する
		this.UpdateAsObservable()
		    // ターゲットがいなくなったら停止
		    .Select(x => _core.Target != null)
		    .Subscribe(x => _isMoving.SetValueAndForceNotify(x));

        // 移動中の処理を2つ書くのはおばかちん
        this.UpdateAsObservable()
		    .Where(_ => !_isMoving.Value && IsTooNearIsland() &&  _core.Target == null)
			.Subscribe(_ => {
                transform.LookAt(new Vector3(_basePos.x,_basePos.y,_basePos.z));
                _velocity = transform.forward * _core.MoveSpeed * Time.deltaTime;
                transform.position += _velocity;

            });

		// 移動
		this.UpdateAsObservable()
		    .Where(_ => _isMoving.Value)
			.Subscribe(_ => {
				// 移動
				transform.position += _velocity;
			});
	}

	/// <summary>
	/// 近すぎないかを返す
	/// </summary>
	/// <returns><c>true</c>, if too near was checked, <c>false</c> otherwise.</returns>
	bool IsTooNear() {
		return _limitDistance < Vector3.Distance(transform.position, _core.Target.transform.position);
	}

    /// <summary>
    /// targetに出来るようにした方がいいかもしれん
    /// </summary>
    /// <returns></returns>
    bool IsTooNearIsland()
    {
        return _limitDistance < Vector3.Distance(transform.position, _basePos);
    }

    /// <summary>
    /// 一番近くの敵に向かって移動する
    /// </summary>
    public void MoveToNearestEnemy() {
		// ターゲットのほうを向く
		transform.LookAt(_core.Target.transform.position);
		if(IsTooNear()){
			_velocity = transform.forward * _core.MoveSpeed * Time.deltaTime;
		}
		else{
			_velocity = Vector3.zero;
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class BaseHp : MonoBehaviour {

    // 拠点のhp
    [SerializeField]
    private int _hp = 0;
    public int Hp
    {
        get { return _hp; }
        set { _hp = value;
            _hp = _hp < 0 ? 0 : _hp;
        }
    }

    void Start()
    {
        this.UpdateAsObservable()
            .Subscribe(_ => print(_hp));
    }
}

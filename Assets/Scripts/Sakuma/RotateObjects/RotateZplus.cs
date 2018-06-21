using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;

public class RotateZplus : RotateObjects {
    protected override void Start()
    {
        base.Start();
        this.UpdateAsObservable()
            .Where(_ => !_parent.activeSelf)
            .Subscribe(_ => _button.gameObject.SetActive(true));

        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.E))
            .Where(_ => CanRotate())
            .Subscribe(_ => this.RotateMethod());
    }
    protected override void OnClickRotate()
    {
        _rotateObj.transform.DORotate(new Vector3(0, 0, 90f), _animationTime, RotateMode.WorldAxisAdd).SetRelative().OnComplete(StopRotate);
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;

public class RotateXplus : RotateObjects {
    protected override void Start()
    {
        base.Start();
        this.UpdateAsObservable()
            .Where(_ => !_parent.activeSelf)
            .Subscribe(_ => _button.gameObject.SetActive(true));
    }

    protected override void OnClickRotate()
    {
        _rotateObj.transform.DORotate(new Vector3(90f, 0, 0), _animationTime, RotateMode.WorldAxisAdd).SetRelative().OnComplete(StopRotate);
    }
}

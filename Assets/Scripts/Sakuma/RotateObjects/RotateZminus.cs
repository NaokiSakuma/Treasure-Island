using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateZminus : RotateObjects
{
    protected override void Start()
    {
        base.Start();
        _button.gameObject.SetActive(false);
    }
    protected override void OnClickRotate()
    {
        _rotateObj.transform.DORotate(new Vector3(0, 0, -90f), _animationTime, RotateMode.WorldAxisAdd).SetRelative().OnComplete(StopRotate);
    }
}

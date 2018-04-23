using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateXplus : RotateObjects {
    protected override void OnClickRotate()
    {
        _rotateObj.transform.DORotate(new Vector3(90f, 0, 0), _animationTime, RotateMode.WorldAxisAdd).SetRelative().OnComplete(StopRotate);
    }
}

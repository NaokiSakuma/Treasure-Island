using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateYminus : RotateObjects {
    protected override void OnClickRotate()
    {
        _rotateObj.transform.DORotate(new Vector3(0, -90f, 0), _animationTime, RotateMode.WorldAxisAdd).SetRelative().OnComplete(StopRotate);
    }
}

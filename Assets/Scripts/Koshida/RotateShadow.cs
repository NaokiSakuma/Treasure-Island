using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class RotateShadow : MonoBehaviour
{
    [SerializeField]
    private Transform _appearObj;

    [SerializeField]
    private float _collisonSize = 1;

    [SerializeField]
    private float _moveamo = 3.5f;

    [SerializeField]
    private GameObject _light;

    //最大角度
    public float _maxRot = 10.0f;

    //初期地点
    private Vector3 _startPosition;
    private Vector3 _startCollisionSize;

    // Use this for initialization
    void Start()
    {
        _startPosition = transform.position;
        _startCollisionSize = transform.localScale;

        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                transform.localRotation = _appearObj.localRotation;
            });

        //ライトがY方向に変化したとき、影の判定をずらす
        this.ObserveEveryValueChanged(_ => _light.transform.localEulerAngles.y)
            .Select(rotateY => (rotateY >= _maxRot ? rotateY - 360 : rotateY) / _maxRot)
            .Where(rot => Mathf.Abs(rot) <= 1.0f)
            .Subscribe(r =>
            {
                UpdateScaleX(_collisonSize * Mathf.Abs(r));
                transform.position = _startPosition + new Vector3(_moveamo * r, 0, 0);
            });

        //ライトがX方向に変化したとき、影の判定をずらす
        this.ObserveEveryValueChanged(_ => _light.transform.localEulerAngles.x)
            .Select(rotateX => (rotateX >= _maxRot ? rotateX - 360 : rotateX) / _maxRot)
            .Where(rot => Mathf.Abs(rot) <= 1.0f)
            .Subscribe(r =>
            {
                UpdateScaleY(_collisonSize * Mathf.Abs(r));
                transform.position = _startPosition + new Vector3(0, -_moveamo * r, 0);
            });

    }

    //Y軸の角度によって拡縮する軸の変更
    void UpdateScaleX(float sizeX)
    {
        if (Mathf.Abs(_appearObj.transform.forward.x) >= 0.98f)
        {
            transform.transform.localScale = _startCollisionSize + new Vector3(0,   0, sizeX);
        }

        if (Mathf.Abs(_appearObj.transform.right.x) >= 0.98f)
        {
            transform.transform.localScale = _startCollisionSize + new Vector3(sizeX, 0, 0);
        }

        if (Mathf.Abs(_appearObj.transform.up.x) >= 0.98f)
        {
            transform.transform.localScale = _startCollisionSize + new Vector3(0, sizeX, 0);
        }
    }

    //Y軸の角度によって拡縮する軸の変更
    void UpdateScaleY(float sizeY)
    {
        if (Mathf.Abs(_appearObj.transform.forward.y) >= 0.98f)
        {
            transform.transform.localScale = _startCollisionSize + new Vector3(0, 0, sizeY);
        }

        if (Mathf.Abs(_appearObj.transform.right.y) >= 0.98f)
        {
            transform.transform.localScale = _startCollisionSize + new Vector3(sizeY, 0, 0);
        }

        if (Mathf.Abs(_appearObj.transform.up.y) >= 0.98f)
        {
            transform.transform.localScale = _startCollisionSize + new Vector3(0, sizeY, 0);
        }
    }

    //valueの値に近かったらture
    bool CheckNearValue(int baseValue,int value)
    {
        bool n = false;

        if (value == baseValue - 1 || value == baseValue + 1 || value == baseValue)
        {
            n = true;
        }

        return n;
    }

    ////X軸の角度によって拡縮する軸の変更
    //void UpdateScaleY(float rotateX , float sizeY)
    //{
    //    Debug.Log(rotateX);

    //    if (rotateX == 0 || rotateX == 180 || rotateX == 360)
    //    {
    //        transform.localScale = _startCollisionSize + new Vector3(0, sizeY, 0);
    //    }
    //    else if(rotateX == 90 || rotateX == 270)
    //    {
    //        transform.localScale = _startCollisionSize + new Vector3(0, 0, sizeY);
    //    }
    //}

    //rX:0 - sY
    //rX:90 - sZ
    //rX:180 - sY
    //rX:270 - sZ
    //rX:360 - sY

}

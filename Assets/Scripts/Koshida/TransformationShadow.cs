using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TransformationShadow : MonoBehaviour
{
    [SerializeField]
    private Transform _appearObj;

    [SerializeField]
    private Vector2 _collisonSize = new Vector2(1,1);

    [SerializeField]
    private Vector2 _moveamo = new Vector2(3.5f,3.5f);

    [SerializeField]
    private GameObject _light;

    //最大角度
    private float _maxRot = 10.0f;

    //初期地点
    private Vector3 _startPosition;
    private Vector3 _startCollisionSize;

    // Use this for initialization
    void Start()
    {
        _startPosition = transform.position;
        _startCollisionSize = transform.localScale;
        _maxRot = _light.GetComponent<GucchiCS.LightController>().LimitAngle;

        //回転させるオブジェクトと同期
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                transform.localRotation = _appearObj.localRotation;
            });

        //ライトの方向によって影の判定をずらす
        this.ObserveEveryValueChanged(_ => _light.GetComponent<GucchiCS.LightController>().EulerAngles)
            .Subscribe(rotate =>
            {
                UpdateShadow(rotate);
            });
    }

    //影判定の変形
    void UpdateShadow(Vector3 objRot)
    {
        //ライトの変更を係数に
        float shadowXAmo = objRot.y / _maxRot;
        float shadowYAmo = objRot.x / _maxRot;

        float absX = Mathf.Abs(shadowXAmo);
        float absY = Mathf.Abs(shadowYAmo);

        //影のサイズ変更
        if (absX <= 1.0f && absY <= 1.0f)
        {
            UpdateScale(_collisonSize.x * absX, _collisonSize.y * absY);
        }

        //影の移動
        transform.position = _startPosition + new Vector3(_moveamo.x * shadowXAmo, _moveamo.y * -shadowYAmo, 0);
    }

    //軸の角度によって拡縮する軸の変更
    void UpdateScale(float sizeX,float sizeY)
    {
        Vector3 offsetSize = _startCollisionSize;

        //オブジェクトの前がX軸方向
        if (Mathf.Abs(_appearObj.transform.forward.x) >= 0.98f)
        {
            offsetSize += new Vector3(0,   0, sizeX);
        }

        //オブジェクトの右がX軸方向
        if (Mathf.Abs(_appearObj.transform.right.x) >= 0.98f)
        {
            offsetSize += new Vector3(sizeX, 0, 0);
        }

        //オブジェクトの上がX軸方向
        if (Mathf.Abs(_appearObj.transform.up.x) >= 0.98f)
        {
            offsetSize += new Vector3(0, sizeX, 0);
        }


        //オブジェクトの前がY軸方向
        if (Mathf.Abs(_appearObj.transform.forward.y) >= 0.98f)
        {
            offsetSize += new Vector3(0, 0, sizeY);
        }

        //オブジェクトの右がY軸方向
        if (Mathf.Abs(_appearObj.transform.right.y) >= 0.98f)
        {
            offsetSize += new Vector3(sizeY, 0, 0);
        }

        //オブジェクトの上がY軸方向
        if (Mathf.Abs(_appearObj.transform.up.y) >= 0.98f)
        {
            offsetSize += new Vector3(0, sizeY, 0);
        }

        //差分の分変更
        transform.transform.localScale = offsetSize;
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
}

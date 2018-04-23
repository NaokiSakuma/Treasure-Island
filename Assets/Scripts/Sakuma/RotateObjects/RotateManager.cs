using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RotateXplus))]
[RequireComponent(typeof(RotateXminus))]
[RequireComponent(typeof(RotateYplus))]
[RequireComponent(typeof(RotateYminus))]
[RequireComponent(typeof(RoteteZplus))]
[RequireComponent(typeof(RotateZminus))]

public class RotateManager : MonoBehaviour
{

    // アニメーションする時間
    [SerializeField]
    private float _animationTime = 0f;
    public float AnimationTime
    {
        get { return _animationTime; }
    }

    // マウスのrayにhitしたオブジェクト
    private GameObject _hitObj = null;
    public GameObject HitObj
    {
        get { return _hitObj; }
    }

    // オブジェクトが回転中か
    private bool _isRotate = false;
    public bool IsRotate
    {
        set { _isRotate = value; }
        get { return _isRotate; }
    }

    // canvas
    [SerializeField]
    private Canvas _canvas = null;

    // ボタンマネージャー
    [SerializeField]
    private GameObject _buttonManager = null;

    // レイヤー
    [SerializeField]
    private LayerMask layerMask;

    void Awake()
    {
        _canvas.gameObject.SetActive(false);
    }
    void Start()
    {
        
        // 左クリックされた時の処理
        this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButton(0))
            .Where(_ => !_isRotate)
            .Subscribe(_ =>
            {
                // マウスの位置からrayを飛ばす
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();
                // オブジェクトがあれば登録
                if (Physics.Raycast(ray, out hit, Mathf.Infinity,layerMask.value))
                {
                    _hitObj = hit.collider.gameObject;
                    print("当たったオブジェクト：" + _hitObj.transform.position);
                    _canvas.gameObject.SetActive(true);
                }
                else
                {
                    // _canvas.gameObject.SetActive(false);
                }
            });

        this.UpdateAsObservable()
            .Subscribe(_ => print("roteta：" + _isRotate));
        // TODO 40fズレる
        this.UpdateAsObservable()
            .Where(_ => _hitObj != null)
            .Subscribe(_ => 
            {
                var cameraPos = Camera.main.WorldToScreenPoint(_hitObj.transform.position);
                _buttonManager.transform.position = new Vector3(cameraPos.x, cameraPos.y + 40f, cameraPos.z);

            });
    }
}
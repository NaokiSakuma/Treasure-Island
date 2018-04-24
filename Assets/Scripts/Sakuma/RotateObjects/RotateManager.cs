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
    private LayerMask layerMask = 0;

    // test
    [SerializeField]
    private RectTransform _canvasRect = null;

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
                // uGUIと重なっていたらreturn
                if (EventSystem.current.IsPointerOverGameObject()) return;
                // マウスの位置からrayを飛ばす
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();
                // オブジェクトがあれば登録
                if (Physics.Raycast(ray, out hit, Mathf.Infinity,layerMask.value))
                {
                    _hitObj = hit.collider.gameObject;
                    var rect = _buttonManager.GetComponent<RectTransform>();
                    rect.sizeDelta = buttonManagerRect();
                    _canvas.gameObject.SetActive(true);
                }
                else
                {
                     _canvas.gameObject.SetActive(false);
                }
            });

        // TODO 40fズレる
        this.UpdateAsObservable()
            .Where(_ => _hitObj != null)
            .Subscribe(_ => 
            {
                //var rect = _hitObj.GetComponent<RectTransform>();
                //var target = _buttonManager.GetComponent<RectTransform>();
                //_buttonManager.transform.position = canvasPosition(_hitObj.transform.position.x, _hitObj.transform.position.y);
                var cameraPos = Camera.main.WorldToViewportPoint(_hitObj.transform.position);
                var cameraPos2 = Camera.main.WorldToScreenPoint(_hitObj.transform.position);
                var CanvasRect = _canvas.GetComponent<RectTransform>();
                Vector2 WorldObject_ScreenPosition = new Vector2(
                    ((cameraPos2.x)),
                    ((cameraPos.y * CanvasRect.sizeDelta.y)));
                _buttonManager.transform.position = WorldObject_ScreenPosition;//new Vector3(cameraPos.x, cameraPos.y, cameraPos.z);

            });
    }
    /// <summary>
    /// ボタンマネージャーのrectTransform
    /// </summary>
    /// <returns>x：width、y：height</returns>
    private Vector2 buttonManagerRect()
    {
        return new Vector2(100.0f - 30.0f * (_hitObj.gameObject.GetComponent<Renderer>().bounds.size.x - 1.0f),
                                100.0f - 40.0f * (_hitObj.gameObject.GetComponent<Renderer>().bounds.size.y - 1.0f));

    }
}
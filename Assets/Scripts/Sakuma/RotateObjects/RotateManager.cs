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

    // マウスのrayにhitしたオブジェクト（仮選択状態）
    private GameObject _hitObj = null;
    public GameObject HitObj
    {
        get { return _hitObj; }
    }

    // 初期で仮選択させたいオブジェクト
    [SerializeField]
    GameObject _defaultTempSelectObj = null;

    // 選択したオブジェクト
    private GameObject _selectedObj = null;
    public GameObject SelectedObj
    {
        get { return _selectedObj; }
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

    // 回転軸オブジェクト
    [SerializeField]
    private GameObject _rotateObj = null;

    // rayと当たるレイヤーマスク
    [SerializeField]
    private LayerMask layerMask = 0;

    void Awake()
    {
        _buttonManager.gameObject.SetActive(false);
    }
    void Start()
    {
        // オブジェクトコントロールモードでの処理
        this.UpdateAsObservable()
            .Where(_ => GucchiCS.StageManager.Instance.IsPlay)
            .Where(_ => { return (GucchiCS.ModeChanger.Instance.Mode == GucchiCS.ModeChanger.MODE.OBJECT_CONTROL) || (GucchiCS.ModeChanger.Instance.Mode == GucchiCS.ModeChanger.MODE.OBJECT_CONTROL_SELECTED); })
            .Where(_ => !GucchiCS.ModeChanger.Instance.IsChanging)
            .Where(_ => !_isRotate)
            .Where(_ => !EventSystem.current.IsPointerOverGameObject())
            .Subscribe(_ =>
            {
                // マウスの位置からrayを飛ばす
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();

                // 左クリック時
                if (Input.GetMouseButtonDown(0))
                {
                    // オブジェクトがあれば登録
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask.value) && _hitObj)
                    {
                        _selectedObj = _hitObj;
                        GucchiCS.ModeChanger.Instance.SelectedObject = _selectedObj;
                        GucchiCS.ModeChanger.Instance.Mode = GucchiCS.ModeChanger.MODE.OBJECT_CONTROL_SELECTED;
                    }
                    // 無ければUIを消す
                    else
                    {
                        _selectedObj = null;
                        _buttonManager.gameObject.SetActive(false);
                        _rotateObj.SetActive(false);
                        GucchiCS.ModeChanger.Instance.SelectedObject = null;
                        GucchiCS.ModeChanger.Instance.Mode = GucchiCS.ModeChanger.MODE.OBJECT_CONTROL;
                    }
                }
                else
                {
                    // 選択していない状態でオブジェクトがあれば仮選択
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask.value))
                    {
                        _hitObj = hit.collider.gameObject;
                    }
                    else
                    {
                        if (!_hitObj)
                        {
                            _hitObj = _defaultTempSelectObj;
                        }
                    }
                }
            });

        //this.UpdateAsObservable()
        //    .Subscribe(_ =>
        //    {
        //        Debug.Log("hitObj = " + (_hitObj ? _hitObj.name : "null"));
        //        Debug.Log("selectedObj = " + (_selectedObj ? _selectedObj.name : "null"));
        //    });

        // rayが当たっているオブジェクトを監視
        this.ObserveEveryValueChanged(x => _selectedObj)
            .Where(_ => _selectedObj != null)
            .Subscribe(_ =>
            {
                // 再びつける
                StartCoroutine(TurnOnAgain());
            });


        // ボタンを回転させるUIの表示位置
        this.UpdateAsObservable()
            .Where(_ => _selectedObj != null)
            .Where(_ => { return (GucchiCS.ModeChanger.Instance.Mode == GucchiCS.ModeChanger.MODE.OBJECT_CONTROL) || (GucchiCS.ModeChanger.Instance.Mode == GucchiCS.ModeChanger.MODE.OBJECT_CONTROL_SELECTED); })
            .Subscribe(_ =>
            {
                // カメラのスクリーン座標
                var cameraScreen = Camera.main.WorldToScreenPoint(_selectedObj.transform.position);

                //回転軸の位置調整
                _rotateObj.transform.position = _selectedObj.transform.position;

                //// buttonManagerの場所
                //Vector2 objectPosition = new Vector2(cameraScreen.x, cameraScreen.y);
                //_buttonManager.transform.position = objectPosition;

            });

        // ゲームモードによってbuttonManagerを消す
        this.UpdateAsObservable()
            .Where(_ => GucchiCS.ModeChanger.Instance.Mode != GucchiCS.ModeChanger.MODE.OBJECT_CONTROL)
            .Where(_ => GucchiCS.ModeChanger.Instance.Mode != GucchiCS.ModeChanger.MODE.OBJECT_CONTROL_SELECTED)
            .Subscribe(_ =>
            {
                _buttonManager.gameObject.SetActive(false);
                _selectedObj = null;

                //回転軸の非表示
                _rotateObj.SetActive(false);
            });
    }
    /// <summary>
    /// ボタンマネージャーのrectTransform
    /// </summary>
    /// <returns>x：width、y：height</returns>
    private Vector2 buttonManagerRect()
    {
        // オブジェクトのサイズ
        var objSize = _selectedObj.gameObject.GetComponent<Renderer>().bounds.size;
        // オブジェクトのxyzで一番大きいサイズ
        var maxSize = Mathf.Max(objSize.x, objSize.y, objSize.z);
        // buttonManagerのデフォルトのwidth,hitght
        const float defaultWH = 20.0f;
        // いい感じの位置に配置
        // 30.0f,40.0fは良い感じのUIの配置位置
        // 1.0fはscaleを1.0fを基準に作っているため引いている
        return new Vector2(defaultWH - 30.0f * (maxSize - 1.0f),
                                defaultWH - 40.0f * (maxSize - 1.0f));

    }

    /// <summary>
    /// 再びbuttonManagerを付ける
    /// </summary>
    IEnumerator TurnOnAgain()
    {
        _buttonManager.gameObject.SetActive(false);
        yield return null;
        _buttonManager.gameObject.SetActive(true);
        _rotateObj.SetActive(true);
    }
}
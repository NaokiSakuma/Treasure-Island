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

    // rayと当たるレイヤーマスク
    [SerializeField]
    private LayerMask layerMask = 0;

    void Awake()
    {
        _buttonManager.gameObject.SetActive(false);
    }
    void Start()
    {

        // 左クリックされた時の処理
        this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonDown(0))
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
                // オブジェクトがあれば登録
                if (Physics.Raycast(ray, out hit, Mathf.Infinity,layerMask.value))
                {
                    _hitObj = hit.collider.gameObject;
                    GucchiCS.ModeChanger.Instance.SelectedObject = _hitObj;
                    GucchiCS.ModeChanger.Instance.Mode = GucchiCS.ModeChanger.MODE.OBJECT_CONTROL_SELECTED;

                    // ボタンのrectTransFormを変更 必要になったら復旧
                    // var rect = _buttonManager.GetComponent<RectTransform>();
                    // rect.sizeDelta = buttonManagerRect();
                }
                // 無ければUIを消す
                else
                {
                    _hitObj = null;
                    _buttonManager.gameObject.SetActive(false);
                    GucchiCS.ModeChanger.Instance.SelectedObject = null;
                    GucchiCS.ModeChanger.Instance.Mode = GucchiCS.ModeChanger.MODE.OBJECT_CONTROL;
                }
            });
        　
        // rayが当たっているオブジェクトを監視
        this.ObserveEveryValueChanged(x => _hitObj)
            .Where(_ => _hitObj != null)
            .Subscribe(_ =>
            {
                // 再びつける
                StartCoroutine(TurnOnAgain());
            });


        // ボタンを回転させるUIの表示位置
        this.UpdateAsObservable()
            .Where(_ => _hitObj != null)
            .Where(_ => { return (GucchiCS.ModeChanger.Instance.Mode == GucchiCS.ModeChanger.MODE.OBJECT_CONTROL) || (GucchiCS.ModeChanger.Instance.Mode == GucchiCS.ModeChanger.MODE.OBJECT_CONTROL_SELECTED); })
            .Subscribe(_ =>
            {                
                // カメラの設定に応じて使う必要有
                // カメラのビューポート座標
                // var cameraView = Camera.main.WorldToViewportPoint(_hitObj.transform.position);
                // カメラのスクリーン座標
                var cameraScreen = Camera.main.WorldToScreenPoint(_hitObj.transform.position);
                // canvasのrectTransform
                // var canvasRect = _canvas.GetComponent<RectTransform>();
                // buttonManagerの場所
                Vector2 objectPosition = new Vector2(((cameraScreen.x)),((/*cameraView.y * canvasRect.sizeDelta.y*/cameraScreen.y)));
                _buttonManager.transform.position = objectPosition;

            });

        // ゲームモードによってbuttonManagerを消す
        this.UpdateAsObservable()
            .Where(_ => GucchiCS.ModeChanger.Instance.Mode != GucchiCS.ModeChanger.MODE.OBJECT_CONTROL)
            .Where(_ => GucchiCS.ModeChanger.Instance.Mode != GucchiCS.ModeChanger.MODE.OBJECT_CONTROL_SELECTED)
            .Subscribe(_ =>
            {
                _buttonManager.gameObject.SetActive(false);
                _hitObj = null;
            });

        // ポーズ画面に行ったとき
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Escape))
            .Subscribe(_ =>
            {
                _buttonManager.gameObject.SetActive(false);
                _hitObj = null;
            });

        // ポーズ画面に行ったとき
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Escape))
            .Subscribe(_ =>
            {
                _buttonManager.gameObject.SetActive(false);
                _hitObj = null;
            });
    }
    /// <summary>
    /// ボタンマネージャーのrectTransform
    /// </summary>
    /// <returns>x：width、y：height</returns>
    private Vector2 buttonManagerRect()
    {
        // オブジェクトのサイズ
        var objSize = _hitObj.gameObject.GetComponent<Renderer>().bounds.size;
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
    }
}
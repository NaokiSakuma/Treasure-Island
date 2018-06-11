using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RotateXplus))]
[RequireComponent(typeof(RotateXminus))]
[RequireComponent(typeof(RotateYplus))]
[RequireComponent(typeof(RotateYminus))]
[RequireComponent(typeof(RoteteZplus))]
[RequireComponent(typeof(RotateZminus))]

public class RotateManager : SingletonMonoBehaviour<RotateManager>
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

    // ステージ上のオブジェクト全体
    [SerializeField]
    private Transform _stageObject = null;

    // ステージ上のオブジェクト
    private List<Transform> _stageChildObjs = new List<Transform>();
   // private List<StageObject> stageObject;

    // 仮選択オブジェクトの要素番号
    private int _indexStageNum = 0;

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
    //[SerializeField]
    //private Canvas _canvas = null;


    // player
    [SerializeField]
    private Konji.PlayerControl _player = null;

    // ボタンマネージャー
    [SerializeField]
    private GameObject _buttonManager = null;

    // 回転軸オブジェクト
    [SerializeField]
    private GameObject _rotateObj = null;

    // rayと当たるレイヤーマスク
    [SerializeField]
    private LayerMask layerMask = 0;

    protected override void Awake()
    {
        _buttonManager.gameObject.SetActive(false);
        // ステージ上のオブジェクトを取得
        foreach (Transform child in _stageObject)
        {
            _stageChildObjs.Add(child);
            //stageObject.Add(child.GetComponent<StageObject>());

        }
    }
    void Start()
    {
        // オブジェクトを仮登録する
        this.UpdateAsObservable()
            .Take(1)
            .Subscribe(_ => 
            {
                _hitObj = _stageChildObjs[0].gameObject;
            });

        // モードチェンジャー
        var modeChanger = GucchiCS.ModeChanger.Instance;

        // オブジェクトコントロールモードでの処理
        this.UpdateAsObservable()
            .Where(_ => GucchiCS.StageManager.Instance.IsPlay)
            .Where(_ =>  (modeChanger.Mode == GucchiCS.ModeChanger.MODE.OBJECT_CONTROL) || (modeChanger.Mode == GucchiCS.ModeChanger.MODE.OBJECT_CONTROL_SELECTED))
            .Where(_ => !modeChanger.IsChanging)
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
                        if(_selectedObj)
                            _selectedObj.GetComponent<StageObject>().IsSelect = false;
                        _selectedObj = _hitObj;
                        modeChanger.SelectedObject = _selectedObj;
                        modeChanger.Mode = GucchiCS.ModeChanger.MODE.OBJECT_CONTROL_SELECTED;

                        // オブジェクトのエフェクトをONにする
                        _selectedObj.GetComponent<StageObject>().IsSelect = true;
                    }
                    // 無ければUIを消す
                    else
                    {
                        if(_selectedObj)
                            _selectedObj.GetComponent<StageObject>().IsSelect = false;
                        _selectedObj = null;
                        _buttonManager.gameObject.SetActive(false);
                        _rotateObj.SetActive(false);
                        modeChanger.SelectedObject = null;
                        modeChanger.Mode = GucchiCS.ModeChanger.MODE.OBJECT_CONTROL;
                    }
                }
                else
                {
                    // 選択していない状態でオブジェクトがあれば仮選択
                    if (GucchiCS.ControlState.Instance.IsStateMouse)
                    {
                        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask.value))
                        {
                            if(_hitObj)
                                _hitObj.GetComponent<StageObject>().IsTemporary = false;
                            _hitObj = hit.collider.gameObject;
                            _hitObj.GetComponent<StageObject>().IsTemporary = true;
                        }
                        else
                        {
                            if (_hitObj)
                                _hitObj.GetComponent<StageObject>().IsTemporary = false;
                              // 最初に仮選択されるオブジェクト
                              _hitObj = _stageChildObjs[0].gameObject;
                        }
                    }
                    // WASD仮選択
                    // 次の要素
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A))
                        {
                            _hitObj.GetComponent<StageObject>().IsTemporary = false;
                            _indexStageNum = (_indexStageNum + 1 >= _stageChildObjs.Count) ? 0 : ++_indexStageNum;
                            _hitObj = _stageChildObjs[_indexStageNum].gameObject;
                            _hitObj.GetComponent<StageObject>().IsTemporary = true;
                        }
                        // 前の要素
                        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
                        {
                            _hitObj.GetComponent<StageObject>().IsTemporary = false;
                            _indexStageNum = (_indexStageNum - 1 < 0) ? _stageChildObjs.Count - 1 : --_indexStageNum;
                            _hitObj = _stageChildObjs[_indexStageNum].gameObject;
                            _hitObj.GetComponent<StageObject>().IsTemporary = true;
                        }

                        // オブジェクトの選択
                        else if (Input.GetKeyDown(KeyCode.Return))
                        {
                            if (_hitObj == null) return;
                            if (_selectedObj != null)
                            {
                                _selectedObj.GetComponent<StageObject>().IsSelect = false;
                                _selectedObj = null;
                                _rotateObj.SetActive(false);
                                _buttonManager.gameObject.SetActive(false);
                                modeChanger.SelectedObject = null;
                                modeChanger.Mode = GucchiCS.ModeChanger.MODE.OBJECT_CONTROL;
                                return;
                            }
                            _hitObj.GetComponent<StageObject>().IsTemporary = false;
                            _selectedObj = _hitObj;
                            modeChanger.SelectedObject = _selectedObj;
                            modeChanger.Mode = GucchiCS.ModeChanger.MODE.OBJECT_CONTROL_SELECTED;
                            _selectedObj.GetComponent<StageObject>().IsSelect = true;
                        }
                    }
                }

            });

        this.UpdateAsObservable()
            .Where(_ => _player.IsDead)
            .Subscribe(_ =>
            {
                if(_selectedObj)
                    _selectedObj.GetComponent<StageObject>().IsSelect = false;
                _selectedObj = null;
                _rotateObj.SetActive(false);
                _buttonManager.gameObject.SetActive(false);
                modeChanger.SelectedObject = null;
                modeChanger.Mode = GucchiCS.ModeChanger.MODE.OBJECT_CONTROL;
            });

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
            .Where(_ => { return (modeChanger.Mode == GucchiCS.ModeChanger.MODE.OBJECT_CONTROL) || (modeChanger.Mode == GucchiCS.ModeChanger.MODE.OBJECT_CONTROL_SELECTED); })
            .Subscribe(_ =>
            {
                // カメラのスクリーン座標
                var cameraScreen = Camera.main.WorldToScreenPoint(_selectedObj.transform.position);

                //回転軸の位置調整
                _rotateObj.transform.position = _selectedObj.transform.position;


            });

        // ゲームモードによってbuttonManagerを消す
        this.UpdateAsObservable()
            .Where(_ => modeChanger.Mode != GucchiCS.ModeChanger.MODE.OBJECT_CONTROL)
            .Where(_ => modeChanger.Mode != GucchiCS.ModeChanger.MODE.OBJECT_CONTROL_SELECTED)
            .Subscribe(_ =>
            {
                if(_hitObj)
                _hitObj.GetComponent<StageObject>().IsTemporary = false;
                if(_selectedObj)
                _selectedObj.GetComponent<StageObject>().IsSelect = false;

                _buttonManager.gameObject.SetActive(false);
                _selectedObj = null;

                //回転軸の非表示
                _rotateObj.SetActive(false);
            });

        // ポーズ画面に行ったとき
        this.UpdateAsObservable()
            .Where(_ => Pausable.Instance.pausing || _player.IsDead)
            .Subscribe(_ =>
            {
                HideObject();
            });

        // 回転中はボタンを押せなくする（グレーアウト）
        this.ObserveEveryValueChanged(_ => _isRotate)
            .Where(_ => GucchiCS.StageManager.Instance.IsPlay)
            .Subscribe(_ =>
            {
                foreach (Transform button in _buttonManager.transform)
                {
                    button.GetComponent<Button>().interactable = !button.GetComponent<Button>().interactable;
                }
            });
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

    public void HideObject()
    {
        _buttonManager.gameObject.SetActive(false);
        if (_hitObj)
            _hitObj.GetComponent<StageObject>().IsTemporary = false;
        if (_selectedObj)
            _selectedObj.GetComponent<StageObject>().IsSelect = false;
        _hitObj = null;
    }
}
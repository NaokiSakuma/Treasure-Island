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
[RequireComponent(typeof(RotateZplus))]
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

    // mouseのray
    private Ray _mouseRay;

    private enum Effect
    {
        Temporary = 0,      // bule
        Select              // yellow
    }

    protected override void Awake()
    {
        _buttonManager.gameObject.SetActive(false);
        // ステージ上のオブジェクトを取得
        foreach (Transform child in _stageObject)
        {
            _stageChildObjs.Add(child);
        }
    }
    void Start()
    {
        // モードチェンジャー
        var modeChanger = GucchiCS.ModeChanger.Instance;
        // RaycastHit
        RaycastHit hit = new RaycastHit();
        // オブジェクトを触れるモードかどうか
        bool isTouchMode = false;
        // オブジェクトを回転させることが出来るかどうか
        bool canRotateObject = false;

        // デバッグ
        this.UpdateAsObservable()
            .Subscribe(_ => print("button SetActive：" + _buttonManager.activeSelf));

        // オブジェクトを仮登録する
        this.UpdateAsObservable()
            .Take(1)
            .Subscribe(_ =>
            {
                _hitObj = _stageChildObjs[0].gameObject;
            });

        // 更新
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                // マウスのrayの更新
                _mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                // オブジェクトを触れるかどうかの更新
                isTouchMode = (modeChanger.Mode == GucchiCS.ModeChanger.MODE.OBJECT_CONTROL) || (modeChanger.Mode == GucchiCS.ModeChanger.MODE.OBJECT_CONTROL_SELECTED);
                // オブジェクトを回転させることが出来るかどうかの更新
                canRotateObject = GucchiCS.StageManager.Instance.IsPlay && isTouchMode && !modeChanger.IsChanging && !_isRotate;
            });


        // オブジェクトコントロールモードでの処理
        this.UpdateAsObservable()
            .Where(_ => canRotateObject)
            .Where(_ => !EventSystem.current.IsPointerOverGameObject())
            .Subscribe(_ =>
            {
                // 左クリック時
                if (Input.GetMouseButtonDown(0))
                {
                    // オブジェクトがあれば登録
                    if (Physics.Raycast(_mouseRay, out hit, Mathf.Infinity, layerMask.value) && _hitObj)
                    {
                        //if(_selectedObj)
                        //    _selectedObj.GetComponent<StageObject>().IsSelect = false;

                        ChangeEffect(_selectedObj, Effect.Select);
                        _selectedObj = _hitObj;
                        modeChanger.SelectedObject = _selectedObj;
                        modeChanger.Mode = GucchiCS.ModeChanger.MODE.OBJECT_CONTROL_SELECTED;

                        // オブジェクトのエフェクトをONにする
                        //_selectedObj.GetComponent<StageObject>().IsSelect = true;
                        ChangeEffect(_selectedObj, Effect.Select);
                    }
                    // 無ければUIを消す
                    else
                    {
                        //if(_selectedObj)
                        //    _selectedObj.GetComponent<StageObject>().IsSelect = false;

                        ChangeEffect(_selectedObj, Effect.Select);

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
                        if (Physics.Raycast(_mouseRay, out hit, Mathf.Infinity, layerMask.value))
                        {
                            ChangeEffect(_hitObj, Effect.Temporary);
                            _hitObj = hit.collider.gameObject;
                            ChangeEffect(_hitObj, Effect.Temporary, true);
                        }
                        else
                        {
                            ChangeEffect(_hitObj, Effect.Temporary);
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
                            ChangeEffect(_hitObj, Effect.Temporary);
                            _indexStageNum = (_indexStageNum + 1 >= _stageChildObjs.Count) ? 0 : ++_indexStageNum;
                            _hitObj = _stageChildObjs[_indexStageNum].gameObject;
                            ChangeEffect(_hitObj, Effect.Temporary, true);
                        }
                        // 前の要素
                        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
                        {
                            ChangeEffect(_hitObj, Effect.Temporary);
                            _indexStageNum = (_indexStageNum - 1 < 0) ? _stageChildObjs.Count - 1 : --_indexStageNum;
                            _hitObj = _stageChildObjs[_indexStageNum].gameObject;
                            ChangeEffect(_hitObj, Effect.Temporary, true);
                        }

                        // オブジェクトの選択
                        else if (Input.GetKeyDown(KeyCode.Space))
                        {
                            if (_hitObj == null) return;
                            if (_selectedObj != null)
                            {
                                ChangeEffect(_selectedObj, Effect.Select);
                                _selectedObj = null;
                                _rotateObj.SetActive(false);
                                _buttonManager.gameObject.SetActive(false);
                                modeChanger.SelectedObject = null;
                                modeChanger.Mode = GucchiCS.ModeChanger.MODE.OBJECT_CONTROL;
                                return;
                            }
                            ChangeEffect(_hitObj, Effect.Temporary);
                            _selectedObj = _hitObj;
                            modeChanger.SelectedObject = _selectedObj;
                            modeChanger.Mode = GucchiCS.ModeChanger.MODE.OBJECT_CONTROL_SELECTED;
                            ChangeEffect(_selectedObj, Effect.Select, true);

                        }
                    }
                }

            });

        // プレイヤーが死んだら
        this.UpdateAsObservable()
            .Where(_ => _player.IsDead)
            .Subscribe(_ =>
            {
                ChangeEffect(_selectedObj, Effect.Select);
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
                ChangeEffect(_hitObj, Effect.Temporary);
                ChangeEffect(_selectedObj, Effect.Select);

                _buttonManager.gameObject.SetActive(false);
                _selectedObj = null;

                //回転軸の非表示
                _rotateObj.SetActive(false);
            });

        // ポーズ画面に行ったとき
        this.UpdateAsObservable()
            .Where(_ => _player.IsDead)
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
        ChangeEffect(_hitObj, Effect.Temporary);
        ChangeEffect(_selectedObj, Effect.Select);
        _hitObj = null;
        _selectedObj = null;
        _rotateObj.SetActive(false);
        GucchiCS.ModeChanger.Instance.SelectedObject = null;

    }

    /// <summary>
    /// エフェクトを変える
    /// </summary>
    /// <param name="obj">エフェクトを変えたいオブジェクト</param>
    /// <param name="effectNum">エフェクト番号</param>
    /// <param name="isOn">true:on / false:off</param>
    private void ChangeEffect(GameObject obj ,Effect effectNum = 0 , bool isOn = false)
    {
        // オブジェクトの実態がない
        if (!obj) return;
        switch(effectNum)
        {
            // blue
            case Effect.Temporary :
                obj.GetComponent<StageObject>().IsTemporary = isOn;
                break;
            // yellow
            case Effect.Select:
                obj.GetComponent<StageObject>().IsSelect = isOn;
                break;
            // other
            default:
                Debug.LogError("enumの値が異常です。");
                break;
        }
    }
}
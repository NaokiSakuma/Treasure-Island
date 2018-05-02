using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

public class RotateObjects : MonoBehaviour {

    // ボタン
    [SerializeField]
    protected Button _button = null;

    // アニメーションする時間
    protected float _animationTime = 1f;

    // 回転マネージャー
    [SerializeField]
    protected RotateManager _rotateManager = null;

    // 回転するgameObject
    protected GameObject _rotateObj = null;

    // 表示・非表示を切り替える
    protected bool _isChangeHide = false;

    // 親オブジェクト
    protected GameObject _parent = null;

    protected virtual void Awake()
    {
        _animationTime = _rotateManager.AnimationTime;
        _parent = _button.transform.parent.gameObject;
    }
    // Use this for initialization
    protected virtual void Start ()
    {
        // マウスのrayにhitしたオブジェクトを監視
        _rotateManager.ObserveEveryValueChanged(x => x.HitObj)
            .Subscribe(_ => _rotateObj = _rotateManager.HitObj);

        // ボタンが押された時の処理
        _button.onClick.AsObservable()
            .Where(_ => _rotateObj != null)
            .Where(_ => !_rotateManager.IsRotate)
            .Subscribe(_ =>
            {
                // 回転する
                _rotateManager.IsRotate = true;
                OnClickRotate();
            });

        // Zキーが押された時の処理
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Z))
            .Subscribe(_ => { _isChangeHide = !_isChangeHide;});

        // _isChangeHideを監視
        this.ObserveEveryValueChanged(x => x._isChangeHide)
            .Subscribe(_ => HideButton());

    }

    /// <summary>
    /// ボタンが押されて回転する
    /// </summary>
    protected virtual void OnClickRotate() { }

    /// <summary>
    /// 回転停止
    /// </summary>
    protected virtual void StopRotate()
    {
        _rotateManager.IsRotate = false;
    }

    /// <summary>
    /// ボタンを表示・非表示する
    /// </summary>
    private void HideButton()
    {
        _button.gameObject.SetActive(!_button.IsActive());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;
using DG.Tweening;

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
        _rotateManager.ObserveEveryValueChanged(x => x.SelectedObj)
            .Subscribe(_ => _rotateObj = _rotateManager.SelectedObj);

        // ボタンが押された時の処理
        _button.onClick.AsObservable()
            .Where(_ => CanRotate())
            .Subscribe(_ =>
            {
                RotateMethod();
            });
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

    /// <summary>
    /// オブジェクト回転の一連の流れ
    /// </summary>
    protected void RotateMethod()
    {
        // SEを鳴らす
        AudioManager.Instance.PlaySE(AUDIO.SE_OBJECTROTATE);

        // 回転する
        _rotateManager.IsRotate = true;

        // 回転している間、ボタンを大きくする
        Sequence seq = DOTween.Sequence()
            .Append(_button.GetComponent<RectTransform>().DOScale(new Vector3(1.3f, 1.3f, 1.3f), _animationTime * 0.9f))
            .Append(_button.GetComponent<RectTransform>().DOScale(new Vector3(1.0f, 1.0f, 1.3f), _animationTime * 0.1f));
        seq.Play();

        OnClickRotate();
    }

    protected bool CanRotate()
    {
        return _rotateObj != null && !_rotateManager.IsRotate;
    }
}

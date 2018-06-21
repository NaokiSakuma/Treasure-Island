using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPauseItem {
    /// <summary>
    /// マウスオーバーした時に呼ばれる
    /// </summary>
    void OnEnter();

    /// <summary>
    /// マウスが離れた時に呼ばれる
    /// </summary>
    void OnExit();

    /// <summary>
    /// クリックされた時に呼ばれる
    /// </summary>
    void OnClick();
}

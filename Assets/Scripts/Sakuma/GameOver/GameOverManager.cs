using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class GameOverManager : SingletonMonoBehaviour<GameOverManager> {

    // inspecter見にくいかも
    // オブジェクトが点滅する時間
    [SerializeField]
    private float _blinkTime = 0;

    // 点滅する回数
    [SerializeField]
    private int _blinkNum = 0;

    // 最後に光る時間
    [SerializeField]
    private float _lastRight = 0;

    // 点滅するオブジェクト
    [SerializeField]
    private List<GameObject> _blinkObjs = new List<GameObject>();

    // プレイヤー
    [SerializeField]
    private Konji.PlayerControl _player = null;

    // canvas
    [SerializeField]
    private GameObject _canvas = null;

    // ゲームオーバーUIのプレハブ
    [SerializeField]
    private GameObject _overPrefab = null;

    private 
	// Use this for initialization
	void Start () {

        // ライトの点滅
        this.UpdateAsObservable()
            .Where(_ => _player.IsDead)
            .Where(_ => GucchiCS.ModeChanger.Instance.Mode != GucchiCS.ModeChanger.MODE.CLEAR)
            .Take(1)
            .Subscribe(_ => BlinkCoroutine());
	}


    /// <summary>
    /// ライトの点滅
    /// </summary>
    IEnumerator Blink()
    {
        // _blinkNum回点滅する
        for (int i = 0; i < _blinkNum; i++)
        {
            yield return new WaitForSeconds(_blinkTime);
            foreach (var blinkObj in _blinkObjs)
            {
                if (blinkObj == null) continue;
                blinkObj.gameObject.SetActive(!blinkObj.gameObject.activeSelf);
            }
        }
        // 最後に長めに光らせる
        yield return new WaitForSeconds(_lastRight);
        foreach (var blinkObj in _blinkObjs)
        {
            if (blinkObj == null) continue;
            blinkObj.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ライトの点滅のコルーチン
    /// </summary>
    private void BlinkCoroutine()
    {
        Observable.FromCoroutine(Blink)
            .DelayFrame(10)
            .Subscribe(_ =>
            {
                // SEを鳴らす
                AudioManager.Instance.StopBGM();
                AudioManager.Instance.PlaySE(AUDIO.SE_GAMEOVER);

                GameObject prefab = (GameObject)Instantiate(_overPrefab);
                prefab.transform.SetParent(_canvas.transform, false);
            });
    }

    public void AddBlinkObject(GameObject obj)
    {
        _blinkObjs.Add(obj);
    }
}

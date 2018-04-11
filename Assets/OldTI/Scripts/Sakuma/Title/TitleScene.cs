using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour {

    // ボタン
    [SerializeField]
    private Button _button;
    // フェードのrange用
    [SerializeField]
    private FadeImage _fadeImage;
    // フェードする速度
    [SerializeField]
    private float _fadeSpeed = 0f;
	// Use this for initialization
	void Start () {
        AudioManager.Instance.PlayBGM(AUDIO.BGM_TITLEBGM);

        this._button.onClick.AsObservable()
            .First()
            .Subscribe(_ =>
            {
                //タイトルボタンSE
                AudioManager.Instance.PlaySE(AUDIO.SE_TITLEBUTTON);
                StartCoroutine(FadeOut());
            });
	}

    /// <summary>
    /// フェードアウト
    /// </summary>
    /// <returns>フェードアウトしきるまで再帰</returns>
    private IEnumerator FadeOut()
    {
        _fadeImage.Range += _fadeSpeed;
        if (_fadeImage.Range <= 1.0f)
        {
            yield return null;
            yield return StartCoroutine(FadeOut());
        }
        SceneManager.LoadScene("ProtoType");

        AudioManager.Instance.PlayBGM(AUDIO.BGM_PLAYBGM,0.5f);

        yield break;
    }

}

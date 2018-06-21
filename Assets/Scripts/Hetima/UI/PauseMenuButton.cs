using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuButton : MonoBehaviour {

    // ポーズ画像
    [SerializeField]
    private Sprite _pauseSprite;

    // 再生画像
    [SerializeField]
    private Sprite _playSprite;

    void Start(){
        var img = GetComponent<Image>();

        // クリックされた時の処理
        img.OnPointerClickAsObservable()
            .Where(_ => GucchiCS.StageManager.Instance.IsPlay)
            .Where(_ => !GucchiCS.ModeChanger.Instance.IsChanging)
            .Subscribe(_ => {
                // SEを鳴らす
                AudioManager.Instance.PlaySE(AUDIO.SE_POSE);

                Pausable.Instance.pausing = !Pausable.Instance.pausing;
            });

                // 一時停止中と再生中で画像を変更する
                this.ObserveEveryValueChanged(x => Pausable.Instance.pausing)
            .Subscribe(isPause => {
                img.sprite = isPause ? _playSprite : _pauseSprite;
            });
    }
}

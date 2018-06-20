using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : SingletonMonoBehaviour<FadeManager>
{

    //円エフェクト
    [SerializeField]
    GameObject circleFade;

    //イン・アウト含めたフェードの種類  
    private enum FadeKindAll
    {
        LightIn,
        LightOut,
        CircleIn,
        CircleOut,
    };

    //フェードの種類
    public enum FadeKind
    {
        Light,
        Circle
    }

    //最後のフェードアウト
    private FadeKind lastFadeout = FadeKind.Light;

    float fadeTime;
    public float FadeTime
    {
        get {return fadeTime; }
    }


    //フェードイン
    public void InPlay()
    {
        switch(lastFadeout)
        {
            case FadeKind.Light:
                Play(FadeKindAll.LightIn);
                break;
            case FadeKind.Circle:
                Play(FadeKindAll.CircleIn);
                break;
        }
    }

    //フェードアウト
    public void OutPlay(FadeKind fade)
    {
        if(fade == FadeKind.Light)
        {
            Play(FadeKindAll.LightOut);
        }

        if (fade == FadeKind.Circle)
        {
            Play(FadeKindAll.CircleOut);
        }
        lastFadeout = fade;
    }

    /// <summary>
    /// フェードを実行する
    /// </summary>
    /// <param name="fadeKind"></param>
    private void Play(FadeKindAll fadeKind)
    {
        switch (fadeKind)
        {
            case FadeKindAll.LightIn:
                LightFadeIn.Instance.Play();
                fadeTime = LightFadeIn.Instance.FadeTime;
                break;
            case FadeKindAll.LightOut:
                LightFadeIn.Instance.Play();
                fadeTime = LightFadeIn.Instance.FadeTime;
                break;
            case FadeKindAll.CircleIn:
                CirecleFade.Instance.Play(CirecleFade.FadeMode.In);
                fadeTime = CirecleFade.Instance.FadeTime;
                break;
            case FadeKindAll.CircleOut:
                CirecleFade.Instance.Play(CirecleFade.FadeMode.Out);
                fadeTime = CirecleFade.Instance.FadeTime;
                break;
            default:
                break;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : SingletonMonoBehaviour<FadeManager>
{

    [SerializeField]
    GameObject circle;

    [SerializeField]
    GameObject lightFade;

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
    private static FadeKind lastFadeout;
    public FadeKind LastFadeout
    {
        set { lastFadeout = value; }
    }


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
                //Instantiate(lightFade, null);
                //lightFade.GetComponentInChildren<LightFadeIn>().Play();
                //Debug.Log(lightFade.GetComponentInChildren<LightFadeIn>().gameObject.name);
                //fadeTime = lightFade.GetComponentInChildren<LightFadeIn>().FadeTime; break;
            case FadeKindAll.CircleIn:
                Instantiate(circle);
                circle.GetComponentInChildren<CirecleFade>().Play(CirecleFade.FadeMode.In);
                fadeTime = circle.GetComponentInChildren<CirecleFade>().FadeTime;
                break;
            case FadeKindAll.CircleOut:
                Instantiate(circle);
                circle.GetComponentInChildren<CirecleFade>().Play(CirecleFade.FadeMode.Out);
                fadeTime = circle.GetComponentInChildren<CirecleFade>().FadeTime;
                break;
            default:
                Debug.Log("条件外");
                break;
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : SingletonMonoBehaviour<FadeManager>
{
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
    private FadeKind lastFadeout;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
                break;
            case FadeKindAll.LightOut:
                break;
            case FadeKindAll.CircleIn:
                CirecleFade.Instance.Play(CirecleFade.FadeMode.In);
                break;
            case FadeKindAll.CircleOut:
                CirecleFade.Instance.Play(CirecleFade.FadeMode.Out);
                break;
            default:
                break;
        }
    }

}

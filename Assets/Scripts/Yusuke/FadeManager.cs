using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : SingletonMonoBehaviour<FadeManager>
{
    public enum FadeKind
    {
        Light,
        Circle
    }

    //フェード
    private FadeKind fade;
    public FadeKind Fade
    {
        set { fade = value; }
        get { return fade; }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void Play()
    {
        if(fade == FadeKind.Light)
        {
            LightFadeIn.Instance.Play();
        }
        else
        {
            CirecleFade.Instance.Play(CirecleFade.FadeMode.In);
        }
    }

}

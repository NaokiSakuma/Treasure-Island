using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class MiniMapSlide : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start()
    {
        this.ObserveEveryValueChanged(_ => GucchiCS.ModeChanger.Instance.Mode)
            .Where(mode => mode != GucchiCS.ModeChanger.MODE.GAME)
            .Subscribe(_ =>
            {
                animator.SetBool("Show", true);
            });

        this.ObserveEveryValueChanged(_ => GucchiCS.ModeChanger.Instance.Mode)
            .Where(mode => mode == GucchiCS.ModeChanger.MODE.GAME)
            .Subscribe(_ =>
            {
                animator.SetBool("Show", false);
            });

    }
}

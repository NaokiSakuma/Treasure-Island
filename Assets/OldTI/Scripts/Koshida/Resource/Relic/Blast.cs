using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Blast : MonoBehaviour
{
    void Start()
    {
        ParticleSystem particle = this.GetComponent<ParticleSystem>();

        particle.Play();

        this.UpdateAsObservable()
            .Where(_ => !particle.IsAlive())
            .Subscribe(_ => Destroy(this.gameObject));
    }
}

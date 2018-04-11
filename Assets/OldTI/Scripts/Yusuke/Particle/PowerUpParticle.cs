using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpParticle : MonoBehaviour {

   public ParticleSystemRenderer powerUpParticle;
    int framecnt = 0;
    bool isCntUp;
    [SerializeField]  float speed;

	// Use this for initialization
	void Start () {
        powerUpParticle = Instantiate(powerUpParticle);
        isCntUp = true;
  
    }
    void Update()
    {
        //進行度
        float time = (float)framecnt / speed;
        //大きさを補完
        float size = Mathf.Lerp(powerUpParticle.minParticleSize, powerUpParticle.maxParticleSize, time);

        //大きさを設定    
        powerUpParticle.transform.localScale = new Vector3(size, size, size);

    
        if(isCntUp)
        {
            framecnt++;
        }
        else
        {
            framecnt--;
        }



        if (time >= 1.0f)
        {
            isCntUp = false;
        }
        if (time <= 0.0f)
        {
            isCntUp = true;
        }
    }

    public void Play(Vector3 particlePosition)
    {
        powerUpParticle.GetComponent<ParticleSystem>().transform.localPosition = particlePosition;
        powerUpParticle.GetComponent<ParticleSystem>().Play();
        powerUpParticle.GetComponent<ParticleSystem>().gameObject.SetActive(true);
    }

    public void Stop()
    {
        powerUpParticle.GetComponent<ParticleSystem>().gameObject.SetActive(false);
        powerUpParticle.GetComponent<ParticleSystem>().Stop();

    }

}

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
        // ここで Particle System を停止する.
        powerUpParticle.GetComponent<ParticleSystem>(). Play();
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
}

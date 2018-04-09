using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OccurrenceRelicParticle : MonoBehaviour {
    //ユニット
    UnitCore _unitCore;
    //前回の攻撃バフ
    int _oldAdditionalStrength;
    //インク発生オブジェクト
    [SerializeField]
    PowerUpParticle powerUpParticle;

    public static OccurrenceRelicParticle MyInstantiate(OccurrenceRelicParticle occurrenceRelicParticle , UnitCore unitCore)
    {
       OccurrenceRelicParticle occurrenceRelicParticle_ =  Instantiate(occurrenceRelicParticle);
        occurrenceRelicParticle_._unitCore = unitCore;
        return occurrenceRelicParticle_;
    }

    // Use this for initialization
    void Start () {
       // _unitCore = this.transform.parent.gameObject.GetComponent<UnitCore>();
        powerUpParticle = Instantiate(powerUpParticle);
    }
	
	// Update is called once per frame
	void Update () {

     
        if(_unitCore.AdditionalStrength != _oldAdditionalStrength)
        {
            if(_unitCore.AdditionalStrength == 0)
            {
                powerUpParticle.Stop();
                return;
            }
            powerUpParticle.Play(_unitCore.transform.position);
            _oldAdditionalStrength = _unitCore.AdditionalStrength;
        }
    }
}

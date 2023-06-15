using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassAnimation1 : MonoBehaviour
{
    public Vector3 amount;
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        float timeValue = 0.3f;
        float amountValue = 0.3f;
        float randomTime = Random.Range(time - timeValue, time + timeValue);
        float randomAmount = Random.Range(amount.z - amountValue, amount.z + amountValue);
        iTween.PunchScale(gameObject, 
        iTween.Hash(
            "amount", amount,
            "time", randomTime,
            "looptype", iTween.LoopType.loop
        ));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

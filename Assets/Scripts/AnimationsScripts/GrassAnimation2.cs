using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassAnimation2 : MonoBehaviour
{
    public Vector3 amount;
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        iTween.ShakeScale(gameObject, 
        iTween.Hash(
            "amount", amount,
            "time", time,
            "looptype", iTween.LoopType.loop
        ));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

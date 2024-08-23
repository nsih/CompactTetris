using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCon : MonoBehaviour
{
    public float ownTime;
    float timerTime;


    void OnEnable()
    {
        timerTime = ownTime;
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-100, 100), Random.Range(50, 400)));
    }

    void Update()
    {
        CoolTimer();
    }


    void CoolTimer()
    {
        if (timerTime <= 0)
            this.gameObject.SetActive(false);

        else
            timerTime -= Time.deltaTime;
    }
}

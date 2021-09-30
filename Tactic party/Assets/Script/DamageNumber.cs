using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    private float floatSpeed = 0.75f;
    private float slideSpeed = 0.25f;

    void Start()
    {
        Destroy(gameObject, 0.75f);
    }

    void Update()
    {
        transform.position += transform.up * floatSpeed * Time.deltaTime;
        
        if(transform.parent.gameObject.tag == "PlayerUnit")
        {
            transform.position += -transform.right * slideSpeed * Time.deltaTime;
        }
        if(transform.parent.gameObject.tag == "MonsterUnit")
        {
            transform.position += transform.right * slideSpeed * Time.deltaTime;
        }
    }
}

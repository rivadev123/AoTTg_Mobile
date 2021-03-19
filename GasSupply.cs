using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasSupply : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player"))
        {
            col.GetComponent<Player>().Refill();
        }
    }
}

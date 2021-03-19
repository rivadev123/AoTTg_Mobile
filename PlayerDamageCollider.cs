using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageCollider : MonoBehaviour
{
    public string ObjectTagName = "";
    public LayerMask layer;

    public bool Paralyze;
    public float ParalyzeTime;

    public bool Slowness;
    public bool Fire;
    public bool Poison;


    public float Damage;

    public bool Obstruction;
    public bool HasDamagedSomething;
    public GameObject Object;

    
    private void OnDisable()
    {
        HasDamagedSomething = false;

    }
    private void OnTriggerEnter(Collider col)
    {
        
        if (col.CompareTag(ObjectTagName))
        {
            col.GetComponent<EnemyHealth>().GetDamage(Damage);
            Debug.Log("hit");
            HasDamagedSomething = true;
          
        }


    }



}

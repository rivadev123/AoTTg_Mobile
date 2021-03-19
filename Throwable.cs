using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    public bool StaticThrowable;

    public float TimeToDestroy;
    public float BouncesToDestroy;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyObject", TimeToDestroy);
    }
    private void Update()
    {
        if (BouncesToDestroy <= 0f && !StaticThrowable)
        {
            DestroyObject();
        }
    }
    // Update is called once per frame
    private void OnCollisionEnter(Collision col)
    {
        BouncesToDestroy--;
    }
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}

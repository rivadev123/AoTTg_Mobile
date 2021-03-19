using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;

    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("spawn", 1f, timer);
    }

    // Update is called once per frame
    void spawn()
    {
        Instantiate(prefab);
    }
}

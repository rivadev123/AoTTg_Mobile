using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public bool TimeIsStopped;
    
    // Start is called before the first frame update
    public void Freeze()
    {
        TimeIsStopped = true;
    }

    // Update is called once per frame
    public void Unfreeze()
    {
        TimeIsStopped = false;
    }
}

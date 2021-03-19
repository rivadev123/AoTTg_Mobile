using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public float Health;

    public bool OnHealthDecrease;

    // Start is called before the first frame update
    public void GetDamage(float DMG)
    {
        Health -= DMG;
        OnHealthDecrease = true;
        InvokeNextFrame(nextframe);
    }

    // Update is called once per frame
    void nextframe()
    {
        OnHealthDecrease = false;

    }

    public delegate void Function();

    public void InvokeNextFrame(Function function)
    {
        try
        {
            StartCoroutine(_InvokeNextFrame(function));
        }
        catch
        {
            Debug.Log("Trying to invoke " + function.ToString() + " but it doesnt seem to exist");
        }
    }

    IEnumerator _InvokeNextFrame(Function function)
    {
        yield return null;
        function();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChange : MonoBehaviour
{
    public GameObject Old;
    public GameObject New;
    // Start is called before the first frame update
    public void Change()
    {
        Old.SetActive(false);
        New.SetActive(true);
    }

}

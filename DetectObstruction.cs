using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectObstruction : MonoBehaviour {
	public string ObjectTagName = "";

	public bool Obstruction;
    public GameObject Object;
    
	void OnTriggerStay(Collider col)
	{
		if (ObjectTagName != "") {
			if (col != null && !col.isTrigger && col.CompareTag (ObjectTagName)) {
				Obstruction = true;
                Object = col.gameObject;
			}
		} 
		
	
	}





    private void Update()
    {
        if(Object == null)
        {
            Obstruction = false;
        }
    }



    void OnTriggerExit()
	{

		Obstruction = false;

    }

}

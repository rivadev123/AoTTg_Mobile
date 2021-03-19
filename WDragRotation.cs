using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
public class WDragRotation: MonoBehaviour {

	
	[Header("Loc")]

	public float amount = 0.02f;
    public float amountZ = 0.02f;

	public float Smooth = 3;

	private Vector3 def;
	private RigidbodyFirstPersonController fpscontroller;
	void  Start (){
		def = transform.localPosition;
		fpscontroller = GetComponentInParent<RigidbodyFirstPersonController> ();
	}

	void  Update (){
	

        //loc 

        var MouseX = ControlFreak2.CF2Input.GetAxis("Mouse X") * amount;
        var MouseY = ControlFreak2.CF2Input.GetAxis("Mouse Y") * amount;

        float factorX = (-fpscontroller.relativeMovement.x * amount)-MouseX;
		float factorY = (-fpscontroller.relativeMovement.y * amount)-MouseY;
        float factorZ = (-fpscontroller.relativeMovement.z * amountZ);

     


		Vector3 Final = new Vector3(def.x+factorX, def.y+factorY, def.z + factorZ);
		transform.localPosition = Vector3.Lerp(transform.localPosition, Final, Time.deltaTime * Smooth);




	}
}

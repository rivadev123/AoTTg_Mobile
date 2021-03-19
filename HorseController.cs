using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseController : MonoBehaviour
{
    public Rigidbody rb;

    public float speed;

    public bool HorseActivate;

    public GameObject PlayerRefPos;

    public Animation anim;

    public GameObject GFX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        if(rb.velocity.magnitude > 2f)
        {
            Quaternion rot = Quaternion.LookRotation(rb.velocity);
            Quaternion newrot = Quaternion.Euler(0, rot.eulerAngles.y, 0);
            GFX.transform.rotation = Quaternion.Lerp(GFX.transform.rotation, newrot, Time.deltaTime * 5f);
            anim.Play("horse_WALK");
        }
        else
        {
            anim.Play("horse_idle");

        }

        Vector2 input = GetInput();



        float h = input.x;
        float v = input.y;
        Vector3 inputVector = new Vector3(h, 0, v);
        inputVector = Vector3.ClampMagnitude(inputVector, 1);


        if (ControlFreak2.CF2Input.GetAxisRaw("Vertical") > 0.3f)
        {
            rb.AddRelativeForce(0, 0, Time.deltaTime * speed* Mathf.Abs(inputVector.z));
        }
        if (ControlFreak2.CF2Input.GetAxisRaw("Vertical") < -0.3f)
        {
            rb.AddRelativeForce(0, 0, Time.deltaTime * -speed * Mathf.Abs(inputVector.z));
        }
        if (ControlFreak2.CF2Input.GetAxisRaw("Horizontal") > 0.5f)
        {
            rb.AddRelativeForce(Time.deltaTime * speed * Mathf.Abs(inputVector.x), 0, 0);
        }
        if (ControlFreak2.CF2Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            rb.AddRelativeForce(Time.deltaTime * -speed * Mathf.Abs(inputVector.x), 0, 0);
        }
    }


    private Vector2 GetInput()
    {

        Vector2 input = new Vector2

        {
            x = ControlFreak2.CF2Input.GetAxisRaw("Horizontal"),
            y = ControlFreak2.CF2Input.GetAxisRaw("Vertical")
        };

        return input;
    }

}

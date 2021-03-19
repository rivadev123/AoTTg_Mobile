using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using EZCameraShake;
public class TitanEren : MonoBehaviour
{
    public Animation anim;
    public RigidbodyFirstPersonController rbfps;
    public Rigidbody rb;
    public GameObject NapeLocation;
    public Collider[] HeadandNape;

    public GameObject GFX;
    public Vector3 offset;

    public CameraShaker camshake;

    public float speed;
    public float dragGround;
    public float dragAir;

    public float JumpForce;
    public float Gravity;

    public bool Alive;
    public bool Died;


    public bool isAction;

    public DetectObstruction TitanDetectLow;
    public DetectObstruction TitanDetectMedium;

    public GameObject LowAttackCollider;
    public GameObject HighAttackCollider;

    public GameObject PlayerObject;
    // Start is called before the first frame update
    void Start()
    {
        anim.enabled = false;
        anim.enabled = true;

        anim.wrapMode = WrapMode.Loop;
        PlayAnimation("born");
        Invoke("Borned", 3f);
        Invoke("Die", 20f);

        AudioManager.instance.Play("ErenRoar");
    }

    void Die()
    {
        Died = true;
        Alive = false;
        PlayAnimationClamp("die");
        Invoke("DieReal", 3f);
    }
    void DieReal()
    {
        foreach (Collider c in HeadandNape)
        {
            c.enabled = false;
        }
        PlayerObject.SetActive(true);
        PlayerObject.GetComponentInChildren<RigidbodyFirstPersonController>().transform.position = NapeLocation.transform.position + transform.up * 5f;
        PlayerObject.GetComponentInChildren<Rigidbody>().velocity = transform.up * 5f;
        Destroy(gameObject.transform.parent.gameObject);
    }
    void Borned()
    {
        Alive = true;
    }
    void ColliderHighActivate(float start, float end)
    {
        Invoke("ActivateHigh", start);
        Invoke("DeactivateHigh", end);
    }
    void ActivateHigh()
    {
        HighAttackCollider.SetActive(true);
    }
    void DeactivateHigh()
    {
        HighAttackCollider.SetActive(false);
    }
    void ColliderLowActivate(float start, float end)
    {
        Invoke("ActivateLow", start);
        Invoke("DeactivateLow", end);
    }
    void ActivateLow()
    {
        LowAttackCollider.SetActive(true);
    }
    void DeactivateLow()
    {
        LowAttackCollider.SetActive(false);
    }


    void Action(float time)
    {
        isAction = true;
        CancelInvoke("StopAction");
        Invoke("StopAction", time);
    }
    void StopAction()
    {
        isAction = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!Alive && !Died)
        {
            camshake.ShakeOnce(1f, 10f, 0f, 0.5f);
        }
        if (Alive)
        {
            if (rb.velocity.magnitude > 20f && rbfps.Grounded)
            {
                anim.CrossFade("run");
            }
            else
            {
                anim.CrossFade("idle");
            }
            if (!rbfps.Grounded)
            {
                anim.CrossFade("jump_air");
            }

            rb.drag = dragGround;
            if (!rbfps.Grounded)
            {
                rb.drag = dragAir;
            }

            Vector2 input = new Vector2
            {
                x = ControlFreak2.CF2Input.GetAxisRaw("Horizontal"),
                y = ControlFreak2.CF2Input.GetAxisRaw("Vertical")
            };

            rb.AddForce(0, -Gravity, 0);
            if (rbfps.Grounded && ControlFreak2.CF2Input.GetKeyDown(KeyCode.Space) && !isAction)
            {         
                rb.velocity = new Vector3(rb.velocity.x, JumpForce, rb.velocity.z);
            }
            if (!isAction && rbfps.Grounded)
            {
                rb.AddRelativeForce(input.x * speed, 0, input.y * speed);
            }
            GFX.transform.position = transform.position + offset;

            if (rb.velocity.magnitude > 20f)
            {
                Quaternion rot = Quaternion.LookRotation(rb.velocity);
                Quaternion newrot = Quaternion.Euler(0, rot.eulerAngles.y, 0);
                GFX.transform.rotation = Quaternion.Lerp(GFX.transform.rotation, newrot, Time.deltaTime * 5f);
            }


            //attacks
            if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.F) && !isAction && rbfps.Grounded)
            {
                if (TitanDetectLow.Obstruction)
                {             
                    GFX.transform.rotation = transform.rotation;    
                    PlayAnimation("attack_kick");
                    ColliderLowActivate(0.6f, 1f);
                    Invoke("Camshake", 0.6f);

                    Action(2.4f);
                }

                if (TitanDetectMedium.Obstruction)
                {
                    GFX.transform.rotation = transform.rotation;
                    int i = Random.Range(0, 3);
                    if (i == 0)
                    {
                        PlayAnimation("attack_combo_001");
                        Action(1.3f);
                        ColliderHighActivate(0.2f, 0.6f);
                        Invoke("Camshake", 0.2f);
                    }
                    if (i == 1)
                    {
                        PlayAnimation("attack_combo_002");
                        Action(1.3f);
                        ColliderHighActivate(0.1f, 0.6f);
                        Invoke("Camshake", 0.1f);

                    }
                    if (i == 2)
                    {
                        PlayAnimation("attack_combo_003");
                        Action(1.3f);
                        ColliderHighActivate(0.4f, 0.6f);
                        Invoke("Camshake", 0.4f);

                    }
                }
               
            }
        }
    }

    void Camshake()
    {
        camshake.ShakeOnce(2f, 10f, 0f, 1f);
    }
    void PlayAnimation(string Animname)
    {
        anim[Animname].layer = 1;

        anim[Animname].wrapMode = WrapMode.Once;
        anim.CrossFade(Animname, 0.1f);


    }
    void PlayAnimationClamp(string Animname)
    {
        anim[Animname].layer = 1;

        anim[Animname].wrapMode = WrapMode.ClampForever;
        anim.CrossFade(Animname, 0.1f);


    }
}

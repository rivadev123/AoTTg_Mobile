using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NormalTitan : MonoBehaviour
{
    public bool IsNormalTitan;
    public bool IsAbnormalTitan;
    public float Speed;
    public float RotSpeed;
    [Header("Headrotation")]

    public GameObject head;

    public Vector3 offsetrotation;
    Quaternion newrot;
    Quaternion newrotlocal;
    public DetectObstruction Nape;
    public DetectObstruction Eyes;

    public DetectObstruction LegL;
    public DetectObstruction LegR;

    public DetectObstruction PlayerDetect;
    public DetectObstruction PlayerDetectInFrontHead;

    public DetectObstruction GroundDetect;

    [Header("NormalTitan")]
    public DetectObstruction AttackLeftDetect;
    public DetectObstruction AttackRightDetect;

    public DetectObstruction AttackGroundLeftDetect;
    public DetectObstruction AttackGroundRightDetect;

    public DetectObstruction NapeGrabDetect;

    [Header("Abnormal Titan")]
    public DetectObstruction WallDetect;
    public DetectObstruction JumpAttackDetect;
    public float AbnormalAttackDelay;
    private float timer;

    public bool Alert;
    public bool isSit;
    public bool IsDead;
    public bool EyeHit;
    public bool IsAction;
    public bool Move;
    public bool IsFall;

    public bool Frozen;
    public bool Died;

    private float Move_Speed;


    private Rigidbody rb;
    private Animation anim;
    private GameObject target;
    private LevelManager lvlmanager;
    [Header ("eat")]
    public bool IsEatLeft;
    public bool IsEatRight;

    public Transform eatlocL;
    public Transform eatlocR;

    private float timer_eat;
    private Transform GrabbedPerson;
    private Transform RotGFX;

    [Header("BodyParts")]
    public bool IsAttackLeftArm;
    public bool IsAttackRightArm;
    public bool IsGrabLeftArm;
    public bool IsGrabRightArm;
    

    public GameObject[] LeftArm;
    public GameObject[] RightArm;
    public GameObject[] WholeBody;

    public DetectObstruction LeftHandGrab;
    public DetectObstruction RightHandGrab;

    public GameObject[] Feet;

    public Collider[] HeadAndNape;

    [Header("Details")]
    public GameObject[] Hair;
    public MeshRenderer eye;


    [Header("particles")]

    public GameObject Deathsmoke;


    private TimeManager timemanager;
    private GameObject ErenTitanTemp;
    // Start is called before the first frame update
    void Start()
    {
        lvlmanager = FindObjectOfType<LevelManager>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animation>();
        timemanager = FindObjectOfType<TimeManager>();

        int r = Random.Range(0, 2);
        if (r == 1)
        {
            isSit = true;
        }
        else
        {
            isSit = false;
        }
        // ChooseHair();
        lvlmanager.TitanSpawn();
        ChooseEye();
    }
    void ChooseHair()
    {
        int random = Random.Range(0, Hair.Length);
        Hair[random].SetActive(true);
    }
    void ChooseEye()
    {
        int random = Random.Range(0, 8);

        eye.material.SetTextureOffset("_MainTex", new Vector2(0,(float)random * 0.125f));
    }

    void AttackBody(float time)
    {
        CancelInvoke("AttackBody2");
        Invoke("AttackBody2", time);
    }
    void AttackBody2()
    {

        foreach (GameObject g in WholeBody)
        {
            g.tag = "HurtCollider";
        }
        IsAttackLeftArm = true;
    }
    void AttackLeftArm(float time)
    {
        CancelInvoke("AttackLeftArm2");
        Invoke("AttackLeftArm2", time);
    }
    void AttackLeftArm2()
    {
      
        foreach (GameObject g in LeftArm)
        {
            g.tag = "HurtCollider";
        }
        IsAttackLeftArm = true;
    }
    void AttackRightArm(float time)
    {
        CancelInvoke("AttackRightArm2");
        Invoke("AttackRightArm2", time);
    }
    void AttackRightArm2()
    {

        foreach (GameObject g in RightArm)
        {
            g.tag = "HurtCollider";
        }
        IsAttackRightArm = true;
    }

    void AttackEnd(float time)
    {
        CancelInvoke("StopAttack");
        Invoke("StopAttack", time);
    }
    void StopAttack()
    {
        IsAttackLeftArm = false;
        IsAttackRightArm = false;
        IsGrabLeftArm = false;
        IsGrabRightArm = false;
        IsEatLeft = false;
        IsEatRight = false;

        foreach (GameObject g in LeftArm)
        {
            g.tag = "Untagged";
        }
        foreach (GameObject g in RightArm)
        {
            g.tag = "Untagged";
        }
        foreach (GameObject g in WholeBody)
        {
            g.tag = "Untagged";
        }
    }
    void Action(float time)
    {
        IsAction = true;
        CancelInvoke("StopAction");

        Invoke("StopAction", time);
    }
    void StopAction()
    {
        if (IsDead)
            return;

        EyeHit = false; 
        IsAction = false;
        IsFall = false;
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
    void PlayAnimationClampSpeed(string Animname, float speed)
    {
        anim[Animname].layer = 1;
        anim[Animname].speed = speed;

        anim[Animname].wrapMode = WrapMode.ClampForever;
        anim.CrossFade(Animname, 0.1f);


    }

    private void MoveNow(float m, float time)
    {
        Move = true;
        Move_Speed = m;
        Invoke("StopMove", time);
    }

   
    void StopMove()
    {
        Move = false;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (Frozen)
            return;

        if (PlayerDetectInFrontHead.Obstruction && !IsAction)
        {
            Vector3 direction = (target.transform.position - head.transform.position);
            Quaternion euleroffset = Quaternion.Euler(offsetrotation);

            head.transform.rotation = Quaternion.Lerp(newrot, Quaternion.LookRotation(direction) * euleroffset, Time.deltaTime * 2f);
            newrot = head.transform.rotation;
            newrotlocal = head.transform.localRotation;

        }
        else
        {
           
                head.transform.localRotation = Quaternion.Lerp(newrotlocal, Quaternion.Euler(0, 0, 0), Time.deltaTime * 2f);
                newrot = head.transform.rotation;
                newrotlocal = head.transform.localRotation;
            
        }
        if (IsEatLeft)
        {
            GrabbedPerson.transform.position = eatlocL.transform.position;
            RotGFX.transform.rotation = eatlocL.transform.rotation;

        }

        if (IsEatRight)
        {
            GrabbedPerson.transform.position = eatlocR.transform.position;
            RotGFX.transform.rotation = eatlocR.transform.rotation;

        }
    }
  

    void Update()
    {
        if(Died && !IsDead)
        {
            Die();
        }
      if(timemanager.TimeIsStopped && !Frozen)
        {
            Frozen = true;
            StopAttack();
            StopAction();
        }
        if (!timemanager.TimeIsStopped && Frozen)
        {
            Frozen = false;
        }

        anim.enabled = true;
        rb.isKinematic = false;
        if(Frozen)
        {      
            rb.isKinematic = true;
            anim.enabled = false;
        }



        //hit eye
        if (Eyes.Obstruction && !EyeHit && !IsDead && !IsFall)
        {
            if (Eyes.Obstruction)
            {
                CheckKill(Eyes, Eyes.gameObject);
            }

            EyeHit = true;
            PlayAnimation("hit_eye");
            Action(2f);

            Invoke("Attack_Combo", 2f);
        }

        //hitnape
        if (Nape.Obstruction && !IsDead)
        {

            IsDead = true;
            Die();
            CheckKill(Nape, Nape.gameObject);            //if player used blade


        }

        //hitleg
        if ((LegL.Obstruction || LegR.Obstruction) && !IsDead && !IsFall)
        {
            //if player used blade
            if (LegL.Obstruction)
            {
                CheckKill(LegL, LegL.gameObject);
            }
            if (LegR.Obstruction)
            {
                if (LegR.Obstruction)
                {
                    CheckKill(LegR, LegR.gameObject);
                }
            }
            IsFall = true;
            PlayAnimationClamp("sit_hunt_down");
            Action(5f);
            Invoke("Stand", 5f);
        }




        if (Frozen)
            return;


        if (IsDead)
        {
            IsAction = true;
        }
        //walk
        if (!IsDead)
        {
            if (rb.velocity.magnitude > 1f)
            {
                if (IsNormalTitan)
                {
                    anim.CrossFade("run_walk");
                }
                if (IsAbnormalTitan)
                {
                    anim.CrossFade("run_abnormal");
                }
            }
            else
            {
                if (isSit)
                {
                    anim.CrossFade("sit_down");
                }
                else
                {
                    anim.CrossFade("idle");
                }
            }
        }

        if(!IsAction && Alert)
        {
            Vector3 dir = (target.transform.position - transform.position);
            Quaternion rot = Quaternion.LookRotation(dir);
            Quaternion newrot = Quaternion.Euler(0, rot.eulerAngles.y, 0);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, newrot, Time.deltaTime * RotSpeed);
        }

        

        //standup if see player
        if (PlayerDetect.Obstruction)
        {
            target = PlayerDetect.Object;
            if (!Alert)
            {
                Alert = true;
                if (isSit)
                {
                    Stand();
                }
            }
        }
 
        //movement
      
        if(!isSit && !IsAction && Alert)
        {
            rb.velocity = transform.forward * Speed * transform.localScale.x;
        }
        if(Move)
        {
            if (IsNormalTitan)
            {
                rb.velocity = transform.forward * Move_Speed * transform.localScale.x;
            }
            if(IsAbnormalTitan)
            {
                if (!WallDetect.Obstruction)
                {
                    rb.velocity = transform.forward * Move_Speed * transform.localScale.x;
                }
                else
                {
                    rb.velocity = Vector3.zero;
                }
            }
        }
        if (!GroundDetect.Obstruction)
        {
            rb.velocity += transform.up * -10f;
        }

        AbnormalAttackDelay -= Time.deltaTime;
        //attacks
        if (!IsAction)
        {
            if (IsNormalTitan)
            {
                if (AttackLeftDetect.Obstruction)
                {
                    PlayAnimation("attack_anti_AE_l");
                    Action(4f);
                    AttackEnd(2f);
                    AttackLeftArm(0.5f);

                }
                if (AttackRightDetect.Obstruction)
                {
                    PlayAnimation("attack_anti_AE_r");
                    Action(4f);
                    AttackEnd(2f);

                    AttackRightArm(0.5f);
                }

                if (AttackGroundLeftDetect.Obstruction)
                {
                    AttackEnd(1.5f);

                    PlayAnimation("grab_ground_front_l");
                    Action(4f);
                    IsGrabLeftArm = true;

                }
                if (AttackGroundRightDetect.Obstruction)
                {
                    AttackEnd(1.5f);

                    PlayAnimation("grab_ground_front_r");
                    Action(4f);
                    IsGrabRightArm = true;

                }
                if (NapeGrabDetect.Obstruction)
                {
                    AttackEnd(2f);

                    PlayAnimation("grab_head_back_l");
                    Action(3f);
                    IsGrabRightArm = true;

                }
            }
            if (IsAbnormalTitan)
            {
                if (JumpAttackDetect.Obstruction && AbnormalAttackDelay < 0f)
                {
                    PlayAnimationClamp("attack_abnormal_jump");
                    Action(8f);
                    AttackEnd(1f);
                    AttackBody(0.3f);
                    MoveNow(10f, 1.1f);

                    IsFall = true;
                    Invoke("GetUp", 7f);
                }
            }
        }

        if(IsGrabLeftArm && LeftHandGrab.Obstruction && !IsEatLeft && !IsFall)
        {
            GrabbedPerson = LeftHandGrab.Object.transform;
            GrabbedPerson.GetComponent<Player>().IsGrabbed = true;
            RotGFX = GrabbedPerson.GetComponent<Player>().PlayerGFXRot;

            Invoke("KillPlayer",4.3f);

            IsGrabLeftArm = false;
            PlayAnimation("eat_l");
            AttackEnd(10f);
            Action(10f);
            IsEatLeft = true;
        }
        if (IsGrabRightArm && RightHandGrab.Obstruction && !IsEatRight)
        {
            GrabbedPerson = RightHandGrab.Object.transform;
            GrabbedPerson.GetComponent<Player>().IsGrabbed = true;
            RotGFX = GrabbedPerson.GetComponent<Player>().PlayerGFXRot;
            Invoke("KillPlayer", 4.3f);
            IsGrabRightArm = false;
            PlayAnimation("eat_r");
            AttackEnd(10f);
            Action(10f);
            IsEatRight = true;
        }


    }

    void Attack_Combo()
    {
        PlayAnimation("attack_combo");
        Action(5f);

        AttackEnd(2.5f);
        MoveNow(10f, 2.5f);
        AttackLeftArm(0.5f);
        AttackRightArm(0.5f);


    }
    void Die()
    {
        lvlmanager.TitanDie();
        if (IsNormalTitan)
        {
            PlayAnimationClamp("die_front");
        }
        if(IsAbnormalTitan)
        {
            foreach(Collider c in HeadAndNape)
            {
                c.enabled = false;
            }
            if (!IsFall)
            {
                PlayAnimationClamp("die_front");
            }
            else
            {
                PlayAnimationClamp("die_ground");
            }

        }
        IsAttackLeftArm = false;
        IsAttackRightArm = false;
        IsGrabLeftArm = false;
        IsGrabRightArm = false;
        IsEatLeft = false;
        IsEatRight = false;
        CancelInvoke();
        Invoke("DestroyObject", 10f);
    }

    void DestroyObject()
    {
        if(GetComponentInChildren<hitpoint>())
        {
            GetComponentInChildren<hitpoint>().player.DetachHooks();
        }

       GameObject g = Instantiate(Deathsmoke);
        g.transform.position = Nape.transform.position;


        Destroy(gameObject);
    }
    void KillPlayer()
    {
        if(GrabbedPerson != null)
        {
            GrabbedPerson.GetComponent<Player>().IsDead = true;
        }
    }

    void Stand()
    {
        Action(2f);
        PlayAnimation("sit_getup");
        isSit = false;
    }
    void GetUp()
    {
        AbnormalAttackDelay = 3f;

        Action(1.5f);
        PlayAnimation("attack_abnormal_getup");
        isSit = false;
    }

    void CheckKill(DetectObstruction detectobs, GameObject bodypart)
    {
        //if used blades
        if (detectobs.Object.GetComponent<TitanDamageCollider>())
        {
            ErenTitanTemp = detectobs.Object.transform.parent.transform.gameObject;
            Invoke("BlownAway", 0.1f);
            return;
        }
        if (detectobs.Object.GetComponent<Throwable>())
        {
            detectobs.Object.GetComponentInParent<PlayerGFX>().player.BladeHit(bodypart.transform.position);

            detectobs.Object.GetComponent<Throwable>().DestroyObject();

            detectobs.Object = null;
            return;
        }

        if (bodypart == Nape.gameObject)
        {
            detectobs.Object.GetComponentInParent<PlayerGFX>().player.AnimSlashKill(bodypart.transform.position);
        }
        else
        {
            detectobs.Object.GetComponentInParent<PlayerGFX>().player.AnimSlash(bodypart.transform.position);
        }
    }

    void BlownAway()
    {
        PlayAnimationClamp("die_blow");

        Vector3 dir = (ErenTitanTemp.transform.position - transform.position);
        Quaternion rot = Quaternion.LookRotation(dir);
        Quaternion newrot = Quaternion.Euler(0, rot.eulerAngles.y, 0);
        transform.localRotation = newrot;

        rb.velocity = transform.forward * Random.Range(-700f,-100f);
    }
}

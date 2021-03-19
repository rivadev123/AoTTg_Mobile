using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using EZCameraShake;
using UnityEngine.UI;
using ControlFreak2;
public class Player : MonoBehaviour
{
    public RigidbodyFirstPersonController rbfps;
    public CameraShaker camshake;
    private Rigidbody rb;
    private CapsuleCollider cc;
    private Camera cam;

    public float dragGround;
    public float dragAir;

    public Animation anim;
    public Transform PlayerHolder;

    public Transform PlayerGFXRot;
    public Transform PlayerGFXPos;
    public Vector3 offset;

    public float RunSpeedAnimDivide;
    public float JumpForce;
    public float InAirSpeed;

    private float DownForceGravity;
    public float DownForceGravityRate;

    float speedpercent;

    public float DodgeSpeed;

    public bool StopMove ;
    public bool IsHit;
    public bool IsAction;
    public bool IsAttack;
    public bool IsChangeLayer;

    public bool IsWallRun;

    public bool IsDodge;
    public bool IsLockOn;
    public bool IsLockDodge;

    public bool follow_rbfps_rotation;
    public bool IsGrabbed;

    public bool IsJump;
    private float jumptimer;
    [Header("3dmg")]
    public AudioSource reelsound;

    public float Range;
    public float SpeedGas;
    public float SpeedGear; //speed from hook alone
    public bool IsGas;
    public bool IsHookL;
    public bool IsHookR;
    public LayerMask hookable;

    public Transform refpointL;
    public Transform refpointR;

    public LineRenderer linerendL;
    public LineRenderer linerendR;
    public Transform hookL;
    public Transform hookR;
    public Rigidbody hookL_rb;
    public Rigidbody hookR_rb;

    public Transform springjointpivot;
    public SpringJoint springjoint;

    public float spring;
    public float damp;

     Vector2 inputReel;

    [Header("Particles")]

    public ParticleSystem slideSparks;
    public ParticleSystem GasFireLeft;
    public ParticleSystem GasFireRight;

    [Header("UI")]

    public ParticleSystem smoke;
    public ParticleSystem smokeburst;

    public RectTransform LeftUIref;
    public RectTransform RightUIref;

    public RectTransform LeftUIrefSprite;
    public RectTransform RightUIrefSprite;
    public RectTransform MidUIref;

    public TouchButtonSpriteAnimator LeftHook;
    public TouchButtonSpriteAnimator RightHook;

    private float t_L;

    private float t_R;
    public Transform hitpointL;
    public Transform hitpointR;

    public Animator DamageDealt;
    public Text DmgText;


    private Ray ray1;
    private Ray ray2;
    [Header("stats")]
    public float baseHealth;
    public float currentSpeed = 1f;

    public float currentBladeNumber;

    public float currentBladeDurability;

    public float currentGas;

    public Image[] bladesImage;
    private Sprite bladeimage;

    public Slider[] GasSlider;

    public Slider AbilityCoolDownSlider;
    public Image AbilityImage;

    private float currentabilityCD;
    private float currentDefaultabilityCD;

    private float defaultBladeNumber;
    private float defaultBladeDurability;
    private float defaultGas;

    [Header("abilities")]
    public bool mikasasmashatk;
    public bool titaneren;

    [Header("abilitywheel")]
    private float wheelTimer;
    public GameObject AbilityUI;

    
    [Header("abilities2")]


    public bool UsingAbility;

    //TitanEren
    public bool ErenAttacking;
    public Animation titananim;
    public GameObject AttackCollider;
    private float AttackComboTimer;

    //spinattack
    public bool IsSpinAttack;
    private float LeviSpinAttackTimer;
    public float LeviSpinAttack_Cd;

    //bladethrow
    public bool IsBladeThrow;
    public GameObject BladeThrowPrefab;
    public float ThrowingStrength;
    private float BladeThrowTimer;
    public float BladeThrowCoolDown;
    private GameObject bladeprefab;

    //Timestop
    public bool IsTimeStop;

    public Animator UIEffectsAnim;
    private float TimeStopTimer;
    public float TimeStopDuration;

    public float TimeStopCooldown;

    //ErenTitan
    public bool IsErenTitan;

    public GameObject ErenTitanPrefab;
    public GameObject PlayerDisable;
    public Animator FlashAnims;
    public GameObject Lightning;

    private float ErenTitanTimer;
    public float ErenTitanDuration;

    public float ErenTitanCooldown;

    //SpeedBoost
    public bool IsSpeedBoost;

    private float SpeedBoostTimer;
    public float SpeedBoostDuration;

    public float SpeedBoostCooldown;


    //bowshoot
    public bool IsbowShoot;
    public GameObject ArrowPrefab;
    public float ShootStrength;
    private float BowShootTimer;
    public float BowShootCooldown;
    private GameObject arrowlaunchpos;

    public GameObject[] weapons_old;
    public GameObject weapons_bow;

    [Header("Atttacks")]
    public float currentAttkDrag;
    private float currentAtkTime;
    [Header("AttackCollidersBlades")]
    public Collider[] blades;
    public MeshRenderer[] BladeRenderers;

    public Animator AnimHitSomething;
    public RectTransform SlashTransform;

    [Header("WallRun")]
    public DetectObstruction WallDetect;
    public float WallrunUpSpeed;

    [Header("Death")]
    public ParticleSystem blood;
    public bool IsDead;
    private bool Dead;
    Vector2 input;
    Vector3 inputVector;

    private string currentanim;
    private LevelManager lvlmanager;
    private AudioManager audio;
    private TimeManager timemanager;
    // Start is called before the first frame update

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("HurtCollider"))
        {
            IsDead = true;
        }
    }
    void Start()
    {
        audio = FindObjectOfType<AudioManager>();
        timemanager = FindObjectOfType<TimeManager>();
        lvlmanager = FindObjectOfType<LevelManager>();
        rbfps = GetComponent<RigidbodyFirstPersonController>();
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
        cam = GetComponentInChildren<Camera>();

        defaultBladeDurability = currentBladeDurability;
        defaultBladeNumber = currentBladeNumber;
        defaultGas = currentGas;

        bladeimage = bladesImage[0].sprite;

        slideSparks.enableEmission = false;

        //animlayers
        anim.wrapMode = WrapMode.Loop;
        foreach(Collider g in blades)
        {
            g.enabled = false;
        }
    }
    private void FixedUpdate()
    {
        if (IsGas)
        {
            rb.AddRelativeForce(inputVector * SpeedGas * Time.deltaTime * 50f);
        }

        if (IsHookR)
        {
            rb.AddForce((hitpointR.position - transform.position).normalized * SpeedGear * Time.deltaTime * 50f);
        }
        if (IsHookL)
        {
            rb.AddForce((hitpointL.position - transform.position).normalized * SpeedGear * Time.deltaTime * 50f);
        }
     
    }

    private void LateUpdate()
    {
        linerendL.SetPosition(0, refpointL.position);
        linerendL.SetPosition(1, hookL.position);
        linerendR.SetPosition(0, refpointR.position);
        linerendR.SetPosition(1, hookR.position);
        hookL.transform.position = Vector3.Lerp(refpointL.position, hitpointL.position, t_L);
        hookR.transform.position = Vector3.Lerp(refpointR.position, hitpointR.position, t_R);
    }
    //  private void LateUpdate()
    // {

    //  }
    // Update is called once per frame



    public void AnimSlash(Vector3 pos)
    {
        AnimHitSomething.Play("Slash", 0, 0.01f);

        Vector2 v = new Vector2(rb.velocity.x, rb.velocity.y);
        SlashTransform.transform.rotation = Quaternion.Euler(0,0,Random.Range(0,360f));

        Vector3 v2 = Camera.main.WorldToScreenPoint(pos);
        SlashTransform.transform.position = v2;

        currentBladeDurability -= 200f / rb.velocity.magnitude;
    }


    public void AnimSlashKill(Vector3 pos)
    {
        AnimHitSomething.Play("Slash", 0, 0.01f);

        Vector2 v = new Vector2(rb.velocity.x, rb.velocity.y);
        SlashTransform.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360f));

        Vector3 v2 = Camera.main.WorldToScreenPoint(pos);
        SlashTransform.transform.position = v2;

        currentBladeDurability -= 200f / rb.velocity.magnitude;

        DmgText.text = Mathf.RoundToInt(rb.velocity.magnitude * 10f).ToString();
        DamageDealt.Play("Damage", 0, 0.01f);
    }

    public void BladeHit(Vector3 pos)
    {
        {
            AnimHitSomething.Play("Slash", 0, 0.01f);

            Vector2 v = new Vector2(rb.velocity.x, rb.velocity.y);
            SlashTransform.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360f));

            Vector3 v2 = Camera.main.WorldToScreenPoint(pos);
            SlashTransform.transform.position = v2;

        }
    }




    void PlayAnimation(string Animname)
    {
        anim[Animname].layer = 1;

        anim[Animname].wrapMode = WrapMode.Once;
        anim.CrossFade(Animname,0.1f);


    }
    void makebooltrue()
    {
        IsAction = true;
    }
    void IsActionFinish()
    {
        IsAction = false;

    }
    void Update()
    {

        input.x = ControlFreak2.CF2Input.GetAxisRaw("Horizontal");
        input.y = ControlFreak2.CF2Input.GetAxisRaw("Vertical");

        float h = input.x;
        float v = input.y;
        inputVector = new Vector3(h, 0, v);
        inputVector = Vector3.ClampMagnitude(inputVector, 1);
        Vector3 relativeInputVector = transform.InverseTransformDirection(inputVector);

        //DEATH
        rb.isKinematic = false;
        cc.enabled = true;
        if (IsGrabbed)
        {
            rb.isKinematic = true;
            cc.enabled = false;

        }

        if (Dead)
        {
            rb.isKinematic = true;
            cc.enabled = false;
        }

        if (IsGrabbed || Dead)
        {
            IsAction = true;
        }


        if (!Dead && IsDead)
        {
            lvlmanager.PlayerDie();
            Dead = true;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            blood.Play();
            SkinnedMeshRenderer[] skinnedmeshrenderers = PlayerGFXPos.GetComponentsInChildren<SkinnedMeshRenderer>();
            MeshRenderer[] meshrenderers = PlayerGFXPos.GetComponentsInChildren<MeshRenderer>();


            foreach (SkinnedMeshRenderer s in skinnedmeshrenderers)
            {
                s.enabled = false;
            }
            foreach (MeshRenderer m in meshrenderers)
            {
                m.enabled = false;
            }

        }



        if (rbfps.RoofStop)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
        }



        //SmOKE
        smoke.enableEmission = false;
        if (IsGas)
        {
            smoke.enableEmission = true;
            currentGas -= Time.deltaTime;
        }







        //animation
        slideSparks.enableEmission = false;

        if (!IsAction)
        {
            if (rb.velocity.magnitude / RunSpeedAnimDivide > 1f && rbfps.Grounded)
            {
                anim.CrossFade("run_levi");
                if (rb.velocity.magnitude > 60f)
                {
                    anim.CrossFade("slide");
                    slideSparks.enableEmission = true;
                }

            }
            else
            {
                anim.CrossFade("stand_levi");
            }
            if (!rbfps.Grounded)
            {

                if (rbfps.relativeMovementUpdate.y > 0f)
                {
                    anim.CrossFade("air_rise");
                }
                else
                {
                    anim.CrossFade("air_fall");

                }
                if (IsGas)
                {
                    anim.CrossFade("air2");

                }
                if (IsHookL && !IsHookR)
                {
                    anim.CrossFade("air_hook_l");
                }
                if (IsHookR && !IsHookL)
                {
                    anim.CrossFade("air_hook_r");
                }
                if (IsHookR && IsHookL)
                {
                    anim.CrossFade("air_hook");
                }
            }
            if (IsWallRun)
            {
                anim.CrossFade("wallrun");

            }
        }
        if (IsGrabbed)
        {
            anim.CrossFade("grabbed");
        }

        //drag
        if (rbfps.Grounded)
        {

            rb.drag = dragGround;
            if (rb.velocity.magnitude > 60f)
            {
                rb.drag = 2f;
            }
        }


        if (rbfps.Grounded || IsWallRun || IsHookL || IsHookR || IsDodge)
        {
            DownForceGravity = 0f;
        }
        if(!rbfps.Grounded)
        {
            DownForceGravity += DownForceGravityRate * Time.deltaTime;
            rb.AddForce(0, -DownForceGravity, 0);
        }
        if (!rbfps.Grounded || IsJump)
        {
            rb.drag = dragAir;
        }

        if (IsAttack)
        {
            rb.drag = currentAttkDrag;
        }
        if (IsDodge)
        {
            rb.drag = 2f;
        }


        //Blades and Gas UI
        Color c = Color.white;
        c.a = 0;
        Color w = Color.white;
        w.a = 1;
        Color b = Color.black;

        UpdateAbility();
  

        GasSlider[0].value = currentGas / defaultGas;
        GasSlider[1].value = currentGas / defaultGas;

        if (currentBladeNumber >= 3)
        {

            bladesImage[0].color = w;
            bladesImage[1].color = w; 
            bladesImage[2].color = w;
        }
        if (currentBladeNumber ==2)
        {     
            bladesImage[0].color = w;
            bladesImage[1].color = w;
            bladesImage[2].color = c;
        }
        if (currentBladeNumber == 1)
        {
            bladesImage[0].color = w;
            bladesImage[1].color = c;
            bladesImage[2].color = c;
        }
        if (currentBladeNumber == 0)
        {
            bladesImage[0].color = c;
            bladesImage[1].color = c;
            bladesImage[2].color = c;
        }
        //3dmg
   
      
        springjoint.spring = 0f;
        springjoint.damper = 0f;
        rb.useGravity = true;
        springjoint.connectedBody = null;

        if(!IsHookR && !IsHookL)
        {
            springjoint.maxDistance = 0f;
        }

        if (IsHookL)
        {
            rb.useGravity = false;
            springjointpivot.position = hitpointL.position;
            linerendL.enabled = true;
            t_L += 1f * Time.deltaTime;
            t_L = Mathf.Clamp01(t_L);
            springjoint.spring = spring;
            springjoint.damper = damp;

            springjoint.connectedBody = hookL_rb;


            LeftHook.spriteNeutral.color = w;
        }
        else
        {
            LeftHook.spriteNeutral.color = b;

            linerendL.enabled = false;

            t_L -= 1f * Time.deltaTime;
            t_L = Mathf.Clamp01(t_L);
        }
        if (IsHookR)
        {
            rb.useGravity = false;
            springjointpivot.position = hitpointR.position;

            linerendR.enabled = true;

            t_R += 1f * Time.deltaTime;
            t_R = Mathf.Clamp01(t_R);
            springjoint.spring = spring;
            springjoint.damper = damp;

            springjoint.connectedBody = hookR_rb;

            RightHook.spriteNeutral.color = w;

        }
        else
        {
            RightHook.spriteNeutral.color = b;

            linerendR.enabled = false;

            t_R -= 1f * Time.deltaTime;
            t_R = Mathf.Clamp01(t_R);
        }


        if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.Q) && !IsHookL && !IsHookR)
        {
            IsLockOn = false;
            FireHookL();
            FireHookR();
        }
        if (ControlFreak2.CF2Input.GetKeyUp(KeyCode.Q))
        {
            DetachHooks();
        }
        if (ControlFreak2.CF2Input.GetButtonDown("Fire1"))
        {
            IsLockOn = true;
            FireHookL();
        }
        if (ControlFreak2.CF2Input.GetButtonDown("Fire2"))
        {
            IsLockOn = true;
            FireHookR();
        }

        //Reel
        float reely = ControlFreak2.CF2Input.GetAxisRaw("VerticalReel");

        if(reely > 0.1f)
        {
            springjoint.maxDistance += reely * 15f * Time.deltaTime;
        }
        if (reely < -0.1f)
        {
            springjoint.maxDistance += reely * 15f * Time.deltaTime;
        }
        //Boost
        if (inputVector.magnitude != 0f && !rbfps.Grounded && currentGas > 0f)
        {
            IsGas = true;
        }
        else
        {
            IsGas = false;
        }

        jumptimer -= Time.deltaTime;
        if (rbfps.Grounded && ControlFreak2.CF2Input.GetKeyDown(KeyCode.Space))
        {
            jumptimer = 0.2f;
            IsJump = true;
            rb.velocity = new Vector3(rb.velocity.x, JumpForce,rb.velocity.z);
        }
        if (!rbfps.Grounded && relativeInputVector.magnitude != 0f)
        {
            //rb.AddRelativeForce(inputVector * InAirSpeed);
        }
        if(rbfps.Grounded && jumptimer < 0)
        {
            IsJump = false;
        }

        ///CANMOVE
        rbfps.CanMove = true;
              
        if (IsAttack)
        {
            rbfps.CanMove = false;
        }
        if(StopMove)
        {
            rbfps.CanMove = false;
        }
        if (rb.velocity.magnitude > 60f && rbfps.Grounded)
        {
            rbfps.CanMove = false;
        }

        PlayerGFXPos.transform.position = transform.position + offset;

        if (!IsGrabbed)
        {
            if (Mathf.Abs(rb.velocity.x + rb.velocity.z) > 1f && !follow_rbfps_rotation && !IsGas)
            {
                Quaternion rot = Quaternion.LookRotation(rb.velocity);
                Quaternion newrot = Quaternion.Euler(0, rot.eulerAngles.y, 0);

                PlayerGFXRot.transform.localRotation = Quaternion.Lerp(PlayerGFXRot.transform.localRotation, newrot, Time.deltaTime * 20f);
            }
            if (Mathf.Abs(rb.velocity.x + rb.velocity.z) > 1f && IsGas && !rbfps.Grounded)
            {
                Quaternion rot = Quaternion.LookRotation(rb.velocity);
                Quaternion newrot = Quaternion.Euler(0, rot.eulerAngles.y, 0);

                PlayerGFXRot.transform.localRotation = Quaternion.Lerp(PlayerGFXRot.transform.localRotation, newrot, Time.deltaTime * 20f);
            }
            if (follow_rbfps_rotation || IsWallRun)
            {
                Quaternion rot = rb.transform.rotation;
                Quaternion newrot = Quaternion.Euler(0, rot.eulerAngles.y, 0);
                PlayerGFXRot.transform.localRotation = newrot;
            }
        }


        if(input.y > 0.3f && WallDetect.Obstruction && !IsDodge && !IsAttack && currentGas > 0f)
        {
            IsWallRun = true;
            rb.velocity = transform.up * WallrunUpSpeed + transform.forward * 2f;
            currentGas -= Time.deltaTime;        }
        else
        {
            IsWallRun = false;
        }


        //ROLL
        if (!rbfps.Grounded && ControlFreak2.CF2Input.GetKeyDown(KeyCode.Space) && (!IsAction || IsAttack) && currentGas > 0f) 
        {
            Dodge();
        }

        //Reload
        if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.R)  && !IsAction && currentBladeNumber > 0f)
        {
            ReloadBlade();
        }
        //ATTACK

        if(currentBladeDurability <= 0f && BladeRenderers[0].enabled)
        {
            foreach(MeshRenderer r in BladeRenderers)
            {
                r.enabled = false;
            }
        }
        if (currentBladeDurability > 0f && !BladeRenderers[0].enabled && !IsDead)
        {
            foreach (MeshRenderer r in BladeRenderers)
            {
                r.enabled = true;
            }
        }
        if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.F) && !IsAttack && !IsDodge && !IsAction )
        {
            Attack();
        }

        //Here
        LeviSpinAttackTimer -= Time.deltaTime;
        BladeThrowTimer -= Time.deltaTime;
        TimeStopTimer -= Time.deltaTime;
        ErenTitanTimer -= Time.deltaTime;
        SpeedBoostTimer -= Time.deltaTime;
        BowShootTimer -= Time.deltaTime;

        if (wheelTimer > 0f && !IsGrabbed)
        {
            if (ControlFreak2.CF2Input.GetKeyUp(KeyCode.LeftShift) && !IsAttack && !IsDodge && IsSpinAttack && LeviSpinAttackTimer < 0f && !IsAction)
            {
                LeviAttack();
                LeviSpinAttackTimer = LeviSpinAttack_Cd;
            }
            if (ControlFreak2.CF2Input.GetKeyUp(KeyCode.LeftShift)  && BladeThrowTimer < 0f && currentBladeDurability > 0f && IsBladeThrow)
            {
                BladeThrow();
                BladeThrowTimer = BladeThrowCoolDown;
            }
            if (ControlFreak2.CF2Input.GetKeyUp(KeyCode.LeftShift) && TimeStopTimer < 0f  && IsTimeStop)
            {
                TimeStop();
                TimeStopTimer = TimeStopCooldown;
            }
            if (ControlFreak2.CF2Input.GetKeyUp(KeyCode.LeftShift) && ErenTitanTimer < 0f && IsErenTitan)
            {
                ErenTitanTimer = ErenTitanCooldown;
                ErenTitanActivate();

            }
            if (ControlFreak2.CF2Input.GetKeyUp(KeyCode.LeftShift) && SpeedBoostTimer < 0f && IsSpeedBoost)
            {
                SpeedBoostTimer = SpeedBoostCooldown;
                SpeedBoostStart();
            }
            if (ControlFreak2.CF2Input.GetKeyUp(KeyCode.LeftShift) && BowShootTimer < 0f && IsbowShoot)
            {
                BowShootTimer = BowShootCooldown;
                ShootBow();
            }
        }

        //abilityUI
        if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.LeftShift))
        {
            wheelTimer = 0.3f;
        }
        if (ControlFreak2.CF2Input.GetKey(KeyCode.LeftShift))
        {
            wheelTimer -= Time.deltaTime;
        }
        if(wheelTimer < 0f && !AbilityUI.activeInHierarchy)
        {
            OpenAbilityUI();       
        }
    }


    void FireHookL()
    {
        if (IsHookL)
        {
            hitpointL.parent = null;
            audio.Play("HookReturn");
            reelsound.Stop();

            IsHookL = false;
            if (rbfps.relativeMovementUpdate.y > 60f && !IsHookR)
            {
                IsAction = true;
                Invoke(nameof(IsActionFinish), 0.5f);
                PlayAnimation("air_release");

            }
            return;
        }
        if (IsLockOn)
        {
            ray1 = cam.ScreenPointToRay(MidUIref.position);
        }
        else
        {
            ray1 = cam.ScreenPointToRay(LeftUIref.position);
        }
        Vector3 point = cam.ScreenToWorldPoint(LeftUIref.position);
        RaycastHit hit;
        if (Physics.Raycast(ray1, out hit, Range, hookable))
        {
            GasFireLeft.Play();

            audio.Play("HookFire");
            reelsound.Play();

            IsHookL = true;
            hitpointL.parent = hit.collider.transform;

            hitpointL.position = hit.point;

            if ((hitpointL.position - transform.position).magnitude > springjoint.maxDistance)
            {
                springjoint.maxDistance = (hitpointL.position - transform.position).magnitude;
            }


        }
    }
    void FireHookR()
    {
        if (IsHookR)
        {
            hitpointR.parent = null;
            audio.Play("HookReturn");
            reelsound.Stop();

            if (rbfps.relativeMovementUpdate.y > 60f && !IsHookL)
            {
                IsAction = true;
                Invoke(nameof(IsActionFinish), 0.5f);
                PlayAnimation("air_release");

            }
            IsHookR = false;
            return;
        }
        if (IsLockOn)
        {
            ray2 = cam.ScreenPointToRay(MidUIref.position);
        }
        else
        {
            ray2 = cam.ScreenPointToRay(RightUIref.position);
        }
        Vector3 point = cam.ScreenToWorldPoint(RightUIref.position);
        RaycastHit hit;
        if (Physics.Raycast(ray2, out hit, Range, hookable))
        {
            IsHookR = true;
            audio.Play("HookFire");
            GasFireRight.Play();
            reelsound.Play();

            hitpointR.parent = hit.collider.transform;
            hitpointR.position = hit.point;
            if ((hitpointR.position - transform.position).magnitude > springjoint.maxDistance)
            {
                springjoint.maxDistance = (hitpointR.position - transform.position).magnitude;
            }

        }
    }
    public void DetachHooks()
    {

        hitpointR.parent = null;
        audio.Play("HookReturn");
        reelsound.Stop();
        IsHookR = false;

        hitpointL.parent = null;
        audio.Play("HookReturn");
        reelsound.Stop();
        IsHookL = false;

    }
    void Attack()
    {
        audio.Play("Slash");
       
        IsAction = true;
        Invoke(nameof(IsActionFinish), 1f);
        Invoke("StartAttack", 0f);
        currentAtkTime = 0.5f;
      
        PlayAnimation("attack1_hook_l1");
    }
    void ReloadBlade()
    {
        if (rbfps.Grounded)
        {

            IsAction = true;
            Invoke(nameof(IsActionFinish), 1.7f);
            Invoke(nameof(ReloadedBlade), 1f);
            StopMoveFunc(1.7f);
            PlayAnimation("changeBlade");
        }
        if (!rbfps.Grounded)
        {
            IsAction = true;
            Invoke(nameof(IsActionFinish), 0.5f);
            Invoke(nameof(ReloadedBlade), 0.25f);
            StopMoveFunc(0.5f);

            PlayAnimation("changeBlade_air");
        }
    }
    void ReloadedBlade()
    {
        currentBladeDurability = defaultBladeDurability;
        currentBladeNumber--;
    }
    void LeviAttack()
    {

        IsAction = true;
        Invoke(nameof(IsActionFinish), 1f);

        Invoke("StartAttack", 0.5f);
        Invoke("LeviBoost", 0.5f);

        currentAtkTime = 0.5f;

        PlayAnimation("attack5");
    }
    void LeviBoost()
    {
        audio.Play("SpinAttack");

        float mag = rb.velocity.magnitude;

        mag = Mathf.Clamp(mag, 20f, 1000f);
        rb.velocity = cam.transform.forward * mag;
    }

    void BladeThrow()
    {
        //audio.Play("Slash");
        
        IsAction = true;
        Invoke(nameof(IsActionFinish), 1f);
        Invoke(nameof(ThrowBlades), 0.5f);
        Invoke(nameof(FinishThrowingBlades), 1f);

        follow_rbfps_rotation = true;

        PlayAnimation("bladethrow");
    }
    void ThrowBlades()
    {
        bladeprefab = Instantiate(BladeThrowPrefab,PlayerHolder);
        bladeprefab.transform.position = transform.position + transform.forward * 2f;
        bladeprefab.transform.rotation = transform.rotation;
        bladeprefab.GetComponent<Rigidbody>().velocity = cam.transform.forward * ThrowingStrength;
        currentBladeDurability = 0f;
    }
    void FinishThrowingBlades()
    {
        follow_rbfps_rotation = false;
    }
   

    public void TimeStop()
    {
        timemanager.Freeze();
        UIEffectsAnim.Play("TimeStop");
        Invoke("TimeContinue", TimeStopDuration);
    }
    void TimeContinue()
    {
        UIEffectsAnim.Play("TimeContinue");

        timemanager.Unfreeze();
    }

    void ErenTitanActivate()
    {
        GameObject g = Instantiate(ErenTitanPrefab);
        g.transform.position = transform.position;
        g.transform.rotation = transform.rotation;
        PlayerDisable.SetActive(false);
        g.GetComponentInChildren<TitanEren>().PlayerObject = PlayerDisable;
    }

    public void SpeedBoostStart()
    {
        Invoke("SpeedBoostSlow", SpeedBoostDuration);
        currentSpeed = currentSpeed * 2;
        SpeedGas = SpeedGas * 2;
    }
    void SpeedBoostSlow()
    {
        currentSpeed = currentSpeed / 2;
        SpeedGas = SpeedGas / 2;
    }

    void ShootBow()
    {
        //audio.Play("Slash");

        IsAction = true;
        Invoke(nameof(IsActionFinish), 2f);
        Invoke(nameof(ArrowFire), 0.25f);
        Invoke(nameof(ArrowFire), 0.83f);
        Invoke(nameof(ArrowFire), 1.45f);

        Invoke(nameof(FinishShootBow), 2.5f);

        follow_rbfps_rotation = true;

        weapons_bow.SetActive(true);
        foreach(GameObject g in weapons_old)
        {
            g.SetActive(false);
        }
        PlayAnimation("shootbow");
    }
    void ArrowFire()
    {
        audio.Play("ArrowShoot");

        bladeprefab = Instantiate(ArrowPrefab, PlayerHolder);
        bladeprefab.transform.position = transform.position + transform.forward * 2f;
        bladeprefab.transform.rotation = transform.rotation;
        bladeprefab.GetComponent<Rigidbody>().velocity = cam.transform.forward * ShootStrength;
    }
    void FinishShootBow()
    {
        follow_rbfps_rotation = false;
        weapons_bow.SetActive(false);
        foreach (GameObject g in weapons_old)
        {
            g.SetActive(true);
        }
    }


    void Dodge()
    {
        currentGas -= 10f;
        Invoke("FinishDodge", 0.5f);
        smokeburst.Play();
        if (!rbfps.Grounded)
            {
            if (IsLockOn && (IsHookL || IsHookR))
            {
                if (!IsLockDodge)
                {
                    if (IsHookL && IsHookR)
                    {
                        rb.velocity = ((hitpointL.position + hitpointR.position) / 2 - transform.position).normalized * rb.velocity.magnitude;
                    }
                    if (IsHookL)
                    {
                        rb.velocity = (hitpointL.position - transform.position).normalized * rb.velocity.magnitude;
                    }
                    if (IsHookR)
                    {
                        rb.velocity =(hitpointR.position - transform.position).normalized * rb.velocity.magnitude;
                    }
                   camshake.ShakeOnce(2f, 5f, 0f, 0.7f);
                    IsLockDodge = true;
                }
            }
            else
            {
                if (!IsDodge && !IsAttack)
                {
                    IsDodge = true;
                    if (inputVector.magnitude != 0f)
                    {
                        rb.AddRelativeForce(inputVector * DodgeSpeed, ForceMode.VelocityChange);
                    }
                    else
                    {
                        rb.velocity = transform.forward * DodgeSpeed;
                    }
                    IsAction = true;
                    Invoke(nameof(IsActionFinish), 0.5f);
                    PlayAnimation("dash");
                    camshake.ShakeOnce(2f, 5f, 0f, 0.7f);
                }
            }
         
        }

    }
    void FinishDodge()
    {
        IsDodge = false;
        IsLockDodge = false;
    }
    

    void StartAttack()
    {
        IsAttack = true;
        if (currentBladeDurability > 0f)
        {
            foreach (Collider g in blades)
            {
                g.enabled = true;
            }
        }
        CancelInvoke("FinishAttack");
        Invoke("FinishAttack",currentAtkTime);
    }
    void FinishAttack()
    {

        if (IsAttack)
        {
            IsAttack = false;
            foreach (Collider g in blades)
            {
                g.enabled = false;
            }
        }
    }
    
    void StopMoveFunc(float time)
    {
        StopMove = true;
        CancelInvoke("UnstopMove");
        Invoke("UnstopMove",time);
    }
    void UnstopMove()
    {
        StopMove = false;
    }

    public void OpenAbilityUI()
    {
        AbilityUI.SetActive(true);
    }
    public void SelectAbility(int abilityindex, Image img)
    {
        wheelTimer = 0.3f;

        //HERE
        IsSpinAttack = false;
        IsBladeThrow = false;
        IsTimeStop = false;
        IsErenTitan = false;
        IsSpeedBoost = false;
        IsbowShoot = false;

        AbilityUI.SetActive(false);

        AbilityImage.sprite = img.sprite;

        //HERE
        if(abilityindex == 0)
        {
            IsSpinAttack = true;
        }
        if (abilityindex == 1)
        {
            IsBladeThrow = true;
        }
        if (abilityindex == 2)
        {
            IsTimeStop = true;
        }
        if (abilityindex == 3)
        {
            IsErenTitan = true;
        }
        if (abilityindex == 4)
        {
            IsSpeedBoost = true;
        }
        if (abilityindex == 5)
        {
            IsbowShoot = true;
        }
    }
    void UpdateAbility()
    {
        //HERE
        if (IsSpinAttack)
        {
            AbilityCoolDownSlider.value = (LeviSpinAttack_Cd-LeviSpinAttackTimer) / LeviSpinAttack_Cd;
        }
        if (IsBladeThrow)
        {
            AbilityCoolDownSlider.value = (BladeThrowCoolDown-BladeThrowTimer) / BladeThrowCoolDown;
        }
        if (IsTimeStop)
        {
            AbilityCoolDownSlider.value = (TimeStopCooldown - TimeStopTimer) / TimeStopCooldown;
        }
        if (IsErenTitan)
        {
            AbilityCoolDownSlider.value = (ErenTitanCooldown - ErenTitanTimer) / ErenTitanCooldown;
        }
        if (IsSpeedBoost)
        {
            AbilityCoolDownSlider.value = (SpeedBoostCooldown - SpeedBoostTimer) / SpeedBoostCooldown;
        }
        if (IsbowShoot)
        {
            AbilityCoolDownSlider.value = (BowShootCooldown - BowShootTimer) / BowShootCooldown;
        }
    }

    public void Refill()
    {
        currentBladeNumber = defaultBladeNumber;
        currentGas = defaultGas;
    }
}


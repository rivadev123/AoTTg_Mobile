using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColossalTitan : MonoBehaviour
{
    public Animation anim;


    public float SmokeBlastTime;
    float blasttimer;
    public bool IsAttack;

    public DetectObstruction Front;
    public DetectObstruction Vertical;

    public GameObject[] attackcolliders;
    public DetectObstruction Nape;

    public GameObject SmokeAttackCollider;
    public ParticleSystem SmokeBlast;

    public float HP;

    bool hitCD;
    // Start is called before the first frame update
    void Start()
    {
        blasttimer = SmokeBlastTime;
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

    void Attack(float time)
    {
        IsAttack = true;
        CancelInvoke("AttackBody2");
        Invoke("AttackBody2", time);

        foreach (GameObject g in attackcolliders)
        {
            g.tag = "HurtCollider";
        }

    }
    void AttackBody2()
    {
        IsAttack = false;
        foreach (GameObject g in attackcolliders)
        {
            g.tag = "Untagged";
        }
    }
    void AttackSmoke(float time)
    {
        IsAttack = true;
        CancelInvoke("AttackSmoke2");
        Invoke("AttackSmoke2", time);


    }
    void AttackSmoke2()
    {
        IsAttack = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!IsAttack)
        {
            anim.Play("idle");

            if (Front.Obstruction)
            {
                Attack(6f);
                PlayAnimation("attack_sweep");
            }
            if(Vertical.Obstruction)
            {
                Attack(6f);
                PlayAnimation("attack_sweep_vertical");
            }
        }
        if(Nape.Obstruction && !hitCD)
        {
            hitCD = true;
            HP--;
            Invoke("HitCdFinish", 2f);
            CheckKill(Nape, Nape.transform.gameObject);
            blasttimer = 0f;
        }

        if (HP <=0 )
        {
            Die();
        }

        if(!IsAttack)
        {
            blasttimer -= Time.deltaTime;
        }
        if(blasttimer < 0f && !IsAttack)
        {
            blasttimer = SmokeBlastTime;
            AttackSmoke(4f);
            PlayAnimation("attack_steam");
            Invoke("SmokeBlast1", 1.2f);
        }
    }

    void SmokeBlast1()
    {
        SmokeBlast.Play();
    }
    void HitCdFinish()
    {
        hitCD = false;
    }

    void CheckKill(DetectObstruction detectobs, GameObject bodypart)
    {
        //if used blades

        if (detectobs.Object.GetComponent<Throwable>())
        {
            detectobs.Object.GetComponentInParent<PlayerGFX>().player.BladeHit(bodypart.transform.position);

            detectobs.Object.GetComponent<Throwable>().DestroyObject();

            detectobs.Object = null;
            return;
        }

         detectobs.Object.GetComponentInParent<PlayerGFX>().player.AnimSlash(bodypart.transform.position);
       
    }

    void Die()
    {
        FindObjectOfType<LevelManager>().MissionComplete("You Beat the Colossal!");

        if (GetComponentInChildren<hitpoint>())
        {
            GetComponentInChildren<hitpoint>().player.DetachHooks();
        }

        Destroy(gameObject);
    }
}

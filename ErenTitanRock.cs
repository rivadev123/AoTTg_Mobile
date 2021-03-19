using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErenTitanRock : MonoBehaviour
{
    public Animator animwalk;
    public Animation anim;
    public DetectObstruction titaninfront;

    bool move;
    bool done;
    public float timemoved;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("StartMove", 5f);
        animwalk.speed = 0;
    }

    private void StartMove()
    {
        move = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!done)
        {
            if (move)
            {

                if (titaninfront.Obstruction)
                {
                    animwalk.speed = 0;
                    anim.enabled = false;
                }
                else
                {
                    animwalk.speed = 1;
                    anim.enabled = true;
                    anim.Play("rock_walk");
                    timemoved += Time.deltaTime;


                }
            }
            else
            {
                anim.Play("rock_lift");
            }
        }

        if(timemoved > 90f && !done)
        {
            done = true;
            Invoke("FinishLevel", 3f);
        }
        if(done)
        {
            anim.Play("rock_fix_hole");
        }
    }

    void FinishLevel()
    {
        FindObjectOfType<LevelManager>().MissionComplete("Eren closed the wall! Humanity's first victory!");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int MaxEnemies;
    public int enemycount;


    public GameObject TitanNormal;
    public GameObject TitanAbnormal;

    private Transform[] spawns;
    public Transform[] spawnsCity;
    public Transform[] spawnsForest;
    public GameObject[] levels;


    public Animator LoadingScreen;
    int index_;


    private float timesurvived;
    public Text timesurvivedText;

    public Text enemieskilledendtext;

    [Header("Survival")]

    public float LevelChosen; 

    public float LevelIndex; //0 easy sirvival 1 med survival 2 hard survival
    public float LevelType; //0 survival 1 colossal


    public int enemieskilled;
    public Text enemiesKilledText;
    public Text EndMessage;
    public Animator LevelEndSurvival;


    public float SpawnTime;
    private float timer;
    private TimeManager tm;

    public Animator LevelStartAnim;
    public Text LevelStartText;

    public Animator IntroCam;

    [Header("Collosal")]
    public GameObject ColossalTitan;
    [Header("ErenTitanRock")]
    public GameObject ErenTitanRock;


    // Start is called before the first frame update
    void Start()
    {
        tm = FindObjectOfType<TimeManager>();
        enemiesKilledText.gameObject.SetActive(false);
        enemiesKilledText.gameObject.SetActive(true);

        LevelIndex = PlayerPrefs.GetInt("LevelIndex");
        LevelChosen = PlayerPrefs.GetInt("LevelChosen");


        LevelType = PlayerPrefs.GetInt("LevelType"); //COLOSSAL
        ColossalTitan.SetActive(false);
        ErenTitanRock.SetActive(false);

        if (LevelType == 1)
        {
            ColossalTitan.SetActive(true);
            LevelChosen = 0;
        }
        if (LevelType == 2)
        {
            ErenTitanRock.SetActive(true);
            LevelChosen = 0;
        }

        if (LevelIndex == 0)
        {
            MaxEnemies = 5;
        }
        if (LevelIndex == 1)
        {
            MaxEnemies = 10;
        }





        foreach(GameObject g in levels)
        {
            g.SetActive(false);
        }
        if (LevelChosen == 0)
        {
            spawns = spawnsCity;
            levels[0].SetActive(true);
        }
        if (LevelChosen == 1)
        {
            spawns = spawnsForest;
            levels[1].SetActive(true);
        }

        MissionStart();
    }


    public void Restart()
    {
        GoTolevel(SceneManager.GetActiveScene().buildIndex);
    }

     void GoTolevel(int index)
    {
        LoadingScreen.Play("Load");
        index_ = index;
        CancelInvoke("GoToLevel");
        Invoke("GoToLevel", 0.5f);
    }
    void GoToLevel()
    {
        SceneManager.LoadScene(index_);
    }

    private void Update()
    {
        enemiesKilledText.text = "Titans Killed " + enemieskilled.ToString();
        timesurvived += Time.deltaTime;

        if (LevelType == 0)
        {

            //Survival
            timer -= Time.deltaTime;
            if (timer <= 0 & enemycount <= MaxEnemies && !tm.TimeIsStopped)
            {
                timer = SpawnTime;

                if (LevelIndex == 0)
                {
                    SpawnEasy();
                }
                if (LevelIndex == 1)
                {
                    SpawnMedium();
                }
            }
        }
        if(LevelType == 1)
        {

        }
    }


    void SpawnEasy()
    {
        SpawnTitan(TitanNormal);
    }
    void SpawnMedium()
    {
        int random = Random.Range(0, 2);
        if (random == 0)
        {
            SpawnTitan(TitanNormal);
        }
        else
        {
            SpawnTitan(TitanAbnormal);
        }
    }
    void SpawnTitan(GameObject g)
    {
        GameObject g2 = Instantiate(g);

        int r = Random.Range(0, spawns.Length);
        g2.transform.position = spawns[r].position;
        g2.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);

        float r2 = Random.Range(2,6);

        g2.transform.localScale = new Vector3(r2, r2, r2);
    }

    public void TitanSpawn()
    {
        enemycount++;
    }
    public void TitanDie()
    {
        enemycount--;
        enemieskilled++;
    }
    public void PlayerDie()
    {
        enemieskilledendtext.text = enemieskilled.ToString();
        timesurvivedText.text = Mathf.RoundToInt(timesurvived).ToString();
        Invoke("GameOver", 2f);
    }
    void GameOver()
    {
        LevelEndSurvival.gameObject.SetActive(true);
        LevelEndSurvival.Play("End");
    }

    void MissionStart()
    {
        if(LevelType == 1)
        {
            LevelStartAnim.Play("LevelStart");
            LevelStartText.text = "Defeat the colossal, hit its nape 10 times. Titans are guarding the fuel refill stations";
            IntroCam.Play("ColosaalIntro");
        }

        if (LevelType == 2)
        {
            LevelStartAnim.Play("LevelStart");
            LevelStartText.text = "Protect Eren and kill the titans in front of him";
            IntroCam.Play("ErenRockIntro");
        }
    }
    public void MissionComplete(string text)
    {
        EndMessage.text = text;
        enemieskilledendtext.text = enemieskilled.ToString();
        timesurvivedText.text = Mathf.RoundToInt(timesurvived).ToString();
        Invoke("GameOver", 2f);
    }
}

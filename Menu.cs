using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Menu : MonoBehaviour
{

    public Animator LoadingScreen;
    int index_;
    public bool IsMenu;

    public Text charactersText;
    public Text LevelsText;

    private int index_char;
    private int index_level;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1280, 720,true);
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.None;

        index_char = PlayerPrefs.GetInt("CharacterIndex");
        index_char = Mathf.Clamp(index_char, 0, 3);

        index_level = PlayerPrefs.GetInt("LevelChosen");
        index_level = Mathf.Clamp(index_level, 0, 1);

    }

    // Update is called once per frame
    public void IncreaseCharacterIndex()
    {

        index_char++;
        index_char = Mathf.Clamp(index_char, 0, 3);

        PlayerPrefs.SetInt("CharacterIndex", index_char);
    }
    public void DecreaseCharacterIndex()
    {
        index_char--;
        index_char = Mathf.Clamp(index_char, 0, 3);

        PlayerPrefs.SetInt("CharacterIndex", index_char);
    }
    public void IncreaseLevelIndex()
    {

        index_level++;
        index_level = Mathf.Clamp(index_level, 0, 1);

        PlayerPrefs.SetInt("LevelChosen", index_level);
    }
    public void DecreaseLevelIndex()
    {
        index_level--;
        index_level = Mathf.Clamp(index_level, 0, 1);

        PlayerPrefs.SetInt("LevelChosen", index_level);
    }
    public void SelectLevelType(int type)
    {
        PlayerPrefs.SetInt("LevelType", type);
    }
    void Update()
    {
  
     

        if (IsMenu)
        {
 

            if (index_char == 0)
            {
                charactersText.text = "Levi";
            }
            if (index_char == 1)
            {
                charactersText.text = "Eren";
            }
            if (index_char == 2)
            {
                charactersText.text = "Mikasa";
            }
            if (index_char == 3)
            {
                charactersText.text = "Sasha";
            }


            if (index_level == 0)
            {
                LevelsText.text = "City";
            }
            if (index_level == 1)
            {
                LevelsText.text = "Forest";
            }
        }
    }

    public void GoTolevel(int index)
    {
        LoadingScreen.Play("Load");
        index_ = index;
        CancelInvoke("GoToLevel");
        Invoke("GoToLevel",0.5f);
    }
    void GoToLevel()
    {
        SceneManager.LoadScene(index_);
    }
    public void SetIndex(int index)
    {
        PlayerPrefs.SetInt("LevelIndex", index);
    }

    public void YoutubeLink()
    {
        Application.OpenURL("https://www.youtube.com/channel/UC9FN40k-sywKM4v2CSHClbg");
    }
    public void DiscordLink()
    {
        Application.OpenURL("https://discord.gg/Knn8zxh");
    }
}

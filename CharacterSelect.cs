using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    public AbilityUI abilityUI;

    public int CharacterIndex; //0 = levi 1 = eren 2 = mikasa

    public MeshRenderer PlayerEye;
    public GameObject[] characterHair;

    // Start is called before the first frame update
    void Start()
    {   // levi 0.88 ereneyes 1.62

        CharacterIndex = PlayerPrefs.GetInt("CharacterIndex");
        int LevelType = PlayerPrefs.GetInt("LevelType");
        if (LevelType == 0)
        {
            if (CharacterIndex == 0) //levi
            {
                abilityUI.abilityindexes = new int[3] { 0, 1, 2 };
                PlayerEye.material.SetTextureOffset("_MainTex", new Vector2(0, 0.88f));
            }
            if (CharacterIndex == 1) //eren
            {
                abilityUI.abilityindexes = new int[2] {3 ,4};
                PlayerEye.material.SetTextureOffset("_MainTex", new Vector2(0, 1.62f));
            }
            if (CharacterIndex == 2) //mikasa
            {
                abilityUI.abilityindexes = new int[2] { 2, 4 };
                PlayerEye.material.SetTextureOffset("_MainTex", new Vector2(0, 0.76f));
            }
            if (CharacterIndex == 3) //sasha
            {
                abilityUI.abilityindexes = new int[2] { 2, 5 };
                PlayerEye.material.SetTextureOffset("_MainTex", new Vector2(0, 0.51f));
            }
        }
        if (LevelType == 1) //colossal
        {
            abilityUI.abilityindexes = new int[1] {  4};
            PlayerEye.material.SetTextureOffset("_MainTex", new Vector2(0, 1.62f));
            CharacterIndex = 1;
        }
        if (LevelType == 2) //erenrock
        {
            abilityUI.abilityindexes = new int[2] { 2, 4 };
            PlayerEye.material.SetTextureOffset("_MainTex", new Vector2(0, 0.76f));
            CharacterIndex = 2;
        }

        foreach (GameObject g in characterHair)
        {
            g.SetActive(false);
        }
        characterHair[CharacterIndex].SetActive(true);
        abilityUI.SelectAbilities();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    public Player player;
    public GameObject AbilityButton;
    public Transform AbilityHolder;



    public int[] abilityindexes; //0 = spinattack 1 bladethrow
    public Image[] abilityImages; //0 = spinattack 1 bladethrow

    public float offset;
    // Start is called before the first frame update

    // Update is called once per frame
    public void SelectAbilities()
    {
        
        for (int i = 0; i< abilityindexes.Length; i++)
        {
            Debug.Log(i);
            GameObject g = Instantiate(AbilityButton, AbilityHolder);
            g.GetComponent<AbilityButton>().index = abilityindexes[i];

            g.GetComponent<RectTransform>().anchoredPosition = new Vector2(130 + (offset * i),0.4f);

        }

        player.SelectAbility(abilityindexes[0],abilityImages[abilityindexes[0]]);
    }
    public void ChooseButton(int index)
    {
        player.SelectAbility(index,abilityImages[index]);
    }
}

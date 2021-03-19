using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    public int index;

    private void Start()
    {
        GetComponent<Image>().sprite = GetComponentInParent<AbilityUI>().abilityImages[index].sprite;
    }
    public void ChooseButton()
    {
        GetComponentInParent<AbilityUI>().ChooseButton(index);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDataHealthBar : MonoBehaviour
{
    public Slider slider;
    public CharacterData cData = null;
    private void Update()
    {
        //if(cData.maxHealth != 0)
            //slider.value = (float)cData.health / cData.maxHealth;
        Debug.Log(cData.health / cData.maxHealth);
    }
}

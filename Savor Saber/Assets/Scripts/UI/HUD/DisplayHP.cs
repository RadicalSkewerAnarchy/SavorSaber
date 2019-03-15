using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHP : MonoBehaviour
{
    private CharacterData playerData;
    private Image barCover;

    // Start is called before the first frame update
    void Start()
    {
        GameObject play = GameObject.FindGameObjectWithTag("Player");
        playerData = play.GetComponent<CharacterData>();
        barCover = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        barCover.fillAmount = (float)playerData.health / playerData.maxHealth;
    }
}

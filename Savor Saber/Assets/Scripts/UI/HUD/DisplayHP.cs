using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHP : MonoBehaviour
{
    private CharacterData playerData;
    private Image barCover;
    public Image faceSprite;
    public Sprite normalSprite;
    public Sprite damagedSprite;
    public Sprite criticalSprite;

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
        if(barCover.fillAmount<= 0.8f && barCover.fillAmount > 0.4f)
        {
            faceSprite.sprite = damagedSprite;
        }
        else if(barCover.fillAmount <= 0.4f)
        {
            faceSprite.sprite = criticalSprite;
        }
        else
        {
            faceSprite.sprite = normalSprite;
        }
    }
}

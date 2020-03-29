using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PartyUIManager : MonoBehaviour
{

    public Image companionImage;
    public Slider companionHP;

    private AIData companionData;
    private Sprite companionSprite;
    private GameObject currentCompanion = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateHP();
    }

    public void ChangeCompanion(GameObject companion)
    {
        Debug.Log("UI: changing companion to " + companion);
        currentCompanion = companion;
        companionData = companion.GetComponent<AIData>();
        companionImage.sprite = companion.GetComponent<SpriteRenderer>().sprite;
    }

    private void UpdateHP()
    {
        if (currentCompanion != null && companionData != null)
        {
            companionHP.value = (float)companionData.health / (float)companionData.maxHealth;
        }
    }
}

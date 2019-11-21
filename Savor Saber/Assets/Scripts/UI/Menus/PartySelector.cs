using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartySelector : MonoBehaviour
{

    private PlaySFX closeSFX;
    public GameObject[] memberButtons;
    private Commander partyCommander;
    [HideInInspector]
    public GameObject newFruitant;
    // Start is called before the first frame update
    void Start()
    {
        closeSFX = GetComponent<PlaySFX>();
    }
    private void Awake()
    {
        //Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetParty(List<GameObject> partyList, Commander pc)
    {
        partyCommander = pc;
        for(int i = 0; i < memberButtons.Length && i < partyList.Count; i++)
        {
            //set sprites
            SpriteRenderer partySprite = partyList[i].GetComponent<SpriteRenderer>();
            memberButtons[i].GetComponent<Image>().sprite = partySprite.sprite;

            //set target fruitant and commander
            memberButtons[i].GetComponent<PartySelectorFruitant>().targetFruitant = partyList[i];
            memberButtons[i].GetComponent<PartySelectorFruitant>().partyCommander = partyCommander;
        }
    }

    public void AddFruitant()
    {
        partyCommander.JoinTeam(newFruitant);
        CancelSelection();
    }

    public void CancelSelection()
    {
        closeSFX.Play(closeSFX.sfx);
        //Time.timeScale = 1;
        Destroy(this.gameObject);
    }
}

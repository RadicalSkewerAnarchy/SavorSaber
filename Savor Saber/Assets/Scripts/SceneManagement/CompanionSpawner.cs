using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionSpawner : MonoBehaviour
{
    public bool requireFlag = true;
    public string flagToCheck;
    public string valueToCheck;
    private GameObject player;
    private GameObject companion;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        companion = transform.GetChild(0).gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        if((requireFlag && FlagManager.GetFlag(flagToCheck) == valueToCheck) || !requireFlag)
        {
            Debug.Log("CompanionSpawner: Flag check passed or not required");
            //move morphed companion to player
            companion.transform.position = player.transform.position + new Vector3(-1.25f, 0, 0);
            Debug.Log("Moving companion to " + companion.transform.position);

            //feed the companion template the ingredientdata that player currently has
            PlayerData somaData = player.GetComponent<PlayerData>();
            FlavorInputManager companionFIM = companion.GetComponent<FlavorInputManager>();
            companionFIM.Feed(somaData.GetCurrentFormIngredient(), true, somaData);

            //Find the newly-morphed companion and check if Soma should be riding it

            PlayerController somaController = player.GetComponent<PlayerController>();
            if (somaController.loadRiding)
            {
                GameObject newCompanion = somaData.party[0];
                Debug.Log("Current companion: " + newCompanion);
                FruitantMount saddle = newCompanion.GetComponentInChildren<FruitantMount>();
                saddle.MountOnLoad();

                //reset temporary "riding during load" flag
                somaController.loadRiding = false;
            }

        }
        else
        {
            companion.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

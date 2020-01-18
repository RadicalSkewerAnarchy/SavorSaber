using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Feeder : MonoBehaviour
{
    private Inventory playerInventory;
    private Collider2D hitbox;
    private WaitForSeconds FeedingTimeDelay;
    private EntityController controller;
    private PlayerData somaData;

    private IngredientData[] ingredientArray;

    public bool feedingRecruits = true;
    public Commander partyCommander;
    public int maxPartySize = 3;
    public GameObject partySelectTemplate;
    public Canvas partySelectCanvas;

    // Start is called before the first frame update
    void Start()
    {
        hitbox = GetComponent<Collider2D>();
        playerInventory = GetComponentInParent<Inventory>();
        controller = GetComponentInParent<EntityController>();
        somaData = GetComponentInParent<PlayerData>();

        FeedingTimeDelay = new WaitForSeconds(0.25f);
        hitbox.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetButtonDown(Control.Interact))
        {
            StartCoroutine(Feed());
        }
        SetRotation();
    }

    void SetRotation()
    {
        if (controller.Direction == Direction.East)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else if (controller.Direction == Direction.West)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
        else if (controller.Direction == Direction.South)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }
        else if (controller.Direction == Direction.North)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }
        else if (controller.Direction == Direction.SouthEast)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -45));
        }
        else if (controller.Direction == Direction.SouthWest)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -135));
        }
        else if (controller.Direction == Direction.NorthEast)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 45));
        }
        else if (controller.Direction == Direction.NorthWest)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 135));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FlavorInputManager targetInput = collision.gameObject.GetComponent<FlavorInputManager>();
        if(targetInput != null)
        {
            ingredientArray = playerInventory.GetActiveSkewer().ToArray();
            if(ingredientArray.Length > 0)
            {
                Debug.Log("Feeding" + ingredientArray + "(Array length: " + ingredientArray.Length);
                targetInput.Feed(ingredientArray, true);
                playerInventory.ClearActiveSkewer();
            }
            ingredientArray = null;
        }

        if (feedingRecruits && collision.gameObject.tag == "Prey" && !somaData.party.Contains(collision.gameObject))
        {
            //If we're at max size, spawn the recruitment window
            if(somaData.party.Count >= maxPartySize)
            {
                GameObject partySelectObject = Instantiate(partySelectTemplate, Vector3.zero, Quaternion.identity,partySelectCanvas.transform);
                PartySelector partySelector = partySelectObject.GetComponent<PartySelector>();
                partySelectObject.transform.localPosition = Vector3.zero;

                //give the recruitment window its data
                partySelector.SetParty(somaData.party, partyCommander);
                partySelector.newFruitant = collision.gameObject;
                
            }
            else
            {
                partyCommander.JoinTeam(collision.gameObject, maxPartySize, true);
            }
            
        }
    }

    private IEnumerator Feed()
    {
        hitbox.enabled = true;
        yield return FeedingTimeDelay;
        hitbox.enabled = false;
        yield return null;
    }


}

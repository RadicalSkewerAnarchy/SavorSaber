using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{

    private EntityController controller;
    private Inventory inventory;
    private SpriteRenderer sr;
    private AttackRangedThrowSkewer attack;

    private bool active;

    [SerializeField]
    private bool toggleActivate = true;
    [SerializeField]
    private bool holdActivate = false; //this sucks, don't use it
    [SerializeField]
    private bool activateOnSkewer = false;
    [SerializeField]
    private bool chargeActivate = false;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponentInParent<EntityController>();
        inventory = GetComponentInParent<Inventory>();
        sr = GetComponent<SpriteRenderer>();
        attack = GetComponentInParent<AttackRangedThrowSkewer>();
    }

    // Update is called once per frame
    void Update()
    {
        SetRotation();
        SetActivation();
    }

    /// <summary>
    /// Set the rotation of the laser pointer according to player controller
    /// </summary>
    void SetRotation()
    {
        if (controller.Direction == Direction.West)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else if (controller.Direction == Direction.East)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
        else if (controller.Direction == Direction.North)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }
        else if (controller.Direction == Direction.South)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }
        else if (controller.Direction == Direction.NorthWest)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -45));
        }
        else if (controller.Direction == Direction.NorthEast)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -135));
        }
        else if (controller.Direction == Direction.SouthWest)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 45));
        }
        else if (controller.Direction == Direction.SouthEast)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 135));
        }
    }

    /// <summary>
    /// Check for and execute selected activation methods
    /// </summary>
    void SetActivation()
    {
        //Note: change to use InputManager once a proper control scheme is decided
        if (toggleActivate)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                ToggleActive();
            }
        }
        else if (holdActivate)
        {
            if (Input.GetKey(KeyCode.F))
            {
                active = true;
                sr.enabled = true;
            }
            else
            {
                active = false;
                sr.enabled = false;
            }
        }
        else if (activateOnSkewer)
        {
            if (!inventory.ActiveSkewerEmpty())
            {
                active = true;
                sr.enabled = true;
            }
            else
            {
                active = false;
                sr.enabled = false;
            }
        }
        else if (chargeActivate)
        {
            if (attack.currLevel > 0)
            {
                active = true;
                sr.enabled = true;
            }
            else
            {
                active = false;
                sr.enabled = false;
            }
        }
    }

    /// <summary>
    /// Toggle laser on and off if toggle activation is selected
    /// </summary>
    void ToggleActive()
    {
        if (active)
        {
            active = false;
            sr.enabled = false;
        }
        else
        {
            active = true;
            sr.enabled = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PressurePlate : PoweredObject
{
    private SpriteRenderer sr;
    private Collider2D[] overlappingObject = null;
    public PoweredObject[] targetObjects;

    public Sprite onSprite;
    public Sprite offSprite;

    private bool lastOn = false;
    private bool hasChangedStates = false;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        bool pressed = IsPressed();
        if (active && pressed && hasChangedStates)
        {
            //iterate over all target objects and turn them on if they aren't already
            foreach (PoweredObject targetObject in targetObjects)
            {
                if (!targetObject.active)
                    targetObject.TurnOn();
            }
            sr.sprite = onSprite;
            //Debug.Log("TURN ON");
        }
        else if (active && !pressed && hasChangedStates)
        {
            //iterate over all target objects and turn them off if they aren't already
            foreach (PoweredObject targetObject in targetObjects)
            {
                if (targetObject.active)
                    targetObject.ShutOff();
            }
            sr.sprite = offSprite;
            //Debug.Log("TURN OFF");
        }
    }

    /// <summary>
    /// Check if any valid objects are on the plate, and if the state has changed since last frame
    /// </summary>
    /// <returns></returns>
    bool IsPressed()
    {
        if (CheckValidCollisions())
        {
            //if it was not pressed last frame but is now, mark that we've changed states
            if (!lastOn)
            {
                hasChangedStates = true;
                lastOn = true;
                //Debug.Log("Valid collision detected,change of state detected");
            }
            else
            {
                hasChangedStates = false;
                //Debug.Log("Valid collision detected, no change of state");
            }
            return true;
        }
        else
        {
            //if it was pressed last frame but is not pressed now, mark that we've changed states
            if (lastOn)
            {
                hasChangedStates = true;
                lastOn = false;
                //Debug.Log("No collision detected ,change of state detected");
            }
            else
            {
                hasChangedStates = false;
                //Debug.Log("No collision detected, no change of state");
            }
            return false;
        }
    }

    /// <summary>
    /// Helper function to determine if any overlapping objects are valid
    /// </summary>
    /// <returns></returns>
    private bool CheckValidCollisions()
    {
        overlappingObject = Physics2D.OverlapBoxAll(transform.position - new Vector3(0, 0.35f), new Vector2(1.5f, 0.75f), 0f);

        if (overlappingObject.Length == 0)
            return false;
        else
        {
            foreach (Collider2D collider in overlappingObject)
            {
                if (collider.gameObject.layer == 0 || collider.gameObject.layer == 8)
                {
                    //Debug.Log("Pressure plate collision with " + collider.gameObject);
                    return true;
                }
            }
            return false;
        }
    }

}

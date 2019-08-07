using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PoweredObjectCharger : MonoBehaviour
{

    public bool active = true;
    public int damageBoostValue = 4;

    private PoweredObject targetObject;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //GetComponent<SpriteRenderer>().color = new Color(0,244,255,255);
    }

    public void Activate()
    {
        active = true;
        sr.color = new Color(0, 244, 255, 255);
        if (targetObject != null)
            targetObject.TurnOn();
    }

    public void Deactivate()
    {
        sr.color = Color.white;
        active = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if what you touched wasn't a valid powered object, we don't care about it
        PoweredObject testObject = collision.gameObject.GetComponent<PoweredObject>();
        if (testObject == null || !testObject.canBeSourCharged)
            return;
        else
        {
            //keep a reference to the valid powered object we're touching
            targetObject = testObject;
        }

        if (active)
        {
            targetObject.TurnOn();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(targetObject != null && collision.gameObject == targetObject.gameObject)
        {
            targetObject = null;
        }
    }
}

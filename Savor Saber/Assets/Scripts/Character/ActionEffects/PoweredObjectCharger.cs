using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PoweredObjectCharger : MonoBehaviour
{

    public bool active = true;
    private PoweredObject targetObject;
    // Start is called before the first frame update
    public int damageBoostValue = 4;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().color = new Color(0,244,255,255);
    }

    public void Activate()
    {
        active = true;
        if (targetObject != null)
            targetObject.TurnOn();
    }

    public void Deactivate()
    {
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

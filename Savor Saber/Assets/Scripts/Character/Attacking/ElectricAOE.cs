using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricAOE : MonoBehaviour
{

    private CharacterData characterData;
    private bool inAOE = false;
    public int damagePerTic = 1;
    public float damageRate = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Object in waterfall");
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Prey")
        {
            Debug.Log("Damaging valid target with electric field");
            characterData = collision.gameObject.GetComponent<CharacterData>();
            inAOE = true;
            DamageOverTime();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Prey")
        {
            StopCoroutine(ExecuteAfterSeconds());
            characterData = null;
            inAOE = false;           
        }
    }

    public void DamageOverTime()
    {
        bool killingBlow = false;
        if (inAOE)
        {
            //test to see if this tic will inflict a killing blow
            if (characterData.health - damagePerTic <= 0)
                killingBlow = true;

            characterData.DoDamage(damagePerTic);
            //Debug.Log("Health reduced to " + characterData.health + " by DoT effect");

            if (killingBlow)
                return;

            StartCoroutine(ExecuteAfterSeconds());
        }
        else
        {
            return;
        }

    }

    protected IEnumerator ExecuteAfterSeconds()
    {
        //things to happen before delay

        yield return new WaitForSeconds(damageRate);

        //things to happen after delay
        DamageOverTime();

        yield return null;
    }
}

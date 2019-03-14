using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterfallDamage : MonoBehaviour
{

    public bool pepperInCloud = false;
    public int damagePerTic = 5;
    CharacterData characterData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "GhostReaper")
        {
            characterData = collision.gameObject.GetComponent<CharacterData>();
            pepperInCloud = true;
            DamageOverTime();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "GhostReaper")
        {
            characterData = null;
            pepperInCloud = false;
            StopCoroutine(ExecuteAfterSeconds());
        }
    }

    public void DamageOverTime()
    {
        bool killingBlow = false;
        if (pepperInCloud)
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

        yield return new WaitForSeconds(0.25f);

        //things to happen after delay
        DamageOverTime();

        yield return null;
    }
}
}

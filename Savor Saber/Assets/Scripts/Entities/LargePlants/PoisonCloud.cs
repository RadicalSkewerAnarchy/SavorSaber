using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloud : MonoBehaviour
{
    PlayerData characterData;
    public float dotTicLength = 1;
    bool playerInCloud;
    public int damagePerTic = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            characterData = collision.gameObject.GetComponent<PlayerData>();
            playerInCloud = true;
            DamageOverTime();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            characterData = null;
            playerInCloud = false;
            StopCoroutine(ExecuteAfterSeconds());
        }
    }

    public void DamageOverTime()
    {
        bool killingBlow = false;
        if (playerInCloud)
        {
            //test to see if this tic will inflict a killing blow
            if (characterData.health - 1 <= 0)
                killingBlow = true;

            characterData.DoDamageIgnoreIFrames(damagePerTic);
            Debug.Log("Health reduced to " + characterData.health + " by DoT effect");

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

        yield return new WaitForSeconds(dotTicLength);

        //things to happen after delay
        DamageOverTime();

        yield return null;
    }
}

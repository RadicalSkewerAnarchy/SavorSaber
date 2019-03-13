using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloud : MonoBehaviour
{
    CharacterData characterData;
    public int dotTicLength;
    bool playerInCloud;
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
        if(collision.gameObject.tag == "Player")
        {
            characterData = collision.gameObject.GetComponent<CharacterData>();
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

            characterData.DoDamage(1);
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

        yield return new WaitForSeconds(0.25f);

        //things to happen after delay
        DamageOverTime();

        yield return null;
    }
}

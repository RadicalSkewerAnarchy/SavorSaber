using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricAOE : MonoBehaviour
{

    //private CharacterData characterData;
    private List<CharacterData> characterList;
    private bool inAOE = false;
    private bool active = true;
    private SpriteRenderer sr;
    private WaitForSeconds fieldDelay;

    public bool hurtPlayer = true;
    public bool hurtDrones = false;

    public int damagePerTic = 1;
    public float damageRate = 1f;
    /// <summary>
    /// The number of seconds that the field can be disabled for
    /// </summary>
    public int disruptiontime = 5;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        fieldDelay = new WaitForSeconds(disruptiontime);
        characterList = new List<CharacterData>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableForSeconds()
    {
        if(active)
            StartCoroutine(FieldTimer());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Object in waterfall");
        if (active && hurtPlayer && (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Prey"))
        {
            //Debug.Log("Damaging player-friendly target with electric field");
            //characterData = collision.gameObject.GetComponent<CharacterData>();
            characterList.Add(collision.gameObject.GetComponent<CharacterData>());
            inAOE = true;
            StopCoroutine(ExecuteAfterSeconds());
            DamageOverTime();
        }
        else if(active && hurtDrones && collision.gameObject.tag == "Predator")
        {
            //Debug.Log("Damaging valid target with electric field");
            //characterData = collision.gameObject.GetComponent<CharacterData>();
            characterList.Add(collision.gameObject.GetComponent<CharacterData>());
            inAOE = true;
            StopCoroutine(ExecuteAfterSeconds());
            DamageOverTime();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Prey")
        {
            StopCoroutine(ExecuteAfterSeconds());
            characterList.Remove(collision.gameObject.GetComponent<CharacterData>());
            inAOE = false;           
        }
        else if(collision.gameObject.tag == "Predator")
        {
            StopCoroutine(ExecuteAfterSeconds());
            characterList.Remove(collision.gameObject.GetComponent<CharacterData>());
            inAOE = false;
        }
    }

    public void DamageOverTime()
    {
        bool killingBlow = false;
        foreach(CharacterData characterData in characterList)
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

    }

    protected IEnumerator ExecuteAfterSeconds()
    {
        //things to happen before delay

        yield return new WaitForSeconds(damageRate);

        //things to happen after delay
        DamageOverTime();

        yield return null;
    }

    protected IEnumerator FieldTimer()
    {
        active = false;
        sr.enabled = false;

        yield return fieldDelay;

        active = true;
        sr.enabled = true;

    }
}

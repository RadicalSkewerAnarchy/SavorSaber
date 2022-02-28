using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricAOE : SkewerBonusEffect
{

    //private CharacterData characterData;
    private List<CharacterData> characterList;
    private bool enemiesInAOE = false;
    private bool active = true;
    private SpriteRenderer sr;
    private Light teslaLight;
    private WaitForSeconds fieldDelay;
    private Animator teslaAnimator;

    public bool hurtPlayer = true;
    public bool hurtDrones = false;

    public float damageRate = 1.5f;
    public int lifespan = -1; //negative values are infinite

    private int damageModifier = 1;

    public AudioClip shockSFX;
    private PlaySFX shockSFXPlayer;

    /// <summary>
    /// The number of seconds that the field can be disabled for
    /// </summary>
    public int disruptiontime = 5;

    public GameObject sparkTemplate;
    public bool overCharged = false;

    // Start is called before the first frame update
    void Start()
    {
        //sr = GetComponent<SpriteRenderer>();
        fieldDelay = new WaitForSeconds(disruptiontime);
        characterList = new List<CharacterData>();
        teslaAnimator = GetComponent<Animator>();
        teslaLight = GetComponentInChildren<Light>();
        shockSFXPlayer = GetComponent<PlaySFX>();

        if (active)
        {
            DamageOverTime();
        }
    }

    // Update is called once per frame
    void Update()
    {
        var powered = gameObject.GetComponent<PoweredObjectCharger>();
        if (powered != null)
        {
            if (powered.enabled == true)
            {
                damageModifier = powered.damageBoostValue;
            }
            else
            {
                damageModifier = 1;
            }
        }
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
            Instantiate(sparkTemplate, collision.gameObject.transform.position, Quaternion.identity);
            shockSFXPlayer.Play(shockSFX);
            if (!enemiesInAOE)
            {
                enemiesInAOE = true;
                StopAllCoroutines();
                DamageOverTime();

            }
        
        }
        else if(active && hurtDrones && collision.gameObject.tag == "Predator")
        {
            //Debug.Log("Damaging valid target with electric field");
            //characterData = collision.gameObject.GetComponent<CharacterData>();
            characterList.Add(collision.gameObject.GetComponent<CharacterData>());
            Instantiate(sparkTemplate, collision.gameObject.transform.position, Quaternion.identity);
            shockSFXPlayer.Play(shockSFX);
            if (!enemiesInAOE)
            {
                enemiesInAOE = true;
                StopAllCoroutines();
                DamageOverTime();

            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Prey")
        {
            StopCoroutine(ExecuteAfterSeconds());
            characterList.Remove(collision.gameObject.GetComponent<CharacterData>());
        }
        else if(collision.gameObject.tag == "Predator")
        {
            StopCoroutine(ExecuteAfterSeconds());
            characterList.Remove(collision.gameObject.GetComponent<CharacterData>());
        }

        if(characterList.Count == 0)
        {
            enemiesInAOE = false;
            StopAllCoroutines();
        }
    }

    public void DamageOverTime()
    {
        //Debug.Log("Entering DoT tic");
        bool killingBlow = false;
        foreach(CharacterData characterData in characterList)
        {
            if (characterData == null)
                continue;

            //test to see if this tic will inflict a killing blow
            //if (characterData.health - magnitude * damageModifier <= 0)
            //    killingBlow = true;

            killingBlow = characterData.DoDamage(magnitude * damageModifier, overCharged);

            //Debug.Log("Health reduced to " + characterData.health + " by DoT effect");

            if (killingBlow)
                continue;


        }
        StartCoroutine(ExecuteAfterSeconds());
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
        //sr.enabled = false;
        //sr.color = new Color(0, 0, 0, 0);
        teslaAnimator.Play("TeslaShutoff");
        teslaLight.color = Color.red;
        characterList.Clear();

        yield return fieldDelay;

        active = true;
        //sr.enabled = true;
        //sr.color = new Color(1, 1, 1, 1);
        teslaAnimator.Play("Tesla");
        teslaLight.color = new Color(0, 1, 0.869112f);

    }

    private IEnumerator SelfTerminateTimer()
    {
        if (lifespan < 0) yield return null;

        yield return new WaitForSeconds(lifespan);

        Destroy(this.gameObject);       
    }
}

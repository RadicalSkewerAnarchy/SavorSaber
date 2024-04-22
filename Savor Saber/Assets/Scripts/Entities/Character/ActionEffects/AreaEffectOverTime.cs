using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEffectOverTime : MonoBehaviour
{

    //private CharacterData characterData;
    protected List<CharacterData> characterList;
    protected bool targetsInAOE = false;
    protected bool active = true;
    protected SpriteRenderer sr;
    protected Light effectLight;
    protected WaitForSeconds fieldDelay;
    protected Animator effectAnimator;

    public bool affectPlayer = true;
    public bool affectDrones = false;

    public float effectRate = 1.5f; 
    public int lifespan = -1; //negative values are infinite

    protected int damageModifier = 1;
    protected int magnitude = 1;
    public AudioClip SFX;
    protected PlaySFX SFXPlayer;

    /// <summary>
    /// The number of seconds that the field can be disabled for
    /// </summary>
    public int disruptionTime = 5;

    public GameObject vfxTemplate;
    public bool overCharged = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //sr = GetComponent<SpriteRenderer>();
        fieldDelay = new WaitForSeconds(disruptionTime);
        characterList = new List<CharacterData>();
        effectAnimator = GetComponent<Animator>();
        effectLight = GetComponentInChildren<Light>();
        SFXPlayer = GetComponent<PlaySFX>();

        if (active)
        {
            EffectOverTime();
        }
    }

    // Update is called once per frame
    protected virtual void Update()
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
            StartCoroutine(DisableTimer());
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Object in waterfall");
        if (active && affectPlayer && (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Prey"))
        {
            characterList.Add(collision.gameObject.GetComponent<CharacterData>());
            if (!targetsInAOE)
            {
                targetsInAOE = true;
                StopAllCoroutines();
                EffectOverTime();
            }
            
        }
        else if(active && affectDrones && collision.gameObject.tag == "Predator")
        {
            //Debug.Log("Damaging valid target with electric field");
            //characterData = collision.gameObject.GetComponent<CharacterData>();
            characterList.Add(collision.gameObject.GetComponent<CharacterData>());
            EffectVFX(collision);
            if (!targetsInAOE)
            {
                targetsInAOE = true;
                StopAllCoroutines();
                EffectOverTime();

            }
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
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
            targetsInAOE = false;
            StopAllCoroutines();
        }
    }

    public void EffectOverTime()
    {
        //Debug.Log("Entering Effect over Time tic");
        Effect();
        StartCoroutine(ExecuteAfterSeconds());
    }

    //Classes that extend this template will put their effects here
    protected virtual void Effect()
    {
        // do something to the targets
    }

    //Classes that extend this template will put their effect vfx here
    protected virtual void EffectVFX(Collider2D collision)
    {
        // play some fx
    }

    protected IEnumerator ExecuteAfterSeconds()
    {
        //things to happen before delay

        yield return new WaitForSeconds(effectRate);

        //things to happen after delay
        EffectOverTime();

        yield return null;
    }


    //Disable the AOE field for a time
    protected IEnumerator DisableTimer()
    {
        active = false;
        //sr.enabled = false;
        //sr.color = new Color(0, 0, 0, 0);
        effectAnimator.Play("AOEShutoff");
        effectLight.color = Color.red;
        characterList.Clear();

        yield return fieldDelay;

        active = true;
        //sr.enabled = true;
        //sr.color = new Color(1, 1, 1, 1);
        effectAnimator.Play("AOE");
        effectLight.color = new Color(0, 1, 0.869112f);

    }

    protected IEnumerator SelfTerminateTimer()
    {
        if (lifespan < 0) yield return null;

        yield return new WaitForSeconds(lifespan);

        Destroy(this.gameObject);       
    }
}

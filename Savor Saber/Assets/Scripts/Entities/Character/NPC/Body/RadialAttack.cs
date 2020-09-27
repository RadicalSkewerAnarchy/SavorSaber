using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialAttack : MonoBehaviour
{
    public bool startActive = true;
    public int particleDamage = 2;
    public int cooldown = 10;
    //number of quarter-second intervals it takes to cast
    public int castTime = 40;
    public Slider castSlider;
    public AudioClip beginChargeSFX;
    public AudioClip criticalChargeSFX;
    public AudioClip shootSFX;


    private WaitForSeconds Cooldown;
    private WaitForSeconds CastTic;
    private ParticleSystem shooter;
    private PlaySFX sfxPlayer;
    private bool active = false;
    // Start is called before the first frame update
    void Start()
    {
        Cooldown = new WaitForSeconds(cooldown);
        CastTic = new WaitForSeconds(0.25f);
        shooter = GetComponent<ParticleSystem>();
        sfxPlayer = GetComponent<PlaySFX>();
        

        if (startActive)
        {
            active = true;
            StartCoroutine(FireLoop());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        if (!active)
        {
            active = true;
            StartCoroutine(FireLoop());
        }
        
    }

    public void Deactivate()
    {
        StopAllCoroutines();
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("PARTICLE COLLISION");
        if(other.tag == "Player" || other.tag == "Prey")
        {
            CharacterData data = other.GetComponent<CharacterData>();
            data.DoDamage(particleDamage); 
        }
    }

    private IEnumerator FireLoop()
    {
        
        castSlider.gameObject.SetActive(false);
        //Debug.Log("Entering fire loop");
        yield return Cooldown;

        sfxPlayer.Play(beginChargeSFX);
        //Debug.Log("Cooldown elapsed, beginning attack");
        castSlider.gameObject.SetActive(true);
        yield return Cast(castTime);
    }

    private IEnumerator Cast(int numTics)
    {
        //Debug.Log("Charge time remaining: " + numTics);
        castSlider.value = ((float)(castTime - numTics) / castTime);

        if(numTics > 0)
        {
            if (numTics == 8)
                sfxPlayer.Play(criticalChargeSFX);
            yield return CastTic;
            yield return Cast(numTics - 1);
        }
        else
        {
            //Debug.Log("Firing");
            shooter.Play();
            sfxPlayer.Play(shootSFX);
            yield return FireLoop();
        }
        yield return null;
    }

}

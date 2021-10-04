using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialAttack : MonoBehaviour
{
    public enum PolarityMode
    {
        Static,
        Alternate,
        Randomize
    }

    public bool startActive = true;
    public int particleDamage = 2;
    public int cooldown = 10;
    //number of quarter-second intervals it takes to cast
    public int castTime = 40;
    public int criticalThreshold = 3;
    public Slider castSlider;
    public AudioClip beginChargeSFX;
    public AudioClip criticalChargeSFX;
    public AudioClip shootSFX;

    public ParticleSystem[] secondaryShooters;

    private WaitForSeconds Cooldown;
    private WaitForSeconds CastTic;
    private ParticleSystem shooter;
    private ParticleSystemRenderer shooterRenderer;
    private PlaySFX sfxPlayer;
    private bool active = false;


    [Header("Polarity properties")]
    public bool swapPolarityOnShoot = false;
    public PolarityMode polarityMode = PolarityMode.Static;
    public Material redMaterial;
    public Material blueMaterial;
    public Material whiteMaterial;
    public Sprite redSprite;
    public Sprite blueSprite;
    public Sprite whiteSprite;
    public SpriteRenderer[] spritesToSwap;
    public Image castBarFillColor;

    private bool isFirstShot = true;

    public enum BulletColors
    {
        Red,
        Blue,
        White,
    }

    public BulletColors currentColor = BulletColors.White;
    private BulletColors randomizedColor;

    // Start is called before the first frame update
    void Start()
    {
        int RedLayer = LayerMask.GetMask("Red");
        int BlueLayer = LayerMask.GetMask("Blue");

        Cooldown = new WaitForSeconds(cooldown);
        CastTic = new WaitForSeconds(0.25f);
        shooter = GetComponent<ParticleSystem>();
        shooterRenderer = GetComponent <ParticleSystemRenderer>();
        sfxPlayer = GetComponent<PlaySFX>();
        castTime *= 4; //get it into quarter seconds
        criticalThreshold *= 4;
        //criticalThreshold = (castTime > 12) ? 12 : (castTime / 12);
        Debug.Log("Critical threshold: " + criticalThreshold);
        if (startActive)
        {
            active = true;
            StartCoroutine(FireLoop());
        }

        ChangePolarityTelegraph(currentColor);
        ChangePolarityBullet(currentColor);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangePolarityTelegraph(RadialAttack.BulletColors color)
    {
        var collision = shooter.collision;
        LayerMask currentMask = collision.collidesWith;

        switch (color)
        {
            case BulletColors.Red:
                    //change telegraph color
                foreach(SpriteRenderer sr in spritesToSwap)
                    sr.sprite = redSprite;
                castBarFillColor.color = Color.red;
                break;
            case BulletColors.Blue:
                    //change telegraph color
                foreach (SpriteRenderer sr in spritesToSwap)
                    sr.sprite = blueSprite;
                castBarFillColor.color = new Color(0,0.94f,1);
                break;
            case BulletColors.White:
                    //change telegraph color
                foreach (SpriteRenderer sr in spritesToSwap)
                    sr.sprite = whiteSprite;
                castBarFillColor.color = Color.white;
                break;
            default:
                return;
        }
    }

    //since the particle changes affect existing particles, don't do these parts until after they've fired
    private void ChangePolarityBullet(BulletColors color)
    {
        var collision = shooter.collision;
        LayerMask currentMask = collision.collidesWith;

        switch (color)
        {
            case BulletColors.Red:
                //change shot color and collision
                Debug.Log("SpreadShot: Setting attack color to red");
                currentMask = collision.collidesWith = currentMask | LayerMask.GetMask("Red");
                currentMask = collision.collidesWith = currentMask ^ LayerMask.GetMask("Blue");
                shooterRenderer.material = redMaterial;
                currentColor = BulletColors.Red;
                break;
            case BulletColors.Blue:
                Debug.Log("SpreadShot: Setting attack color to blue");
                currentMask = collision.collidesWith = currentMask | LayerMask.GetMask("Blue");
                currentMask = collision.collidesWith = currentMask ^ LayerMask.GetMask("Red");
                shooterRenderer.material = blueMaterial;
                currentColor = BulletColors.Blue;
                break;
            case BulletColors.White:
                Debug.Log("SpreadShot: Setting attack color to white");
                currentMask = collision.collidesWith = currentMask | LayerMask.GetMask("Red");
                currentMask = collision.collidesWith = currentMask | LayerMask.GetMask("Blue");
                shooterRenderer.material = whiteMaterial;
                currentColor = BulletColors.White;
                break;
            default:
                return;
        }
    }

    private IEnumerator FireLoop()
    {
        
        castSlider.gameObject.SetActive(false);

        //Wait for the cooldown timer before starting to charge again
        yield return Cooldown;

        if(beginChargeSFX != null)
            sfxPlayer.Play(beginChargeSFX);

        //Debug.Log("Cooldown elapsed, beginning attack");
        castSlider.gameObject.SetActive(true);
        //preform late bullet polarity swaps before the next shot, if necessary, but not on the first shot because that breaks things
        if (polarityMode == PolarityMode.Alternate && !isFirstShot)
        {
            if (currentColor == BulletColors.Red)
            {
                ChangePolarityBullet(BulletColors.Blue);
            }
            else if (currentColor == BulletColors.Blue)
            {
                ChangePolarityBullet(BulletColors.Red);
            }
        }
        else if(polarityMode == PolarityMode.Randomize && !isFirstShot)
        {
            ChangePolarityBullet(randomizedColor);
        }
        if (isFirstShot)
            isFirstShot = false;
        yield return Cast(castTime); 
    }

    private IEnumerator Cast(int numTics)
    {
        //Debug.Log("Charge time remaining: " + numTics);
        castSlider.value = ((float)(castTime - numTics) / castTime);

        if(numTics > 0)
        {
            if (numTics == criticalThreshold && criticalChargeSFX != null)
                sfxPlayer.Play(criticalChargeSFX);
            yield return CastTic;
            yield return Cast(numTics - 1);
        }
        else
        {
            //Debug.Log("Firing");
            shooter.Play();
            sfxPlayer.Play(shootSFX);

            //swap telegraph polarity after shooting
            if (polarityMode == PolarityMode.Alternate)
            {
                if (currentColor == BulletColors.Red)
                {
                    ChangePolarityTelegraph(BulletColors.Blue);
                }
                else if (currentColor == BulletColors.Blue)
                {
                    ChangePolarityTelegraph(BulletColors.Red);
                }
            }
            else if(polarityMode == PolarityMode.Randomize)
            {
                int rng = Random.Range(0, 2);
                Debug.Log("Particle attack: Randomize = " + rng);
                if (rng == 0)
                {
                    ChangePolarityTelegraph(BulletColors.Red);
                    randomizedColor = BulletColors.Red;
                    Debug.Log("Setting particle attack " + this.gameObject.name + "to Red");
                }
                else
                {
                    ChangePolarityTelegraph(BulletColors.Blue);
                    randomizedColor = BulletColors.Blue;
                    Debug.Log("Setting particle attack " + this.gameObject.name + "to Red");
                }
                    

            }
            yield return FireLoop();
        }
        yield return null;
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
        if (active)
        {
            active = false;
            StopAllCoroutines();
        }

    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(gameObject.name + ": PARTICLE COLLISION WITH " + other);
        if (other.tag == "Player" || other.tag == "Prey")
        {
            Debug.Log("Player hit by particle from " + gameObject.name + ", applying damage");
            CharacterData data = other.GetComponent<CharacterData>();
            data.DoDamage(particleDamage);
        }
    }

}

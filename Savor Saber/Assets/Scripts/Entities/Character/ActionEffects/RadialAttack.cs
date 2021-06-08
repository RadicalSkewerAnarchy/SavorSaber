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
    private ParticleSystemRenderer shooterRenderer;
    private PlaySFX sfxPlayer;
    private bool active = false;
    private int criticalThreshold;
    [Header("Polarity properties")]
    public bool swapPolarityOnShoot = false;
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
        castTime = castTime * 4; //get it into quarter seconds
        criticalThreshold = (castTime > 16) ? (int)castTime / 4 : 4;

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
                Debug.Log("SpreadShot: Setting telegraph color to red");
                //currentMask = collision.collidesWith = currentMask | LayerMask.GetMask("Red");
                //currentMask = collision.collidesWith = currentMask ^ LayerMask.GetMask("Blue");
                    //change telegraph color
                foreach(SpriteRenderer sr in spritesToSwap)
                    sr.sprite = redSprite;
                castBarFillColor.color = Color.red;
                //currentColor = BulletColors.Red;
                //shooterRenderer.material = redMaterial;
                break;
            case BulletColors.Blue:
                Debug.Log("SpreadShot: Setting telegraph color to blue");
                //currentMask = collision.collidesWith = currentMask | LayerMask.GetMask("Blue");
                //currentMask = collision.collidesWith = currentMask ^ LayerMask.GetMask("Red");
                    //change telegraph color
                foreach (SpriteRenderer sr in spritesToSwap)
                    sr.sprite = blueSprite;
                castBarFillColor.color = new Color(0,0.94f,1);
                //currentColor = BulletColors.Blue;
                //shooterRenderer.material = blueMaterial;
                break;
            case BulletColors.White:
                Debug.Log("SpreadShot: Setting telegraph color to white");
                //currentMask = collision.collidesWith = currentMask | LayerMask.GetMask("Red");
                //currentMask = collision.collidesWith = currentMask | LayerMask.GetMask("Blue");
                    //change telegraph color
                foreach (SpriteRenderer sr in spritesToSwap)
                    sr.sprite = whiteSprite;
                castBarFillColor.color = Color.white;
                //currentColor = BulletColors.White;
                //shooterRenderer.material = whiteMaterial;
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
        //Debug.Log("PARTICLE COLLISION");
        if(other.tag == "Player" || other.tag == "Prey")
        {
            CharacterData data = other.GetComponent<CharacterData>();
            data.DoDamage(particleDamage); 
        }
    }

    private IEnumerator FireLoop()
    {
        
        castSlider.gameObject.SetActive(false);

        //Wait for the cooldown timer before starting to charge again
        yield return Cooldown;

        sfxPlayer.Play(beginChargeSFX);

        //Debug.Log("Cooldown elapsed, beginning attack");
        castSlider.gameObject.SetActive(true);
        //preform late polarity swaps before the next shot, if necessary, but not on the first shot because that breaks things
        if (swapPolarityOnShoot && !isFirstShot)
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
            if (numTics == criticalThreshold)
                sfxPlayer.Play(criticalChargeSFX);
            yield return CastTic;
            yield return Cast(numTics - 1);
        }
        else
        {
            //Debug.Log("Firing");
            shooter.Play();
            sfxPlayer.Play(shootSFX);

            //swap polarity after shooting
            if (swapPolarityOnShoot)
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
            yield return FireLoop();
        }
        yield return null;
    }

}

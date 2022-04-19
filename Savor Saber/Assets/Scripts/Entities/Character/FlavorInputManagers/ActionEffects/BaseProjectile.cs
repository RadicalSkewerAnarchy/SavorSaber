using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Use to designate a GameObject as a projectile launched by a RangedAttack script
/// Should be used as a base class, with derived classes for specific entities' attacks
/// </summary>
[RequireComponent(typeof(CapsuleCollider2D))]
public class BaseProjectile : MonoBehaviour
{

    #region fields
    /// <summary>
    /// Should this projectile hurt certain factions?
    /// </summary>
    public string[] tagsToIgnore;

    public bool overcharged = false;
    [HideInInspector]
    public CharacterData myCharData;

    /// <summary>
    /// A prefab to be instantiated when the projectile is terminated
    /// </summary>
    public GameObject dropItem; 

    /// <summary>
    /// How fast the projectile travels
    /// </summary>
    public float projectileSpeed;

    /// <summary>
    /// How long the projectile is
    /// </summary>
    public float projectileLength;

    /// <summary>
    /// How wide the projectile is
    /// </summary>
    public float projectileWidth;

    /// <summary>
    /// How much damage the projectile does, if any
    /// </summary>
    public float projectileDamage;

    /// <summary>
    /// How much the projectile should knock back its target
    /// </summary>
    public float projectileKnockback;

    /// <summary>
    /// Should this projectile penetrate targets?
    /// </summary>
    public bool penetrateTargets = false;

    /// <summary>
    /// The thing that created this attack
    /// </summary>
    public GameObject attacker = null;

    /// <summary>
    /// what is the AOE of this projectile on impact?
    /// </summary>
    public float attackRadius = 0.5f;

    /// <summary>
    /// How far this projectile should travel before self-terminating.
    /// Range of 0 will fly FOREVER 
    /// </summary>
    [Range(0, 50)]
    public float range;

    [System.NonSerialized]
    public float projectileRotation;

    /// <summary>
    /// Recipe data for any effects to be applied. Projectile gets this from
    /// the attack script. 
    /// </summary>
    [System.NonSerialized]
    public RecipeData effectRecipeData = null;

    /// <summary>
    /// how much of each flavor is present on the skewer
    /// </summary>
    public Dictionary<RecipeData.Flavors, int> flavorCountDictionary;
    public Dictionary<string, int> ingredientCountDictionary;
    [HideInInspector]
    public IngredientData[] ingredientArray;

    /// <summary>
    /// Direction and rotation fields
    /// </summary>
    [System.NonSerialized]
    public Direction direction;
    [System.NonSerialized]
    public Vector2 directionVector;
    [System.NonSerialized]
    public CapsuleCollider2D projectileCollider;

    protected Vector2 spawnPosition;
#endregion

    #region Bonus Effect Fields

    public GameObject dropTemplate; //template for dropping food item if the skewer misses
    protected GameObject bonusEffectTemplate = null; //for any additional effects to be spawned, e.g. from Trust Buffs
    protected int bonusEffectMagnitude = 1;
    protected bool dropping = false; //prevents duplicate drops
    public bool spawnBonusEffectOnMiss;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        projectileCollider = GetComponent<CapsuleCollider2D>();
        projectileCollider.size = new Vector2(projectileLength, projectileWidth);

        // set projectile velocity vector
        SetGeometry();

        //as a safety measure, if range is infinite, do NOT allow penetrating targets
        if (range == 0)
            penetrateTargets = false;

        spawnPosition = transform.position;
        //Debug.Log("Spawn = " + spawnPosition);
        //Debug.Log("Spawn position: " + spawnPosition);
        //Debug.Log(directionVector);
        //myCharData = GetComponent<CharacterData>();

        //Debug.Log("Projectile spawned with direction vector " + directionVector);

    }

    // Update is called once per frame
    void Update()
    {
        MoveProjectile();
    }

    protected void MoveProjectile()
    {
        transform.Translate(directionVector * projectileSpeed * Time.deltaTime, Space.World);

        if (Vector2.Distance(transform.position, spawnPosition) >= range && range > 0)
        {
            Destroy(this.gameObject);
        }
    }

    protected void SetGeometry()
    {
        if (direction == Direction.East)
        {
            projectileCollider.size = new Vector2(projectileLength, projectileWidth);
        }
        else if (direction == Direction.West)
        {
            projectileCollider.size = new Vector2(projectileLength, projectileWidth);
        }
        else if (direction == Direction.North)
        {
            projectileCollider.size = new Vector2(projectileLength, projectileWidth);
        }
        else if (direction == Direction.South)
        {
            projectileCollider.size = new Vector2(projectileLength, projectileWidth);
        }
        else if (direction == Direction.NorthWest)
        {
            projectileCollider.size = new Vector2(projectileLength, projectileWidth);
        }
        else if (direction == Direction.NorthEast)
        {
            projectileCollider.size = new Vector2(projectileLength, projectileWidth);
        }
        else if (direction == Direction.SouthWest)
        {
            projectileCollider.size = new Vector2(projectileLength, projectileWidth);
        }
        else if (direction == Direction.SouthEast)
        {
            projectileCollider.size = new Vector2(projectileLength, projectileWidth);
        }
    }

    public void SetBonusEffect(GameObject bonus, int mag)
    {
        bonusEffectTemplate = bonus;
        bonusEffectMagnitude = mag;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetType() == typeof(BoxCollider2D))
        {
            Physics2D.IgnoreCollision(collision, gameObject.GetComponent<CapsuleCollider2D>());
        }
        GameObject go = collision.gameObject;

        #region Special cases to ignore
        //ignore skewerableobjects
        if (go.tag == "SkewerableObject")
            return;
        //ignore what launched me
        if (go == attacker)
            return;
        //ignore specified target classes
        foreach(string tag in tagsToIgnore)
        {
            if (go.tag == tag)
                return;
        }

        #endregion

        //if you have a drop item, drop it now
        if (dropItem != null)
            Instantiate(dropItem, transform.position, Quaternion.identity);

        //if what you hit is a character
        CharacterData characterData = go.GetComponent<CharacterData>();
        if (characterData != null)
        {
            // Make projectiles go through a player who is currently in I-frames
            if(go.tag == "Player")
            {
                var playerData = characterData as PlayerData;
                if (playerData != null && playerData.Invincible)
                    return;
            }
            if(bonusEffectTemplate != null && !dropping)
            {
                GameObject bonus = Instantiate(bonusEffectTemplate, transform.position, Quaternion.identity);
                SkewerBonusEffect effect = bonus.GetComponent<SkewerBonusEffect>();
                if(effect != null)
                    effect.SetTarget(collision.gameObject, bonusEffectMagnitude);
                dropping = true;
            }
            if (characterData.DoDamage((int)projectileDamage, overcharged) && myCharData != null)
                myCharData.entitiesKilled += 1;
            if (!penetrateTargets)
                Destroy(this.gameObject);
        }
        //if what you hit is destructible environment
        else if (go.tag == "ThrowThrough")
        {
            DestructableEnvironment envData = go.GetComponent<DestructableEnvironment>();
            //myCharData.damageDealt += (int)projectileDamage;
            //Debug.Log("Dealing DMG");
            if (envData != null)
            {
                envData.Health -= (int)Mathf.Max(projectileDamage, 1);
                //if (!penetrateTargets)
                    //Destroy(this.gameObject);
            }
        }
        //if what you hit is terrain
        else if(go.tag == "Terrain" || go.tag == "Scenery")
        {
            //this should solve projectiles passing through terrain. 
            //unfortunately, requires all
            Destroy(this.gameObject);
            return;
        }
    }
}

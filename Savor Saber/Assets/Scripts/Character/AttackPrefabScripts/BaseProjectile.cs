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
    /// <summary>
    /// Should this projectile hurt certain factions?
    /// </summary>
    public bool hurtPlayer = true;
    public bool hurtDrones = true;

    CharacterData myCharData;

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
        myCharData = GetComponent<CharacterData>();

        Debug.Log("Projectile spawned with direction vector " + directionVector);

    }

    // Update is called once per frame
    void Update()
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

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetType() == typeof(BoxCollider2D))
        {
            Physics2D.IgnoreCollision(collision, gameObject.GetComponent<CapsuleCollider2D>());
        }
        GameObject go = collision.gameObject;
        // Debug.Log("Projectile trigger entered");
        if (go.tag == "SkewerableObject")
            return;
        if (go == attacker)
            return;
        if (dropItem != null)
            Instantiate(dropItem, transform.position, Quaternion.identity);
        if ((go.tag == "Player" || go.tag =="Prey") && !hurtPlayer)
            return;
        if (go.tag == "Predator" && !hurtDrones)
            return;

        CharacterData characterData = go.GetComponent<CharacterData>();
        if (characterData != null)
        {
            //myCharData.damageDealt += (int)projectileDamage;
            //Debug.Log("Dealing DMG");
            if (characterData.DoDamage((int)projectileDamage) && myCharData != null)
                myCharData.entitiesKilled += 1;
            if (!penetrateTargets)
                Destroy(this.gameObject);
        }
        else if (go.tag == "ThrowThrough")
        {
            DestructableEnvironment envData = go.GetComponent<DestructableEnvironment>();
            //myCharData.damageDealt += (int)projectileDamage;
            //Debug.Log("Dealing DMG");
            if (envData != null)
            {
                envData.health -= (int)Mathf.Max(projectileDamage, 1);
                envData.Destroy();
                if (!penetrateTargets)
                    Destroy(this.gameObject);
            }
        }
    }
}

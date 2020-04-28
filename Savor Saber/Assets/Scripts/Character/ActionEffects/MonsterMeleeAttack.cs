using UnityEngine;

public class MonsterMeleeAttack : BaseMeleeAttack
{
    public AudioClip damageSFX;
    private PlaySFXRandPitch sfxPlayer;
    public MonsterChecks monsterChecks;
    public AIData myCharData;

    // Start is called before the first frame update
    void Start()
    {
        sfxPlayer = GetComponent<PlaySFXRandPitch>();
        monsterChecks = gameObject.GetComponentInParent<MonsterChecks>();
        if(monsterChecks == null)
        {
            Debug.Log("MonsterChecks is null");
        }
        //var circleCollider = GetComponent<CircleCollider2D>();
        myCharData = myAttacker.GetComponent<AIData>();
        //Physics2D.IgnoreCollision(circleCollider, GetComponent<CapsuleCollider2D>());
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("THERE IS NO CHARACTERCOLLISION");
        //monsterChecks.Enemies.Clear();
        GameObject g = collision.gameObject;
        string t = g.tag;

        if (t == "Predator")
        {
            if (damageSFX != null)
                sfxPlayer.PlayRandPitch(damageSFX);
            CharacterData charData = g.GetComponent<CharacterData>();
            //Debug.Log("THERE IS CHARACTER COLLISION");
            if (charData != null)
            {
                if (myCharData != null)
                {
                    charData.DoDamage((int)meleeDamage, myCharData.GetOvercharged());
                }
                else
                {
                    Debug.Log("No attacker");
                }
            }
        }
        
        if (g.GetComponent<DestructableEnvironment>() != null)
        {
            DestructableEnvironment de  = g.GetComponent<DestructableEnvironment>();
            de.Health -= ((int)meleeDamage);
        }
    }
}

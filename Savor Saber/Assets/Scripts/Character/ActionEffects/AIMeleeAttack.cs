using UnityEngine;

public class AIMeleeAttack : BaseMeleeAttack
{
    public AudioClip damageSFX;
    private PlaySFXRandPitch sfxPlayer;
    public MonsterChecks monsterChecks;
    public AICharacterData myCharData;

    // Start is called before the first frame update
    void Start()
    {
        sfxPlayer = GetComponent<PlaySFXRandPitch>();
        myCharData = myAttacker.GetComponent<AICharacterData>();
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("THERE IS NO CHARACTERCOLLISION");
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
                    charData.DoDamage((int)meleeDamage, myCharData.health > myCharData.maxHealth);
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

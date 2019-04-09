using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Respawner))]
public class PlayerData : CharacterData
{
    public bool Invincible { get; private set; }
    private const float flickerTime = 0.075f;
    private const float timeConst = 1.25f;
    private SpriteRenderer sp;
    private Respawner res;

    private void Awake()
    {
        InitializeCharacterData();
        sp = GetComponent<SpriteRenderer>();
        res = GetComponent<Respawner>();
    }

    public override bool DoDamage(int damage)
    {
        bool dead = false;
        if (damage > 0)
        {
            if (Invincible)
                return false;
            health -= damage;
            //only play damage SFX if it was not a killing blow so sounds don't overlap
            if (health > 0)
            {
                var deathSoundObj = Instantiate(sfxPlayer, transform.position, transform.rotation);
                deathSoundObj.GetComponent<PlayAndDestroy>().Play(damageSFX);
                Invincible = true;
                StartCoroutine(IFrames(damage * timeConst));
            }
            else if (!res.Respawning)
            {
                dead = true;
                var deathSoundObj = Instantiate(sfxPlayer, transform.position, transform.rotation);
                deathSoundObj.GetComponent<PlayAndDestroy>().Play(deathSFX);
                res.Respawn();
            }
        }
        return dead;
    }

    private IEnumerator IFrames(float time)
    {
        Timer t = new Timer(time);
        bool on = false;
        while (t.Update(flickerTime))
        {           
            sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, on ? 1 : 0.5f);
            on = !on;
            Debug.Log("Iframes");
            yield return new WaitForSeconds(flickerTime);
        } 
        sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 1);
        Invincible = false;
    }
}

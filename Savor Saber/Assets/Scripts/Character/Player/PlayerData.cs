using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Respawner))]
public class PlayerData : CharacterData
{
    [Header("Player-specific fields")]
    public AudioClip lowHealthSFX;
    private PlaySFX altSFXPlayer;
    public bool Invincible { get; set; }
    private const float flickerTime = 0.075f;
    private const float timeConst = 1.25f;
    private SpriteRenderer sp;
    private Respawner res;
    public List<GameObject> party = new List<GameObject>();
    public int lowHealthThreshhold = 2;
    private TrustMeter trust;

    public PartyUIManager partyUI;
    private void Awake()
    {
        InitializeCharacterData();
        sp = GetComponent<SpriteRenderer>();
        res = GetComponent<Respawner>();
        altSFXPlayer = GetComponent<PlaySFX>();
        trust = GetComponent<TrustMeter>();
    }

    public override bool DoDamage(int damage, bool overcharged = false)
    {
        bool dead = false;
        CameraController.instance.Shake(0.01f, 0.02f, 0.1f);
        if (damage > 0)
        {
            if (Invincible)
                return false;
            health -= damage;
            //only play damage SFX if it was not a killing blow so sounds don't overlap
            if (health > 0)
            {
                var deathSoundObj = Instantiate(sfxPlayer, transform.position, transform.rotation);
                altSFXPlayer.Play(damageSFX);

                //play low hp warning if you're at low health
                if(health == lowHealthThreshhold)
                    altSFXPlayer.Play(lowHealthSFX);

                Invincible = true;
                StartCoroutine(IFrames(damage * timeConst));
            }
            else if (!res.Respawning)
            {
                dead = true;
                var deathSoundObj = Instantiate(sfxPlayer, transform.position, transform.rotation);
                altSFXPlayer.Play(deathSFX);
                res.Respawn();
            }
        }
        return dead;
    }

    public bool DoDamageIgnoreIFrames(int damage)
    {
        bool dead = false;
        if (damage > 0)
        {
            health -= damage;
            //only play damage SFX if it was not a killing blow so sounds don't overlap
            if (health > 0)
            {
                var deathSoundObj = Instantiate(sfxPlayer, transform.position, transform.rotation);
                altSFXPlayer.Play(damageSFX);

                //play low hp warning if you're at low health
                if (health == lowHealthThreshhold)
                    altSFXPlayer.Play(lowHealthSFX);
            }
            else if (!res.Respawning)
            {
                dead = true;
                var deathSoundObj = Instantiate(sfxPlayer, transform.position, transform.rotation);
                altSFXPlayer.Play(deathSFX);
                res.Respawn();
            }
        }
        return dead;
    }

    private IEnumerator IFrames(float time)
    {
        Timer t = new Timer(time);
        bool on = false;
        while (!t.Update(flickerTime))
        {           
            sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, on ? 1 : 0.5f);
            on = !on;
            //Debug.Log("Iframes");
            yield return new WaitForSeconds(flickerTime);
        } 
        sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 1);
        Invincible = false;
    }


    #region Party Manipulation
    /// <summary>
    /// Add any fruitant to the player's party.
    /// </summary>
    /// <param name="member">the fruitant</param>
    /// <param name="partysize">size to fit to</param>
    /// <param name="partyoverride">remove fruitants in order to fit</param>
    public void JoinTeam(GameObject member, int partysize = 3, bool partyoverride = false)
    {
        AIData Brain;
        // if subject still exists
        if (member != null)
        {
            // get brain
            Brain = member.GetComponent<AIData>();
            if (Brain != null)
            {
                //if (pd == null) Debug.Log("Player Data is null!!!!");
                if (party.Contains(member))
                {
                    // do nothing
                    return;
                }
                else if (partyoverride)
                {
                    AddMember(member, Brain);
                }
                else if (party.Count >= partysize)
                {
                    // remove if over size
                    AddMember(member, Brain);
                    while (party.Count > partysize)
                    {
                        LeaveTeam(party[0]);
                    }
                }
                else if (party.Count < partysize)
                {
                    AddMember(member, Brain);
                }

                // set mind set
                Brain.CommandCompleted = false;
                Brain.path = null;

                partyUI.ChangeCompanion(member);
                //HARDCODING EFFECT AS A PLACEHOLDER
                trust.SetTrustEffect(RecipeData.Flavors.Spicy);
            }
            else Debug.Log(member.name + " : has no brain! cannot add to party");
        }
        else Debug.Log(this.name + " : is trying to add a null member to the party");
    }

    private void AddMember(GameObject member, AIData brain)
    {
        party.Add(member);
        brain.CommandCompleted = false;
        brain.path = null;
        Debug.Log(member.name + " : has joined the party");
    }

    public void LeaveTeam(GameObject member)
    {
        // if subject still exists
        if (member != null)
        {
            AIData Brain = member.GetComponent<AIData>();
            if (Brain != null)
            {
                // set player party
                party.Remove(member);
                Debug.Log(member.name + " : has left the party");
                // set mind set
                Brain.CommandCompleted = true;
                Brain.path = null;
            }
        }
    }

    public void ClearParty()
    {
        foreach (GameObject member in party)
        {
            AIData Brain = member.GetComponent<AIData>();
            if (Brain != null)
            {
                // set mind set
                Brain.CommandCompleted = true;
                Brain.path = null;
            }
        }

        party.Clear();

        Debug.Log("Party has been cleared!");
    }

    #endregion

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathUtils;

public class FlavorInputManager : MonoBehaviour
{
    #region Feeding
    public bool isCompanion = false;
    protected Dictionary<RecipeData.Flavors, int> flavorCountDictionary = new Dictionary<RecipeData.Flavors, int>();
    protected Dictionary<IngredientData, int> ingredientCountDictionary = new Dictionary<IngredientData, int>();
    public IngredientData[] favoriteIngredients;
    public RecipeData.Flavors favoriteFlavor;
    public IngredientData[] rejectedIngredients;

    protected bool fedFavoriteIngredient = false;

    public AudioClip rewardSFX;
    public AudioClip rejectSFX;

    #endregion

    #region Components
    protected AudioSource sfxPlayer;
    protected AIData characterData;
    protected SpriteRenderer spriteRenderer;
    #endregion

    #region Other
    protected PointVector pv = new PointVector();
    public float dotTicLength = 1;
    [SerializeField]
    protected GameObject rejectedObjectTemplate;
    [SerializeField]
    protected ParticleSystem spawnParticles;
    #endregion

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        sfxPlayer = GetComponent<AudioSource>();
        // differentiate between ai and char data
        characterData = GetComponent<AIData>();
        if (characterData == null)
            characterData = (AIData)GetComponent<CharacterData>();

        if (this.tag == "Prey")
        {
            FavoriteFoodBubble ffb = GetComponentInChildren<FavoriteFoodBubble>();
            ffb.fruitant = this.gameObject;
            ffb.favoriteFood1 = favoriteIngredients[0];
        }
    }

    public virtual void Feed(IngredientData ingredient, bool fedByPlayer, CharacterData feederData)
    {
        //Debug.Log(this.gameObject.name + " is eating " + ingredient.displayName);
        bool rejected = false;
        //check to see if we should reject this
        foreach (IngredientData rejectedIngredient in rejectedIngredients)
        {
            if (ingredient == rejectedIngredient)
            {
                rejected = true;
                //spit out the rejected object
                SpawnRejectedIngredient(ingredient);
                if (sfxPlayer != null)
                {
                    sfxPlayer.clip = rejectSFX;
                    sfxPlayer.Play();
                }
            }
        }
        //if we didn't reject it, heal and check if we should morph
        if (!rejected)
        {
            bool healed = false;
            foreach (IngredientData favoriteIngredient in favoriteIngredients)
            {
                if (ingredient == favoriteIngredient)
                {
                    characterData.DoHeal(6);
                    healed = true;
                }
                else if ((ingredient.flavors & favoriteIngredient.flavors) > 0)
                {
                    characterData.DoHeal(4);
                    healed = true;
                }
            }
            if (!healed) characterData.DoHeal(2);
        }
    }

    public virtual void PlaySpawnParticles()
    {
        if (spawnParticles != null)
            spawnParticles.Play();
        else
            Debug.Log("FlavorInputManager Error: No particle system");
    }

    protected void SpawnRejectedIngredient(IngredientData data)
    {
        GameObject rejectedObject = Instantiate(rejectedObjectTemplate, transform.position, Quaternion.identity);
        SpriteRenderer rejectedSR = rejectedObject.GetComponent<SpriteRenderer>();
        SkewerableObject rejectedSO = rejectedObject.GetComponent<SkewerableObject>();
        rejectedSR.sprite = data.image;
        rejectedSO.data = data;
        sfxPlayer.clip = rejectSFX;
        sfxPlayer.Play();
    }
 
}

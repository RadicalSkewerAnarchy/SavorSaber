using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSkewerAOE : MonoBehaviour
{
    public RecipeData effectRecipeData = null;
    public Dictionary<RecipeData.Flavors, int> flavorCountDictionary;
    public Dictionary<string, int> ingredientCountDictionary;
    public IngredientData[] ingredientArray;
    public float explosionLifetime = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AIData aiData = null;
        //Debug.Log("Explosion collided with " + collision.gameObject);
        if (ingredientArray != null && collision.tag != "Player")
        {
            FlavorInputManager flavorInput = collision.gameObject.GetComponent<FlavorInputManager>();
            if (flavorInput != null)
            {
                flavorInput.Feed(ingredientArray, true);
            }
        }

        if (flavorCountDictionary[RecipeData.Flavors.Umami] > 0 && collision.tag != "Player")
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            aiData = collision.GetComponent<AIData>();
            if(rb != null)
            {
                Vector2 forceVector = (collision.transform.position - transform.position).normalized * flavorCountDictionary[RecipeData.Flavors.Umami] * 1.5f;
                rb.AddForce(forceVector, ForceMode2D.Impulse);
                if(aiData != null){
                    aiData.updateBehavior = false;
                }
            }
        }
        StartCoroutine(Explode(aiData));
    }

    private IEnumerator Explode(AIData aiData)
    {
        yield return new WaitForSeconds(explosionLifetime+1);
        if(aiData != null) aiData.updateBehavior = true;
        Destroy(this.gameObject);
    }
}

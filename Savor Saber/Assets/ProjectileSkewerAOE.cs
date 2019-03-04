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
        Debug.Log("Explosion collided with " + collision.gameObject);
        if (ingredientArray != null)
        {
            FlavorInputManager flavorInput = collision.gameObject.GetComponent<FlavorInputManager>();
            if (flavorInput != null)
            {
                flavorInput.Feed(ingredientArray);
            }
        }

        if (flavorCountDictionary[RecipeData.Flavors.Savory] > 0)
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if(rb != null)
            {
                Vector2 forceVector = (collision.transform.position - transform.position).normalized * flavorCountDictionary[RecipeData.Flavors.Savory];
                rb.AddForce(forceVector, ForceMode2D.Impulse);
            }
        }

        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(explosionLifetime);
        Destroy(this.gameObject);
    }
}

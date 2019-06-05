using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class MGSText : MonoBehaviour
{
    public float fadeTime;
    public float speed;
    private float currTime = 0;
    private TextMeshPro text;
    private float timeBeforeColorFade = 1.25f;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
    }

    public void Configure(IngredientData ing)
    {
        string icon = ing.flavors != RecipeData.Flavors.None ? " {img,Icon" + ing.flavors.ToString() + "}" : string.Empty;
        text.text = TextMacros.instance.Parse(ing.displayName + icon);
    }

    private void Update()
    {
        currTime += Time.deltaTime;
        if(currTime > fadeTime + timeBeforeColorFade)
        {
            Destroy(gameObject);
            return;
        }
        transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        text.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), Mathf.Max(0, currTime - timeBeforeColorFade) / (fadeTime));
    }

}

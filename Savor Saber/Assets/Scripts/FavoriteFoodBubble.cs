using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FavoriteFoodBubble : MonoBehaviour
{
    public GameObject fruitant;
    public GameObject fruitDisplay;
    public SpriteRenderer bubbleRender;
    public SpriteRenderer fruitRender;
    public FlavorInputManager flavors;
    public bool show = false;
    public bool reset = true;
    public Sprite favoriteFood;
    GameObject player;
    public GameObject audioPlayer;
    public AudioClip audio;

    // Start is called before the first frame update
    void Start()
    {
        flavors = fruitant.GetComponent<FlavorInputManager>();
        bubbleRender = GetComponent<SpriteRenderer>();
        fruitRender = fruitDisplay.GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Show();

        // add in one more if statement
        // only do it when there is no cutscene
        //if (not in cutscene)
        //{
            if (InputManager.GetButton(Control.Interact))
            {
                if (Vector2.Distance(player.transform.position, this.transform.position) < 3)
                {
                    show = true;
                }
            }
        //}
    }

    private void Show()
    {
        if (show)
        {
            if (reset)
            {
                // get random favorite

                RecipeDatabase rdb = player.GetComponentInChildren<RecipeDatabase>();
                Sprite s;
                //if (Random.Range(0f, 1.0f) < 0.5f)
                //{
                    // random ingredient
                    int len = flavors.favoriteIngredients.Length;
                    string fav = flavors.favoriteIngredients[Random.Range(0, len - 1)];
                    // get from ingredients
                    IngredientData d = rdb.allIngredients[fav];
                    // display
                    s = d.image;
                //}
                /*else
                {
                    // favorite flavor
                    Debug.Log(flavors.favoriteFlavors);
                    string ff = rdb.flavorToString[flavors.favoriteFlavors];
                    Debug.Log(ff);
                    // get image from database
                    //s = rdb.allFlavors[ff];
                    s = rdb.allFlavors[ff];
                }*/

                Debug.Log(s.name);
                fruitRender.sprite = s;

                // set sprite
                StartCoroutine(EndAfterSeconds(2));

                if (audio != null)
                {
                    GameObject sfx = Instantiate(audioPlayer, transform.position, Quaternion.identity);
                    sfx.GetComponent<PlayAndDestroy>().Play(audio);
                    sfx.GetComponent<AudioSource>().volume /= 2;
                }
                reset = false;
            }

            fruitRender.enabled = true;
            bubbleRender.enabled = true;
        }
        else
        {
            fruitRender.enabled = false;
            bubbleRender.enabled = false;
            reset = true;
        }
    }

    protected IEnumerator EndAfterSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        show = false;
        yield return null;
    }
}

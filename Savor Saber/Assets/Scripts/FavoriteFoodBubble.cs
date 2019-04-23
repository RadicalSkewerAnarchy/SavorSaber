using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FavoriteFoodBubble : MonoBehaviour
{
    public GameObject fruitant;
    public GameObject fruit;
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
        fruitRender = fruit.GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Show();
        if (InputManager.GetButton(Control.Interact))
        {
            if (Vector2.Distance(player.transform.position, this.transform.position) < 4)
            {
                show = true;
            }
        }
    }

    private void Show()
    {
        if (show)
        {
            if (reset)
            {
                // get random favorite
                int len = flavors.favoriteIngredients.Length;
                string ff = flavors.favoriteIngredients[Random.Range(0, len-1)];
                IngredientData d = player.GetComponentInChildren<RecipeDatabase>().allIngredients[ff];

                // set sprite
                fruitRender.sprite = d.image;
                StartCoroutine(EndAfterSeconds(2));

                if (audio != null)
                {
                    GameObject sfx = Instantiate(audioPlayer, transform.position, Quaternion.identity);
                    sfx.GetComponent<PlayAndDestroy>().Play(audio);
                    sfx.GetComponent<AudioSource>().volume /=2 ;
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

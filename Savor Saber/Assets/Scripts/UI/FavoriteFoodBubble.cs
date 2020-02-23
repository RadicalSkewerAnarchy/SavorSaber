using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FavoriteFoodBubble : MonoBehaviour
{
    public GameObject fruitant;
    public GameObject fruitDisplay1;
    public GameObject fruitDisplay2;
    public GameObject fruitDisplay3;
    [HideInInspector]
    public SpriteRenderer bubbleRender;
    [HideInInspector]
    public SpriteRenderer fruitRender1;
    [HideInInspector]
    public SpriteRenderer fruitRender2;
    [HideInInspector]
    public SpriteRenderer fruitRender3;
    [HideInInspector]
    public FlavorInputManager flavors;
    public bool show = false;
    public bool reset = true;
    public bool keep = false;
    public IngredientData favoriteFood1;
    public IngredientData favoriteFood2;
    public IngredientData favoriteFood3;
    GameObject player;
    public GameObject audioPlayer;
    public AudioClip audioFX;

    // Start is called before the first frame update
    void Start()
    {
        flavors = GetComponentInParent<FlavorInputManager>();
        bubbleRender = GetComponent<SpriteRenderer>();
        if (fruitDisplay1 != null)
            fruitRender1 = fruitDisplay1.GetComponent<SpriteRenderer>();
        if (fruitDisplay2 != null)
            fruitRender2 = fruitDisplay2.GetComponent<SpriteRenderer>();
        if (fruitDisplay3 != null)
            fruitRender3 = fruitDisplay3.GetComponent<SpriteRenderer>();

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Show();
        
        // only do it when there is no cutscene
        if (!EventTrigger.InCutscene)
        {
            if (InputManager.GetButton(Control.Knife))
            {
                if (Vector2.Distance(player.transform.position, transform.position) < 2.5f)
                {
                    Debug.Log(this.name + ": Displaying favorite food");
                    show = true;
                }
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
                //RecipeDatabase rdb = player.GetComponentInChildren<RecipeDatabase>();
                Sprite s1 = (favoriteFood1 == null ? null : favoriteFood1.image);
                Sprite s2 = (favoriteFood2 == null ? null : favoriteFood2.image);
                Sprite s3 = (favoriteFood3 == null ? null : favoriteFood3.image);

                if (fruitRender1 != null)
                    if (s1 != null)
                        fruitRender1.sprite = s1;
                if (fruitRender2 != null)
                    if (s2 != null)
                        fruitRender2.sprite = s2;
                if (fruitRender3 != null)
                    if (s3 != null)
                        fruitRender3.sprite = s3;

                // set sprite
                if (!keep)
                    StartCoroutine(EndAfterSeconds(2));

                if (audioFX != null)
                {
                    GameObject sfx = Instantiate(audioPlayer, transform.position, Quaternion.identity);
                    sfx.GetComponent<PlayAndDestroy>().Play(audioFX);
                    sfx.GetComponent<AudioSource>().volume /= 2;
                }
                reset = false;
            }

            if (fruitRender1 != null)
                fruitRender1.enabled = true;
            if (fruitRender2 != null)
                fruitRender2.enabled = true;
            if (fruitRender3 != null)
                fruitRender3.enabled = true;

            bubbleRender.enabled = true;
        }
        else
        {
            if (fruitRender1 != null)
                fruitRender1.enabled = false;
            if (fruitRender2 != null)
                fruitRender2.enabled = false;
            if (fruitRender3 != null)
                fruitRender3.enabled = false;

            bubbleRender.enabled = false;
            if (!keep)
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

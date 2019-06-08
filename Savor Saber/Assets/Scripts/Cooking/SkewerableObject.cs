using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attatch this component to an object to make it skewerable
/// A collision component of some kind is also required
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class SkewerableObject : MonoBehaviour
{
    /// <summary> Ingredient Data SO. This is what will actually be added to the player's skewer </summary>
    public IngredientData data;

    public bool decay = true;
    public Timer decayTime = new Timer(10);
    private bool flickering = false;
    private SpriteRenderer sp;

    //movement values
    public float maxDrift = 2;
    public float driftSpeed = 1;
    private bool attached = false;

    private Vector3 origin;

    public TileNode currentTile;

    public bool Attached { get => attached; set => attached = value; }

    // Use this for initialization
    void Start()
    {
        origin = transform.position;

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();

        if (!Attached)
        {
            float x = Random.Range(-maxDrift, maxDrift) / 4;
            float y = Random.Range(-maxDrift, maxDrift) / 4;
            rb.AddForce(new Vector2(x, y), ForceMode2D.Impulse);
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (!Attached && decay)
        {
            //transform.position = Vector3.MoveTowards(transform.position, target, driftSpeed * Time.deltaTime);
            if (!flickering)
            {
                if (decayTime.Update())
                {
                    flickering = true;
                    StartCoroutine(FlickerOut());
                }
                sp.color = Color.Lerp(Color.white, new Color(0.75f, 0.5f, 0.25f), decayTime.PercentDone - 0.5f);
            }
        }
        else
        {
            transform.position = origin;
        }
        //transform.position = Vector3.MoveTowards(transform.position, target, driftSpeed * Time.deltaTime);
    }

    private IEnumerator FlickerOut()
    {
        float time = 0.525f;
        bool on = true;
        while (time > 0)
        {
            yield return new WaitForSeconds(time -= 0.025f);
            sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, on ? 1 : 0.75f);
            on = !on;
        }
        Destroy(gameObject);
    }
}

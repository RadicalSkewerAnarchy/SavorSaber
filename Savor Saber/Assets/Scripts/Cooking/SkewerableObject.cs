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

    // Add stuff here for movement type

    //movement values
    public float maxDrift = 4;
    public float driftSpeed = 1;
    public bool attached = false;

    private Vector3 target;
    private Vector3 origin;
    private Vector3 halfScale = new Vector3(0.5f, 0.5f, 1f);

    // Use this for initialization
    void Start()
    {
        origin = transform.position;

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float x = transform.position.x + Random.Range(-maxDrift, maxDrift);
        float y = transform.position.y + Random.Range(-maxDrift, maxDrift);
        float z = transform.position.z + Random.Range(-maxDrift, maxDrift);

        target = new Vector3(x, y, z);
    }

    // Update is called once per frame
    void Update()
    {
        if (attached)
        {
            transform.position = origin;
            transform.localScale = halfScale;
        }
        else if (!attached || driftSpeed > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, driftSpeed * Time.deltaTime);
            transform.localScale = halfScale * 2;
            // adjust drift speed
            driftSpeed /= 1.01f;
            if (driftSpeed <= 0.15)
                driftSpeed = 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LimitBreakFire : PoweredObject
{
    public string[] tagsToHit;
    public float speed = 4f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        foreach (string tag in tagsToHit)
        {
            if (collision.gameObject.tag == tag)
            {
                Debug.Log("Fire limit break hit valid target");
                CharacterData data = collision.gameObject.GetComponent<CharacterData>();
                if (data != null)
                {
                    data.DoDamage(999, true);
                }
            }

        }

    }

}

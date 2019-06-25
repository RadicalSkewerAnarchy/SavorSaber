using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class DroneBossSpecial : MonoBehaviour
{
    private Animator AnimatorBody;
    public GameObject knockbackTemplate;
    private bool spinning = false;
    private WaitForSeconds spinDelay;

    // Start is called before the first frame update
    void Start()
    {
        AnimatorBody = GetComponentInParent<Animator>();
        spinDelay = new WaitForSeconds(1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Prey")
        {
            Instantiate(knockbackTemplate, transform.position, Quaternion.identity);
            if (!spinning)
            {
                StartCoroutine(Spin());
            }
        }
    }

    private IEnumerator Spin()
    {
        spinning = true;
        AnimatorBody.Play("Melee");
        yield return spinDelay;
        spinning = false;
    }

}

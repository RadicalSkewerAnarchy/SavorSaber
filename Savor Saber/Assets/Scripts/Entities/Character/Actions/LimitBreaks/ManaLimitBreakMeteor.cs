using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaLimitBreakMeteor : PoweredObject
{
    public Animator attackAnimator;
    public AudioSource attackSFX;
    Collider2D[] objectsInRange;
    public GameObject targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TurnOn()
    {
        base.TurnOn();
    }

    public override void ShutOff()
    {
        base.ShutOff();
    }

    public void DoDamage()
    {
        objectsInRange = Physics2D.OverlapCircleAll(targetPosition.transform.position, 99f);
        foreach(Collider2D collider in objectsInRange)
        {
            if(collider.gameObject.tag == "Player")
            {
                PlayerData playerData = collider.gameObject.GetComponent<PlayerData>();
                if (!playerData.Invincible)
                {
                    playerData.DoDamage(9999);
                }
            }
        }
    }
}

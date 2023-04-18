using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AttackRanged))]
public class AttackRangedTargetZone : MonoBehaviour
{
    [SerializeField]
    private string[] tagsToTarget;
    private GameObject currentTarget;
    private AttackRanged shooter;
    public GameObject explosionTemplate;
    [SerializeField]
    private GameObject explosionSpawnPoint;
    public float shotCooldown = 5;
    public bool active = true;
    private WaitForSeconds cooldownTimer;
    // Start is called before the first frame update
    void Start()
    {
        shooter = GetComponent<AttackRanged>();
        cooldownTimer = new WaitForSeconds(shotCooldown);
        if (active) TurnOn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOn()
    {
        Shoot();
    }

    private void Shoot()
    {
        //Debug.Log("Entering Shoot() loop of static turret");
        //Debug.Log("Turret target is " + currentTarget);
        if (currentTarget != null)
        {
            shooter.Attack(currentTarget.transform.position);
            Instantiate(explosionTemplate, explosionSpawnPoint.transform.position, Quaternion.identity, this.gameObject.transform);
        }
        StartCoroutine(ExecuteAfterSeconds());
    }

    private IEnumerator ExecuteAfterSeconds()
    {
        yield return cooldownTimer;
        Shoot();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach(string tag in tagsToTarget)
        {
            if(collision.gameObject.tag == tag)
            {
                currentTarget = collision.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject == currentTarget)
        {
            currentTarget = null;
        }
    }
}

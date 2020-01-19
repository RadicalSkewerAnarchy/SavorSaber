using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyTeleportBeacon : MonoBehaviour
{

    private PlayerData somaData;
    //private Collider2D[] blockingObjects = new Collider2D[32];
    private Animator spinner;
    private bool teleporting = false;
    private WaitForSeconds OneSecondWait = new WaitForSeconds(1);
    private Collider2D scanner;

    private int numHits = 0;
    // Start is called before the first frame update
    void Start()
    {
        scanner = GetComponent<Collider2D>();
        spinner = GetComponent<Animator>();
        somaData = GetComponentInParent<PlayerData>();
        scanner.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetButtonDown(Control.Command4) && somaData.PartySize > 0 && !teleporting)
        {
            
            StopAllCoroutines();
            StartCoroutine(Scan());
        }
    }

    private IEnumerator Scan()
    {
        teleporting = true;
        scanner.enabled = true;
        numHits = 0;

        yield return OneSecondWait;

        scanner.enabled = false;
        Teleport();

        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        numHits++;
    }

    private void Teleport()
    {
        if (numHits > 0)
        {
            Debug.Log("Not enough space to teleport");
        }
        else
        {
            Debug.Log("Area clear, commencing teleport");
        }
        teleporting = false;
    }

}

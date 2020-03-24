using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyTeleportBeacon : MonoBehaviour
{
    public GameObject teleportEffectPrefab;
    public GameObject radar;
    public Commander partyCommander;

    private PlayerData somaData;
    private AudioSource teleSFX;
    private Animator spinner;
    private bool teleporting = false;
    private WaitForSeconds OneSecondWait = new WaitForSeconds(1);
    private Collider2D scanner;
    private GameObject player;
    private int numHits = 0;

    //AI Director stuff
    private Commander.Criteria ObjectCriteria = Commander.Criteria.None;
    private GameObject Object;
    private AIData.Protocols Verb;
    private Vector2 Location = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        scanner = GetComponent<Collider2D>();
        spinner = GetComponent<Animator>();
        teleSFX = GetComponent<AudioSource>();
        somaData = GetComponentInParent<PlayerData>();
        scanner.enabled = false;
        player = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetButtonDown(Control.Command4) && somaData.PartySize > 0 && !teleporting)
        {
            
            StopAllCoroutines();
            StartCoroutine(Scan());
        }
        radar.transform.position = transform.parent.position;
    }

    private IEnumerator Scan()
    {
        radar.SetActive(true);
        radar.transform.position = transform.parent.position;
        teleporting = true;
        scanner.enabled = true;
        numHits = 0;

        yield return OneSecondWait;

        scanner.enabled = false;
        Teleport();
        radar.SetActive(false);
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Prey")
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

            if(somaData.party.Count > 0)
                teleSFX.Play();

            Vector2 targetPosition;
            int positionIndex = 0;
            foreach(GameObject partyMember in somaData.party)
            {

                //targetPosition = player.transform.position + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f),0);
                if (positionIndex == 0)
                    targetPosition = player.transform.position + new Vector3(1.0f, 0, 0);
                else if (positionIndex == 1)
                    targetPosition = player.transform.position + new Vector3(0, 1.0f, 0);
                else
                    targetPosition = player.transform.position + new Vector3(-1.0f, 0, 0);

                partyMember.transform.position = targetPosition;
                if(teleportEffectPrefab != null)
                {
                    Instantiate(teleportEffectPrefab, targetPosition, Quaternion.identity);
                }
                positionIndex++;
            }

            Verb = AIData.Protocols.Chase;
            ObjectCriteria = Commander.Criteria.None;
            Object = GameObject.FindGameObjectWithTag("Player");
            partyCommander.GroupCommand(player.GetComponent<PlayerData>().party, Verb, ObjectCriteria, Object, Location);
        }
        teleporting = false;
    }

}

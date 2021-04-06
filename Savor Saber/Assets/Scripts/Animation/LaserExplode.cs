using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserExplode : MonoBehaviour
{
    public GameObject explosionPosition;
    public GameObject explosionPrefab;

    public GameObject[] objectsToEnable;
    public GameObject[] objectsToDisable;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnExplosion()
    {
        Instantiate(explosionPrefab, explosionPosition.transform.position, Quaternion.identity);

        foreach(GameObject obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }
    }
}

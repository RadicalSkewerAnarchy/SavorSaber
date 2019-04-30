using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DropOnDeath))]
public class DroneItemGenerator : MonoBehaviour
{

    public GameObject[] itemsToDrop;

    // Start is called before the first frame update
    void Start()
    {
        DropOnDeath dropper = GetComponent<DropOnDeath>();
        GameObject[] dropArray = dropper.drops;

        List<GameObject> dropList = new List<GameObject>();
        dropList.AddRange(dropArray);
        dropList.AddRange(itemsToDrop);

        GameObject[] finalDrops = dropList.ToArray();

        dropper.drops = finalDrops;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

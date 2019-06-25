using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DropOnDeath))]
public class DroneItemGenerator : MonoBehaviour
{

    public GameObject[] itemsToDrop;

    //Decides if the generator should add a random selection of possible items
    public bool randomSelection = false;
    public int randomItemCount = 4;

    // Start is called before the first frame update
    void Start()
    {
        //get the current things that this drone will drop
        //usually just an explosion.
        //ensures that the regular dropondeath is not overwritten
        DropOnDeath dropper = GetComponent<DropOnDeath>();
        GameObject[] dropArray = dropper.drops;

        //final array starts with the contents of dropondeath
        List<GameObject> dropList = new List<GameObject>();
        dropList.AddRange(dropArray);

        if (randomSelection)
        {
            for(int i = 0; i < randomItemCount; i++)
            {
                int itemNumber = Random.Range(0, itemsToDrop.Length);
                dropList.Add(itemsToDrop[itemNumber]);
            }
            GameObject[] finalDrops = dropList.ToArray();
            dropper.drops = finalDrops;
        }
        else
        {
            //add the entire contents of the items to drop list
            dropList.AddRange(itemsToDrop);
            GameObject[] finalDrops = dropList.ToArray();
            dropper.drops = finalDrops;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

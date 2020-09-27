using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject location;
    public void Respawn(GameObject player)
    {
        var cData = player.GetComponent<CharacterData>();
        cData.health = cData.maxHealth;
        //Add more fancy death scene later
        player.transform.position = location.transform.position;
        player.GetComponent<Animator>().Play("Idle");
    }
}

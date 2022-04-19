using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkewerBonusEffect : MonoBehaviour
{

    protected GameObject target;
    protected CharacterData targetData;
    public int magnitude = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void SetTarget(GameObject obj, int mag)
    {
        Debug.Log("Skewer bonus effect - Setting target: " + obj);
        target = obj;
        targetData = obj.GetComponent<CharacterData>();
        magnitude = mag;

    }
}

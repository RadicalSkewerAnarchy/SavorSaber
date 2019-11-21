using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class PartySelectorFruitant : MonoBehaviour
{
    private AudioSource hoverSound;
    private PlaySFX closeSound;
    [HideInInspector]
    public Commander partyCommander;
    public GameObject targetFruitant;

    // Start is called before the first frame update
    void Start()
    {
        hoverSound = GetComponent<AudioSource>();
        closeSound = GetComponentInParent<PlaySFX>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DismissFruitant()
    {
        if(targetFruitant != null && partyCommander != null)
            partyCommander.LeaveTeam(targetFruitant);

        closeSound.Play(closeSound.sfx);
    }

    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer hovering over party select ubtton");
        hoverSound.Play();
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(this.gameObject);
    }
    
}

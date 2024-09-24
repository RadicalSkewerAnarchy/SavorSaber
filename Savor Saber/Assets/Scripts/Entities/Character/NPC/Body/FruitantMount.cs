using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitantMount : MonoBehaviour
{
    private Animator playerAnimator;
    private PlayerData playerData;
    private PlayerController controller;
    private RuntimeAnimatorController mountedAnimationController;
    private RuntimeAnimatorController baseAnimationController;
    private float baseSpeed;
    private bool canMount = false;
    private bool isMounted = false;
    private bool mountDetected = false;
    private GameObject currentlyDetectedMount;
    private bool specialTerrainMount = false;
    private FruitantMountData mountData;
    [SerializeField]
    private GameObject specialTerrainCollider;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponentInParent<Animator>();
        baseAnimationController = playerAnimator.runtimeAnimatorController;
        controller = GetComponentInParent<PlayerController>();
        playerData = GetComponentInParent<PlayerData>();
        baseSpeed = playerData.Speed;   
    }

    // Update is called once per frame
    void Update()
    {
        if(canMount && !isMounted && !EventTrigger.InCutscene && playerData.health > 0 && !controller.mounted && InputManager.GetButtonDown(Control.Dash, InputAxis.Dash))
        {
            Debug.Log("Mount Data is null? " + mountData == null);
            Mount();
        }
        else if (isMounted && InputManager.GetButtonDown(Control.Dash, InputAxis.Dash))
        {
            Dismount();
        }
        
    }

    void Mount()
    {
        playerAnimator.runtimeAnimatorController = mountedAnimationController;
        //controller.SetSpeed(baseSpeed * mountData.speedMultiplier);
        controller.mounted = true;
        isMounted = true;
        if(mountData.crossSpecialTerrain)
        {
            specialTerrainCollider.SetActive(false);
        }
        Destroy(mountData.gameObject);
    }

    void Dismount()
    {
        playerAnimator.runtimeAnimatorController = baseAnimationController;
        //controller.SetSpeed(baseSpeed);
        controller.mounted = false;
        isMounted = false;
        specialTerrainCollider.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Prey") Debug.Log("Fruitant mount detected fruitant in zone");
        //only check for new mount data if we are not already in range of a mount;
        //this makes it so that the first mount we touch is the one we'll use
        if (!mountDetected)
        {
            mountData = collision.gameObject.GetComponent<FruitantMountData>();
        }
        //if the mount data was found, register this object as the currently detected mount and tell Soma they can mount it
        if(mountData != null)
        {
            mountedAnimationController = mountData.animatorController;
            mountDetected = true;
            currentlyDetectedMount = collision.gameObject;
            canMount = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Prey") Debug.Log("Fruitant exiting mount zone");
        if (mountData != null)
        {
            //if the object we're leaving contact with was the currently detected mount, we are no longer detecting a mount
            //you fool, you absolute cretin
            if(collision.gameObject == currentlyDetectedMount)
            {
                mountDetected = false;
                currentlyDetectedMount = null;
            }
            canMount = false;
        }

    }
}

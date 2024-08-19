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
        if(mountData != null && mountData.crossSpecialTerrain)
        {
            specialTerrainCollider.SetActive(true);
        }
        Destroy(mountData.gameObject);
    }

    void Dismount()
    {
        playerAnimator.runtimeAnimatorController = baseAnimationController;
        //controller.SetSpeed(baseSpeed);
        controller.mounted = false;
        isMounted = false;
        specialTerrainCollider.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Prey") Debug.Log("Fruitant mount detected fruitant in zone");
        mountData = collision.gameObject.GetComponent<FruitantMountData>();
        if(mountData != null)
            mountedAnimationController = mountData.animatorController;
        if (mountData != null) canMount = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Prey") Debug.Log("Fruitant exiting mount zone");
        if (mountData != null)
        {
            mountData = collision.gameObject.GetComponent<FruitantMountData>();
            canMount = false;
        }

    }
}

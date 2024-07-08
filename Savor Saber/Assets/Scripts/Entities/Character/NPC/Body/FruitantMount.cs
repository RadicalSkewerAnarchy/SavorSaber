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
    private bool canMount = false;
    private FruitantMountData mountData;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponentInParent<Animator>();
        baseAnimationController = playerAnimator.runtimeAnimatorController;
        controller = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canMount && !EventTrigger.InCutscene && playerData.health > 0 && !controller.riding && InputManager.GetButtonDown(Control.Dash, InputAxis.Dash))
        {
            Mount();
        }
        
    }

    void Mount()
    {
        playerAnimator.runtimeAnimatorController = mountedAnimationController;
        controller.riding = true;
        canMount = false;
    }

    void Dismount()
    {
        playerAnimator.runtimeAnimatorController = baseAnimationController;
        controller.riding = false;
        canMount = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Prey:") Debug.Log("Fruitant mount detected fruitant in zone");
        mountData = collision.gameObject.GetComponent<FruitantMountData>();
        if (mountData != null) canMount = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }
}

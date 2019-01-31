using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class InputSwapper : MonoBehaviour
{
    private InputManager man;
    private void Start()
    {
        man = GetComponent<InputManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
            man.keyboardControls = man.controlProfiles["Wasd"];
        if (Input.GetKeyDown(KeyCode.Keypad2))
            man.keyboardControls = man.controlProfiles["Wasdf"];
        if (Input.GetKeyDown(KeyCode.Keypad3))
            man.keyboardControls = man.controlProfiles["zxc"];
    }
}

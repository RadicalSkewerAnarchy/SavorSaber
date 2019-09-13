using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInputTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetButtonDown(Control.Command1, InputAxis.Command1))
            Debug.Log("Command 1 Activated!");
        if (InputManager.GetButtonDown(Control.Command2, InputAxis.Command2))
            Debug.Log("Command 2 Activated!");
        if (InputManager.GetButtonDown(Control.Command3, InputAxis.Command3))
            Debug.Log("Command 3 Activated!");
        if (InputManager.GetButtonDown(Control.Command4, InputAxis.Command4))
            Debug.Log("Command 4 Activated!");
        var aimMove = InputManager.GetAxesAsVector2(InputAxis.HorizontalAim, InputAxis.VerticalAim);
        if (aimMove != Vector2.zero)
            Debug.Log(aimMove);
    }
}

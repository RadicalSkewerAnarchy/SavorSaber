using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputModeSelectButtonColor : MonoBehaviour
{
    public ButtonSetColor onWasd;
    public ButtonSetColor onZxc;
    public ButtonSetColor onControllerABXY;
    public ButtonSetColor onControllerTriggers;

    public void SetButtons()
    {
        if(InputManager.main.keyboardControls.displayName == "WASD")
        {
            onWasd.SetSelected();
            onZxc.SetUnselected();
        }
        else if(InputManager.main.keyboardControls.displayName == "ZXC")
        {
            onWasd.SetUnselected();
            onZxc.SetSelected();
        }
        if (InputManager.main.gamepadControls.displayName == "ABXY")
        {
            onControllerABXY.SetSelected();
            onControllerTriggers.SetUnselected();
        }
        else if (InputManager.main.gamepadControls.displayName == "Triggers")
        {
            onControllerABXY.SetUnselected();
            onControllerTriggers.SetSelected();
        }
    }
}

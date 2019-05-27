using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInputProfile : MonoBehaviour
{
    public void SetKeyboardControls(ControlProfile p)
    {
        InputManager.main.SetKeyboardProfile(p);
    }
    public void SetGamepadControls(ControlProfile p)
    {
        InputManager.main.SetGamepadProfile(p);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Control
{
    Dash,
    Cook,
    Eat,
    Interact,
    Camp,
    Knife,
    Skewer,
    Throw,
    Pause,
    Confirm,
    Cancel,
    SwapSkewerLeft,
    SwapSkewerRight,
    Up,
    Down,
    Left,
    Right,
}

public enum InputAxis
{
    Horizontal,
    Vertical,
    RightTrigger,
    LeftTrigger,
}

public class InputManager : MonoBehaviour
{
    private static InputManager main;
    // Start is called before the first frame update

    public ControlProfile keyboardControls = null;
    public ControlProfile gamepadControls = null;

    public ControlDict controlProfiles = new ControlDict();
   
    private void Awake()
    {
        if (main == null)
        {
            main = this;
        }
        else
            Destroy(gameObject);
    }

    public static float GetAxis(InputAxis a)
    {
        return Input.GetAxis(a.ToString());
    }
    public static bool GetButton(Control c)
    {
        return Input.GetKey(main.keyboardControls[c]) || (main.gamepadControls.keyBinds.ContainsKey(c) && Input.GetKey(main.gamepadControls[c]));
    }
    public static bool GetButtonDown(Control c)
    {
        return Input.GetKeyDown(main.keyboardControls[c]) || (main.gamepadControls.keyBinds.ContainsKey(c) && Input.GetKeyDown(main.gamepadControls[c]));
    }
    public static bool GetButtonUp(Control c)
    {
        return Input.GetKeyUp(main.keyboardControls[c]) || (main.gamepadControls.keyBinds.ContainsKey(c) && Input.GetKeyUp(main.gamepadControls[c]));
    }


    [System.Serializable] public class ControlDict : SerializableCollections.SDictionary<string, ControlProfile> { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Control
{
    //Up,
    //Down,
    //Left,
    //Right,
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
    public static InputManager main;
    // Start is called before the first frame update

    public ControlProfile keyboardControls = null;
    public ControlProfile gamepadControls = null;

    public ControlDict controlProfiles = new ControlDict();
   
    private void Awake()
    {
        if (main != null)
        {
            DontDestroyOnLoad(gameObject);
            main = this;
        }
        else
            Destroy(gameObject);
    }

    public float GetAxis(InputAxis a)
    {
        return Input.GetAxis(a.ToString().ToLower());
    }
    public bool GetButton(Control c)
    {
        return Input.GetKey(keyboardControls[c]) || Input.GetKey(gamepadControls[c]);
    }
    public bool GetButtonDown(Control c)
    {
        return Input.GetKeyDown(keyboardControls[c]) || Input.GetKeyDown(gamepadControls[c]);
    }
    public bool GetButtonUp(Control c)
    {
        return Input.GetKeyUp(keyboardControls[c]) || Input.GetKeyUp(gamepadControls[c]);
    }


    [System.Serializable] public class ControlDict : SerializableCollections.SDictionary<string, ControlProfile> { }
}

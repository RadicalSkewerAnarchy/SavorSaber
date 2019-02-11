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
    Dash,
}

public enum AxisName
{
    None,
    HorizontalWASD,
    HorizontalArrows,
    HorizontalGamepad,
    VerticalWASD,
    VerticalArrows,
    VerticalGamepad,
    RightTrigger,
    LeftTrigger,
    BothTriggers,
}

public class InputManager : MonoBehaviour
{
    private static InputManager main;
    // Start is called before the first frame update

    public ControlProfile keyboardControls = null;
    public ControlProfile gamepadControls = null;

    public ControlDict controlProfiles = new ControlDict();
    private Dictionary<AxisName, AxisButton> axisButtons = new Dictionary<AxisName, AxisButton>()
    {
        { AxisName.LeftTrigger, new AxisButton(AxisName.LeftTrigger) },
        { AxisName.RightTrigger, new AxisButton(AxisName.RightTrigger) },
        { AxisName.BothTriggers, new AxisButton(AxisName.BothTriggers) },
    };
   
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
        float c = Input.GetAxis(main.gamepadControls[a].ToString());
        return c != 0 ? c : Input.GetAxis(main.keyboardControls[a].ToString());
    }
    public static bool GetButton(Control c)
    {
        return Input.GetKey(main.keyboardControls[c]) || Input.GetKey(main.gamepadControls[c]);
    }
    public static bool GetButton(Control c, InputAxis a)
    {
        return GetButton(c) || main.axisButtons[main.gamepadControls[a]].GetInput();
    }
    public static bool GetButtonDown(Control c)
    {
        return Input.GetKeyDown(main.keyboardControls[c]) || Input.GetKeyDown(main.gamepadControls[c]);
    }
    public static bool GetButtonDown(Control c, InputAxis a)
    {
        return main.axisButtons[main.gamepadControls[a]].GetInputDown() || GetButtonDown(c);
    }
    public static bool GetButtonUp(Control c)
    {
        return Input.GetKeyUp(main.keyboardControls[c]) || Input.GetKeyUp(main.gamepadControls[c]);
    }
    public static bool GetButtonUp(Control c, InputAxis a)
    {
        return main.axisButtons[main.gamepadControls[a]].GetInputUp() || GetButtonDown(c);
    }



    [System.Serializable] public class ControlDict : SerializableCollections.SDictionary<string, ControlProfile> { }
    public class AxisButton
    {
        public AxisName axis;
        public int indexValue;

        private string axisName;
        private bool InputDown { get { return Input.GetAxisRaw(axisName) != 0; } }
        private bool lastValue = false;

        public AxisButton(AxisName axis)
        {
            this.axis = axis;
            axisName = axis.ToString();
        }

        public bool GetInput()
        {
            return InputDown;
        }

        public bool GetInputDown()
        {
            if (InputDown)
            {
                if (!lastValue)
                {
                    lastValue = true;
                    return true;
                }
                return false;
            }
            lastValue = false;
            return false;
        }

        public bool GetInputUp()
        {
            if (!InputDown)
            {
                if (lastValue)
                {
                    lastValue = false;
                    return true;
                }
                return false;
            }
            lastValue = true;
            return false;
        }
    }
}

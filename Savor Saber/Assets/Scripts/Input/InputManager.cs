using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Remove: Cook, Eat, Camp, Knife
// Add: Command1, Command2, Command3, Command4, ConfirmTarget, CancelTarget
public enum Control
{
    Dash,
    Interact = 3,
    Knife = 5,
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
    Command1,
    Command2,
    Command3,
    Command4,
    ConfirmTarget,
    CancelTarget,
}

public enum InputAxis
{
    Horizontal,
    Vertical,
    Dash,
    Skewer,
    Slash,
    Throw,
    HorizontalAim,
    VericalAim,
    Command1,
    Command2,
    Command3,
    Command4,
}

public enum AxisName
{
    None,
    HorizontalWASD,
    HorizontalArrows,
    HorizontalGamepadLeft,
    HorizontalGamepadRight,
    VerticalWASD,
    VerticalArrows,
    VerticalGamepadLeft,
    VerticalGamepadRight,
    RightTrigger,
    LeftTrigger,
    BothTriggers,
    DirectionalPadXPositive,
    DirectionalPadXNegative,
    DirectionalPadYPositive,
    DirectionalPadYNegative,
}

public class InputManager : MonoBehaviour
{
    public static InputManager main;

    private bool controllerMode = false;
    public static bool ControllerMode { get => main.controllerMode; }
    public static ControlProfile Controls { get => main.controllerMode ? main.gamepadControls : main.keyboardControls; }
    public ControlProfile keyboardControls = null;
    public ControlProfile gamepadControls = null;

    public ControlDict controlProfiles = new ControlDict();
    private Dictionary<AxisName, AxisButton> axisButtons = new Dictionary<AxisName, AxisButton>()
    {
        { AxisName.LeftTrigger, new AxisButton(AxisName.LeftTrigger, AxisButton.Mode.Both) },
        { AxisName.RightTrigger, new AxisButton(AxisName.RightTrigger, AxisButton.Mode.Both) },
        { AxisName.BothTriggers, new AxisButton(AxisName.BothTriggers, AxisButton.Mode.Both) },
        { AxisName.DirectionalPadXNegative, new AxisButton(AxisName.DirectionalPadXNegative, AxisButton.Mode.Negative) },
        { AxisName.DirectionalPadXPositive, new AxisButton(AxisName.DirectionalPadXPositive, AxisButton.Mode.Positive) },
        { AxisName.DirectionalPadYNegative, new AxisButton(AxisName.DirectionalPadYNegative, AxisButton.Mode.Negative) },
        { AxisName.DirectionalPadYPositive, new AxisButton(AxisName.DirectionalPadYPositive, AxisButton.Mode.Positive) },
    };
    private float checkTime = 0;
    private void Awake()
    {
        if (main == null)
        {
            main = this;
            DontDestroyOnLoad(transform);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        // May Switch input switching mode later
        checkTime += Time.deltaTime;
        if (checkTime > 1)
        {
            if(!controllerMode)
                controllerMode = Input.GetJoystickNames().Any((s) => !string.IsNullOrEmpty(s));
            checkTime = 0;
        }
    }

    public static Vector2 GetAxesAsVector2(InputAxis xAxis, InputAxis yAxis)
    {
        return new Vector2(GetAxis(xAxis), GetAxis(yAxis));
    }

    public static float GetAxis(InputAxis a)
    {
        float axisVal = 0;
        if(main.gamepadControls.axes.ContainsKey(a))
        {
            axisVal = Input.GetAxis(main.gamepadControls[a].ToString());
            if (axisVal != 0)
            {
                main.controllerMode = true;
                return axisVal;
            }
        }
        if(main.keyboardControls.axes.ContainsKey(a))
        {
            axisVal = Input.GetAxis(main.keyboardControls[a].ToString());
            if (axisVal == 0 && main.keyboardControls.secondaryAxes.ContainsKey(a))
                axisVal = Input.GetAxis(main.keyboardControls.secondaryAxes[a].ToString());
            if (axisVal != 0)
                main.controllerMode = false;
        }
        return axisVal;
    }
    public static bool GetButton(Control c)
    {
        bool buttonVal = false;
        if (main.gamepadControls.keyBinds.ContainsKey(c))
        {
            buttonVal = Input.GetKey(main.gamepadControls[c]);
            if (buttonVal)
            {
                main.controllerMode = true;
                return buttonVal;
            }
        }
        if (main.keyboardControls.keyBinds.ContainsKey(c))
        {
            buttonVal = Input.GetKey(main.keyboardControls[c]);
            if (!buttonVal && main.keyboardControls.secondaryKeyBinds.ContainsKey(c))
                buttonVal = Input.GetKey(main.keyboardControls.secondaryKeyBinds[c]);
            if (buttonVal)
                main.controllerMode = false;
        }
        return buttonVal;
    }
    public static bool GetButton(Control c, InputAxis a)
    {
        bool buttonVal = GetButton(c);
        if (buttonVal)
            return buttonVal;
        if(main.gamepadControls.axes.ContainsKey(a) && main.axisButtons.ContainsKey(main.gamepadControls[a]))
        {
            buttonVal = main.axisButtons[main.gamepadControls[a]].GetButton();
            if (buttonVal)
                main.controllerMode = true;
        }
        return buttonVal;
    }
    public static bool GetButtonDown(Control c)
    {
        bool buttonVal = false;
        if (main.gamepadControls.keyBinds.ContainsKey(c))
        {
            buttonVal = Input.GetKeyDown(main.gamepadControls[c]);
            if (buttonVal)
            {
                main.controllerMode = true;
                return buttonVal;
            }
        }
        if (main.keyboardControls.keyBinds.ContainsKey(c))
        {
            buttonVal = Input.GetKeyDown(main.keyboardControls[c]);
            if (!buttonVal && main.keyboardControls.secondaryKeyBinds.ContainsKey(c))
                buttonVal = Input.GetKeyDown(main.keyboardControls.secondaryKeyBinds[c]);
            if (buttonVal)
                main.controllerMode = false;
        }
        return buttonVal;
    }
    public static bool GetButtonDown(Control c, InputAxis a)
    {
        bool buttonVal = GetButtonDown(c);
        if (buttonVal)
            return buttonVal;
        if (main.gamepadControls.axes.ContainsKey(a) && main.axisButtons.ContainsKey(main.gamepadControls[a]))
        {
            buttonVal = main.axisButtons[main.gamepadControls[a]].GetButtonDown();
            if (buttonVal)
                main.controllerMode = true;
        }
        return buttonVal;
    }
    public static bool GetButtonUp(Control c)
    {
        bool buttonVal = false;
        if(main.gamepadControls.keyBinds.ContainsKey(c))
        {
            buttonVal = Input.GetKeyUp(main.gamepadControls[c]);
            if (buttonVal)
            {
                main.controllerMode = true;
                return buttonVal;
            }
        }
        if(main.keyboardControls.keyBinds.ContainsKey(c))
        {
            buttonVal = Input.GetKeyUp(main.keyboardControls[c]);
            if (!buttonVal && main.keyboardControls.secondaryKeyBinds.ContainsKey(c))
                buttonVal = Input.GetKeyUp(main.keyboardControls.secondaryKeyBinds[c]);
            if (buttonVal)
                main.controllerMode = false;
        }
        return buttonVal;
    }
    public static bool GetButtonUp(Control c, InputAxis a)
    {
        bool buttonVal = GetButtonUp(c);
        if (buttonVal)
            return buttonVal;
        if (main.gamepadControls.axes.ContainsKey(a) && main.axisButtons.ContainsKey(main.gamepadControls[a]))
        {
            buttonVal = main.axisButtons[main.gamepadControls[a]].GetButtonUp();
            if (buttonVal)
                main.controllerMode = true;
        }
        return buttonVal;
    }

    public void SetKeyboardProfile(ControlProfile c)
    {
        keyboardControls = c;
    }
    public void SetGamepadProfile(ControlProfile c)
    {
        gamepadControls = c;
    }

    [System.Serializable] public class ControlDict : SerializableCollections.SDictionary<string, ControlProfile> { }
    public class AxisButton
    {
        public enum Mode
        {
            Positive,
            Negative,
            Both,
        }
        public Mode mode;
        public AxisName axis;
        private string axisName;
        private bool InputDown
        {
            get
            {
                var axisVal = Input.GetAxisRaw(axisName);
                if (mode == Mode.Both)
                    return axisVal != 0;
                if (mode == Mode.Positive)
                    return axisVal > 0;
                if (mode == Mode.Negative)
                    return axisVal < 0;
                return false;
            }
        }
        private bool lastValue = false;

        public AxisButton(AxisName axis, Mode mode)
        {
            this.axis = axis;
            this.mode = mode;
            axisName = axis.ToString().Replace("Positive", string.Empty).Replace("Negative", string.Empty);
        }

        public bool GetButton()
        {
            return InputDown;
        }

        public bool GetButtonDown()
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

        public bool GetButtonUp()
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="newControlProfile", menuName ="Control Profile")]
public class ControlProfile : ScriptableObject
{
    public enum InputSource
    {
        Gamepad,
        Keyboard,
        DKBongos,
    }

    public string displayName;
    public InputSource inputSource;
    public AxisDict axes = new AxisDict()
    {
        {InputAxis.Horizontal, string.Empty},
        {InputAxis.Vertical, string.Empty},
        {InputAxis.LeftTrigger, string.Empty},
        {InputAxis.RightTrigger, string.Empty},
    };
    public KeyCodeDict keyBinds = new KeyCodeDict()
    {
        {Control.Camp, default},
        {Control.Cancel, default},
        {Control.Confirm, default},
        {Control.Cook, default},
        {Control.Dash, default},
        {Control.Eat, default},
        {Control.Interact, default},
        {Control.Knife, default},
        {Control.Pause, default},
        {Control.Skewer, default},
        {Control.Throw, default},
        {Control.Up, KeyCode.W},
        {Control.Down, KeyCode.S},
        {Control.Left, KeyCode.A},
        {Control.Right, KeyCode.D},
    };
    public KeyCode this[Control c]
    {
        get
        {
            return keyBinds[c];
        }
    }
    [System.Serializable] public class KeyCodeDict : SerializableCollections.SDictionary<Control, KeyCode> { }
    [System.Serializable] public class AxisDict : SerializableCollections.SDictionary<InputAxis, string> { }
}

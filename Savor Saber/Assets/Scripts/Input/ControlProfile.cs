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
    public KeySpriteDict sprites = new KeySpriteDict();
    public AxisDict axes = new AxisDict()
    {
        {InputAxis.Horizontal, default},
        {InputAxis.Vertical, default},
        {InputAxis.Dash, default},
        {InputAxis.Skewer, default},
        {InputAxis.Slash, default},
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
    public AxisDict secondaryAxes = new AxisDict() { };
    public KeyCodeDict secondaryKeyBinds = new KeyCodeDict() { };
    public KeyCode this[Control c]
    {
        get
        {
            return keyBinds[c];
        }
    }
    public AxisName this[InputAxis a]
    {
        get
        {
            return axes[a];
        }
    }
    [System.Serializable] public class KeySpriteDict : SerializableCollections.SDictionary<Control, Sprite> { }
    [System.Serializable] public class KeyCodeDict : SerializableCollections.SDictionary<Control, KeyCode> { }
    [System.Serializable] public class AxisDict : SerializableCollections.SDictionary<InputAxis, AxisName> { }
}

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
    public AxisDict axes = new AxisDict();
    public KeyCodeDict keyBinds = new KeyCodeDict();
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

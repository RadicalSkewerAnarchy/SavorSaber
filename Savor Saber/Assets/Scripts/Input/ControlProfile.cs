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
    };
    public KeyCode this[Control c]
    {
        get
        {
            return keyBinds[c];
        }
    }
    [System.Serializable] public class KeyCodeDict : SerializableCollections.SDictionary<Control, KeyCode> { }
}

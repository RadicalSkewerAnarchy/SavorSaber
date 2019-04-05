using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelect : MonoBehaviour
{
    public Player2Data[] characters;
    public int selected;
    public Text nameText;
    public Image image;

    private void Awake()
    {
        selected = characters.Length / 2;
        SetSelected();
    }

    private void Update()
    {
        float axis = Input.GetAxis("Player2Horizontal");
        if (axis < -0.5)
        {
            if (selected > 0)
            {
                --selected;
                SetSelected();
            }
        }
        else if (axis > 0.5)
        {
            if (selected < characters.Length - 1)
            {
                ++selected;
                SetSelected();
            }
        }
        if (Input.GetKeyDown(KeyCode.Joystick2Button7))
        {
        }

    }

    private void SetSelected()
    {
        nameText.text = characters[selected].displayName;
        image.overrideSprite = characters[selected].charaSelectImage;
    }
}

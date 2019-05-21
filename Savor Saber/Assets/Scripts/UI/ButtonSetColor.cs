using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSetColor : MonoBehaviour
{
    public Color selectedNormal = Color.white;
    public Color selectedHighlight = Color.white;
    public Color normal = Color.white;
    public Color highlight = Color.white;
    private Button b;
    // Start is called before the first frame update
    void Start()
    {
        b = GetComponent<Button>();
    }

    public void SetSelected()
    {
        if(b == null)
            b = GetComponent<Button>();
        var colors = new ColorBlock()
        {
            colorMultiplier = b.colors.colorMultiplier,
            disabledColor = b.colors.disabledColor,
            normalColor = selectedNormal,
            pressedColor = b.colors.pressedColor,
            highlightedColor = selectedHighlight,
            fadeDuration = b.colors.fadeDuration,
        };
        b.colors = colors;
    }

    public void SetUnselected()
    {
        if (b == null)
            b = GetComponent<Button>();
        var colors = new ColorBlock()
        {
            colorMultiplier = b.colors.colorMultiplier,
            disabledColor = b.colors.disabledColor,
            normalColor = normal,
            pressedColor = b.colors.pressedColor,
            highlightedColor = highlight,
            fadeDuration = b.colors.fadeDuration,
        };
        b.colors = colors;
    }
}

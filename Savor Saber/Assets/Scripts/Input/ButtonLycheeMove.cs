using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonLycheeMove : MonoBehaviour, IPointerEnterHandler
{
    public Image cursorSprite;

    public void OnPointerEnter(PointerEventData eventData)
    {
        MoveLychee();
    }

    public void MoveLychee()
    {
        Debug.Log("selected");
        cursorSprite.rectTransform.anchoredPosition = this.transform.localPosition - new Vector3(220, -16, 0);
    }
}

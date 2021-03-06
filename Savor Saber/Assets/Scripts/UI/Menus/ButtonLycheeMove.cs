﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonLycheeMove : MonoBehaviour, IPointerEnterHandler
{
    public Image cursorSprite;
    public bool startSelected;
    private AudioSource selectSound;

    void Start()
    {
        if (startSelected)
        {
            MoveLychee();
        }
        selectSound = GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MoveLychee();
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(this.gameObject);
    }

    public void MoveLychee()
    {
        Debug.Log("selected");
        cursorSprite.rectTransform.localPosition = this.transform.localPosition - new Vector3(220, -8, 0);

        if(selectSound != null)
        {
            selectSound.Play();
        }
    }
}

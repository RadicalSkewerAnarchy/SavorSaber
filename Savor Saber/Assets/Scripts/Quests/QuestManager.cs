﻿using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    [SerializeField] private Text text;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }

    public string GetText()
    {
        return text.text;
    }
}

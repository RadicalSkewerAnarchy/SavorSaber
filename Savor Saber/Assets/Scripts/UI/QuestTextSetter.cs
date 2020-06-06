using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTextSetter : MonoBehaviour
{
    private QuestManager questManager;
    // Start is called before the first frame update
    void Start()
    {
        questManager = GameObject.FindObjectOfType<QuestManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateQuestText(string text)
    {
        questManager.SetText(text);
    }
}

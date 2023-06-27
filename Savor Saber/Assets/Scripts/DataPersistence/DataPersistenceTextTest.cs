using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataPersistenceTextTest : MonoBehaviour, IDataPersistence
{
    private int count;
    public Text countText;

    // Start is called before the first frame update
    void Start()
    {
        countText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            count++;
            DisplayCount();
        }
    }
    public void LoadData(GameData data)
    {
        this.count = data.testField;
        DisplayCount();
    }
    public void SaveData(ref GameData data)
    {
        data.testField = this.count;
    }

    private void DisplayCount()
    {
        countText.text = "" + count;
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestManager : MonoBehaviour, IDataPersistence
{
    public static QuestManager instance;
    [SerializeField] private TextMeshProUGUI text;

    public AudioSource updateSFXPlayer;

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

    public void LoadData(GameData data)
    {
        SetText(data.questText);
    }


    public void SaveData(ref GameData data)
    {
        data.questText = text.text;
    }

    public void SetText(string text)
    {
        this.text.text = TextMacros.instance.Parse(text);
        if (!updateSFXPlayer.isPlaying)
        {
            updateSFXPlayer.Play();
        }
        
    }

    public string GetText()
    {
        return text.text;
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    [SerializeField] private TextMeshProUGUI text;

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
        this.text.text = TextMacros.instance.Parse(text);
    }

    public string GetText()
    {
        return text.text;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class FlagManagerDisplay : MonoBehaviour
{
    public FlagManager flagMan;
    public TextMeshProUGUI debugText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Dictionary<string, string> dict = flagMan.GetInstanceFlagDictionary();
        debugText.text = string.Join("\n", dict.Select(x => x.Key + " = " + x.Value).ToArray());
    }
}

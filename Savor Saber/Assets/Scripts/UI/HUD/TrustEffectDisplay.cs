using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TrustEffectDisplay : MonoBehaviour
{
    public Text trustText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDisplayText(string text)
    {
        trustText.text = text;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHP : MonoBehaviour
{

    public CharacterData playerData;
    public RectTransform barCover;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float hpScale = 1.0f - ((float)playerData.health / (float)playerData.maxHealth);
        barCover.transform.localScale = new Vector3(hpScale, 1.0f, 1.0f);
    }
}

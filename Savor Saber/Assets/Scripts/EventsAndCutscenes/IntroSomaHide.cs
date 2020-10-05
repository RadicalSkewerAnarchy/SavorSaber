using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSomaHide : MonoBehaviour
{
    [SerializeField]
    private GameObject soma;
    private SpriteRenderer somaRenderer;

    // Start is called before the first frame update
    void Start()
    {
        GetPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GetPlayer()
    {
        soma = GameObject.Find("Soma");
        if (soma == null)
        {
            Debug.LogError("No instance of Soma detected");
        }
        somaRenderer = soma.GetComponent<SpriteRenderer>();
    }

    public void HidePlayer()
    {
        if(somaRenderer == null)
            GetPlayer();
        somaRenderer.enabled = false;
    }

    public void ShowPlayer()
    {
        if (somaRenderer == null)
            GetPlayer();
        somaRenderer.enabled = true;
    }


}

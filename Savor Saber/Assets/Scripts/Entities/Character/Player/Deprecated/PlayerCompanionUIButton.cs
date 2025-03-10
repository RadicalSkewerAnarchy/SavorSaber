using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerCompanionUIButton : MonoBehaviour
{
    /// <summary>
    /// The ingredient data containing the fruitant morph to be spawned by this button
    /// </summary>
    public IngredientData fruitantData;
    public PlayerCompanionSummon summoner;
    private AudioSource sfx;
    private RectTransform rect;
    private Vector3 baseRectScale;
    public Image silhouette;
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        sfx = GetComponent<AudioSource>();
        baseRectScale.x = rect.localScale.x;
        baseRectScale.y = rect.localScale.y;
        baseRectScale.z = rect.localScale.z;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SummonCompanion()
    {
        summoner.SummonCompanion(fruitantData.displayName);
        summoner.CloseUI();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(this.gameObject);
        ExpandButton();
    }

    public void ExpandButton()
    {
        rect.localScale *= 1.05f;
        sfx.Play();
        //Debug.Log("Pointer over button");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(this.gameObject);
        ShrinkButton();
    }

    public void ShrinkButton()
    {
        rect.localScale = baseRectScale;
    }
}

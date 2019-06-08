using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditClicker : MonoBehaviour
{
    public List<GameObject> credits;
    public GameObject currentCredit;
    public int creditSequence = 0;

    public Transform stumpPosition;
    public float spacing = 0;
    public float rise = 0;
    public float fall = 0;

    public Text nameText;
    public Text roleText;
    public Text titleText;

    private bool nextCredit = true;

    // Start is called before the first frame update
    void Start()
    {
        currentCredit = credits[creditSequence];
        Wiggle(currentCredit, true);

        StumpPosition();
    }

    // Update is called once per frame
    void Update()
    {
        float select = InputManager.GetAxis(InputAxis.Horizontal);
        if (select != 0 && nextCredit)
        {
            select = Mathf.Sign(select);

            creditSequence = (int)(creditSequence + select);
            if (creditSequence >= credits.Count) creditSequence = 0;
            else if (creditSequence < 0) creditSequence = credits.Count - 1;

            SetCredit();

            StumpPosition();

            nextCredit = false;
        }
        else if (select == 0)
        {
            nextCredit = true;
        }
    }

    void StumpPosition()
    {
        // set currentcredit to stump
        currentCredit.transform.position = stumpPosition.position;
        currentCredit.transform.position += new Vector3(0, rise);

        // set everyone elses
        for (int i = 0; i < credits.Count; i++)
        {
            if (credits[i] == currentCredit)
                continue;

            credits[i].transform.position = currentCredit.transform.position;
            int dist = (i - creditSequence);
            credits[i].transform.position += new Vector3(dist * spacing, fall - (Mathf.Abs(dist)*Mathf.Abs(dist))/10f );
        }
    }

    void SetCredit()
    {
        Wiggle(currentCredit, false);
        currentCredit = credits[creditSequence];
        Wiggle(currentCredit, true);
    }

    void Wiggle(GameObject g, bool act)
    {
        Squisher squish = g.GetComponent<Squisher>();
        if (squish != null)
        {
            squish.activate = act;
            nameText.text = squish.nameText;
            roleText.text = squish.roleText;
            titleText.text = squish.titleText;
            roleText.fontSize = squish.roleFontSize;
        }
    }



}





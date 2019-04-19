using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogPlayer : MonoBehaviour
{
    public enum State
    {
        Inactive,
        Scrolling,
        Cancel,
        Waiting,
    }

    #region fields
    public bool Visible { get => dialogBox.activeInHierarchy; set => dialogBox.SetActive(value); }

    /// <summary>
    /// Fields for text scrolling
    /// </summary>
    protected State state = State.Inactive;
    public float typeSpeed = 0.03f;

    /// <summary>
    /// references to objects used for display
    /// </summary>
    public GameObject dialogBoxPrefab;
    public Canvas UICanvas;
    protected GameObject dialogBox;
    protected RectTransform dialogRectTransform;

    protected Text dialogText;
    protected Image dialogImage;
    private AudioSource audioPlayer;
    #endregion

    public void Initialize()
    {
        if (dialogBoxPrefab != null)
        {
            //set position on of dialog box on screen
            dialogBox = Instantiate(dialogBoxPrefab, Vector3.zero, Quaternion.identity);
            dialogBox.transform.SetParent(UICanvas.transform);
            dialogRectTransform = dialogBox.GetComponent<RectTransform>();
            dialogBox.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            dialogText = dialogBox.transform.GetChild(2).GetComponent<Text>();
            audioPlayer = dialogBox.GetComponent<AudioSource>();
            //set dialog box portrait
            Transform portrait = dialogBox.transform.GetChild(1);
            dialogImage = portrait.GetComponent<Image>();
            Visible = false;
        }
        else
        {
            Debug.LogWarning("Warning: No dialog box prefab found when trying to activate Dialog");
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (state == State.Inactive)
            return;
        if (InputManager.GetButtonDown(Control.Confirm))
            state = state.Next();
    }

    public void Cleanup()
    {
        Destroy(dialogBox.gameObject);
    }

    private void InitializeNext(DialogItem item, DialogData actor)
    {
        if (state != State.Inactive)
            StopAllCoroutines();
        var dialogData = actor.GetComponent<DialogData>();
        dialogImage.sprite = dialogData.portraitDictionary[item.emotion];
        if (dialogData.textBlipSound != null)
            audioPlayer.clip = dialogData.textBlipSound;
        Visible = true;
    }

    public IEnumerator NextDialogBubble(DialogItem item, DialogData actor)
    {
        dialogRectTransform.anchoredPosition = GetActorUISpace(actor.gameObject);
        InitializeNext(item, actor);
        yield return StartCoroutine(Scroll(item.text));
    }

    public IEnumerator NextDialogFixed(DialogItem item, DialogData actor)
    {
        dialogRectTransform.anchoredPosition = new Vector3(0, 50, 0);
        InitializeNext(item, actor);
        yield return StartCoroutine(Scroll(item.text));
    }

    public IEnumerator Scroll(string lineOfText)
    {
        var tagOffsets = new List<int>();
        var tags = ParseTags(lineOfText, out tagOffsets);
        int tagInd = 0;
        bool inTag = false;

        int count = 0; // need separate count with letter jumping around from tags
        int letter = 0;
        dialogText.text = "";
        state = State.Scrolling;
        while (state != State.Cancel && (letter < lineOfText.Length - 1))
        {
            if(tagInd < tagOffsets.Count && letter == tagOffsets[tagInd])
            {
                if(inTag)
                {
                    letter += tags[tagInd++].Length;
                    inTag = false;
                    continue;
                }
                else
                {
                    letter += tags[tagInd].Length;
                    dialogText.text += tags[tagInd] + tags[++tagInd];
                    inTag = true;
                    continue;
                }
            }
            if (inTag)
                dialogText.text = dialogText.text.Insert(letter, lineOfText[letter++].ToString());
            else
                dialogText.text += lineOfText[letter++];
            count = (count + 1) % 3;
            if (count == 0)
                audioPlayer.Play();
            yield return new WaitForSeconds(typeSpeed);
        }
        dialogText.text = lineOfText;
        state = State.Waiting;
        yield return new WaitWhile(() => state == State.Waiting);
        state = State.Inactive;
    }
    private List<string> ParseTags(string line, out List<int> offsets)
    {
        offsets = new List<int>();
        var tags = new List<string>();
        int firstInd = line.IndexOf('<', 0);
        if (firstInd == -1)
            return tags;

        for (int i = firstInd; i < line.Length;)
        {
            i = line.IndexOf('<', i);
            int j = line.IndexOf('>', i);
            string tag = line.Substring(i, (j - i) + 1);
            offsets.Add(i);
            tags.Add(tag);
            i = j + 1;
        }
        return tags;
    }
    protected Vector2 GetActorUISpace(GameObject actor)
    {
        RectTransform canvasRect = UICanvas.GetComponent<RectTransform>();
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(actor.transform.position);
        Vector2 UIOffset = new Vector2((float)canvasRect.sizeDelta.x / 2f, -70f);
        Vector2 proportionalPosition = new Vector3(viewportPosition.x * canvasRect.sizeDelta.x, viewportPosition.y * canvasRect.sizeDelta.y);
        return proportionalPosition - UIOffset;
    }

}

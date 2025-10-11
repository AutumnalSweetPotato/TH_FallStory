using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Single<DialogueManager>
{
    public GameObject dialogueBox;
    public Text dialogueText, nameText;

    [TextArea(1, 3)]
    public string[] dialogueLines;
    [SerializeField] private int currentLine;
    [SerializeField] private float textSpeed;

    private bool isScrolling;

    public Questable currentQuestable;

    public QuestTarget questTarget;
    protected override void Awake()
    {
        base.Awake();
        dialogueText.text = string.Empty;
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && dialogueBox.activeSelf && !isScrolling)
        {
            currentLine++;

            if (currentLine < dialogueLines.Length)
            {
                CheckName();
                //dialogueText.text = dialogueLines[currentLine];
                StartCoroutine(ScrollingText());
            }
            else
            {
                CanvasGroup canvasGroup = dialogueBox.GetComponent<CanvasGroup>();
                StartCoroutine(FadeManager.Instance.FadeIn(canvasGroup));
                Player.Instance.EnablePlayerInput();
                StartCoroutine(DelayHide(1f));

                if (currentQuestable != null)
                {
                    currentQuestable.DelegateQuest();
                    QuestManager.Instance.UpdateQuestList();
                }
                else
                {

                }


            }
        }
    }
    
    public void ShowDialogue(string[] newLines, bool hasName)
    {
        dialogueBox.SetActive(true);
        dialogueLines = newLines;
        currentLine = 0;
        CheckName();
        //dialogueText.text = dialogueLines[currentLine];
        StartCoroutine(ScrollingText());
        nameText.gameObject.SetActive(hasName);
        Player.Instance.DisablePlayerInput();
        CanvasGroup canvasGroup =  dialogueBox.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        StartCoroutine(FadeManager.Instance.FadeOut(canvasGroup));
    }
    private void CheckName()
    {
        if (dialogueLines[currentLine].StartsWith("n-"))
        {
            nameText.text = dialogueLines[currentLine].Substring(2);
            currentLine++;

        }
    }

    private IEnumerator ScrollingText()
    {
        isScrolling = true;
        dialogueText.text = "";
        foreach (char c in dialogueLines[currentLine].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        isScrolling = false;
    }
    private IEnumerator DelayHide(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        dialogueBox.SetActive(false);
    }
}

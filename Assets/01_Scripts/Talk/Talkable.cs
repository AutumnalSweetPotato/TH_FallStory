using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talkable : MonoBehaviour
{
    [SerializeField] private bool isEntered;
    [SerializeField] public bool hasName;

    public Questable questable;
    public QuestTarget questTarget;

    [Header("�Ի�����")]
    [TextArea(1, 3)] public string[] lines;
    [TextArea(1, 3)] public string[] congratsLines; //������ɺ�ĶԻ�
    [TextArea(1, 3)] public string[] completedLines; //������ɺ�ĶԻ�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isEntered = true;
            if (questable != null)
            {
                DialogueManager.Instance.currentQuestable = questable;
                DialogueManager.Instance.questTarget = questTarget;
                DialogueManager.Instance.talkable = this;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isEntered = false;
            DialogueManager.Instance.currentQuestable = null;


        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isEntered && !DialogueManager.Instance.dialogueBox.activeSelf)
        {
            if (questable == null)
            {
                DialogueManager.Instance.ShowDialogue(lines, hasName);
            }
            else
            {
                if(questable.quest.questStatus == QuestStatus.completed)
                {
                    DialogueManager.Instance.ShowDialogue(completedLines, hasName);
                }
                else
                {
                    DialogueManager.Instance.ShowDialogue(lines, hasName);
                }
            }


        }
    }


}

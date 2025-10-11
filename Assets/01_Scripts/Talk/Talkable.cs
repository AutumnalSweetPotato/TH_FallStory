using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talkable : MonoBehaviour
{
    [SerializeField] private bool isEntered;
    [SerializeField] private bool hasName;
    [TextArea(1,3)]public string[] talkLines;
    public Questable questable;
    public QuestTarget questTarget;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isEntered = true;
            if(questable != null)
            {
                DialogueManager.Instance.currentQuestable = questable;
                DialogueManager.Instance.questTarget = questTarget;
                
                
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
        if(Input.GetKeyDown(KeyCode.Space) && isEntered && !DialogueManager.Instance.dialogueBox.activeSelf)
        {
            DialogueManager.Instance.ShowDialogue(talkLines,hasName);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTarget : MonoBehaviour
{
    public string questName;
    public enum QuestType { Gathering, Talk, Reach };
    public QuestType questType;

    [Header("Gathering Type Quest")]
    public int amount = 1;//

    [Header("Talk Type Quest")]
    public bool hasTalked;

    [Header("Reach Type Quest")]
    public bool hasReached;

    //MARKER This method will be called once we have completed the quest. Touch the Collider
    //这个方法将会在完成任务之后被调用，比如说我们收集整齐了所有的物品，或者和
    public void QuestComplete()
    {
        if (Player.Instance.questDictionary.TryGetValue(questName, out Quest quest))
        {
            if (quest.questStatus == QuestStatus.accepted)
            {
                switch (questType)
                {
                    case QuestType.Gathering:
                        if (Player.Instance.itemAcount >= quest.requireAmount)
                        {
                            quest.questStatus = QuestStatus.completed;
                            QuestManager.Instance.UpdateQuestList();
                        }
                        break;
                    case QuestType.Reach:
                        if(hasReached)
                        {
                            quest.questStatus = QuestStatus.completed;
                            QuestManager.Instance.UpdateQuestList();
                        }
                        break;
                    case QuestType.Talk:
                        if (hasTalked)
                        {
                            quest.questStatus = QuestStatus.completed;
                            QuestManager.Instance.UpdateQuestList();
                        }
                        break;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (questType == QuestType.Gathering)
            {
                Player.Instance.itemAcount += amount;
                QuestComplete();
            }
            else if (questType == QuestType.Reach)
            {
                hasReached = true;
                QuestComplete();
            }
            
        }
    }
}

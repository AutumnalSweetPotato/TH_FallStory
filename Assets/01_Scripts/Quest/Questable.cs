using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Questable : MonoBehaviour
{
    public Quest quest;
    public void DelegateQuest()
    {
        if(quest.questStatus == QuestStatus.waitting)
        {
            quest.questStatus = QuestStatus.accepted;
            Player.Instance.questDictionary.Add(quest.questName, quest);
        }
        else
        {
            Debug.Log("任务："+quest.questName+"已经领取！");
        }
    }
}

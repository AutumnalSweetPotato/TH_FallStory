using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : Single<QuestManager>
{
    public GameObject[] questUIArray;
    protected override void Awake()
    {
        base.Awake();
    }

    public void UpdateQuestList()
    {
        int i = 0;
        foreach(var quest in Player.Instance.questDictionary)
        {
            questUIArray[i].transform.GetChild(0).GetComponent<Text>().text = quest.Value.questName;
            questUIArray[i].transform.GetChild(1).GetComponent<Text>().text = quest.Value.questStatus.ToString();
            i++;
        }
    }
}

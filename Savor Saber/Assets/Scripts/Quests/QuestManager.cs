using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestManager : MonoBehaviour
{
    public List<Quest> quests;

    public void SetDone(string questName, bool done)
    {
        var quest = FindQuest(questName);
        if (quest != null)
        {
            quest.isDone = done;
        }
    }
    public void SetDone(QuestData data, bool done)
    {
        var quest = FindQuest(data);
        if (quest != null)
        {
            quest.isDone = done;
        }
    }

    public bool TurnIn(string questName)
    {
        var quest = FindQuest(questName);
        if (quest != null)
        {
            if (!quest.isDone)
                return false;

        }
        return false;
    }
    public bool TurnIn(QuestData data)
    {
        var quest = FindQuest(data);
        if (quest != null)
        {
            return false;
        }
        return false;
    }

    private Quest FindQuest(string questName)
    {
        return quests.FirstOrDefault((q) => q.data.displayName == questName);
    }

    private Quest FindQuest(QuestData data)
    {
        return quests.FirstOrDefault((q) => q.data == data);
    }

    public class Quest
    {
        public QuestData data;
        public bool isDone;
    }
}

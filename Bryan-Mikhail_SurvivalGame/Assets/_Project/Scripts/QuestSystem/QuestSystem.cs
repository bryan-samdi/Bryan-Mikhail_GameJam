using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestSystem : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    [System.Serializable]
    public class Quest
    {
        public string questName;
        public string questDescription;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        SetQuest("Escape The Volacno2");
    }

    public List<Quest> quests = new List<Quest>();

    public void SetQuest(string _questName)
    {
        for (int i = 0; i < quests.Count; i++)
        {
            if (quests[i].questName == _questName)
            {
                SetQuestData(quests[i].questName, quests[i].questDescription);
                return;
            }
        }
        Debug.LogError($"Quest name typed as {_questName}, which does not exit in database");
    }

    void SetQuestData(string _questName, string _questDescription)
    {
        nameText.text = _questName;
        descriptionText.text = _questDescription;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private QuestDB questDb;                        //����Ʈ DB
    private QuestObject questObject;                //����Ʈ NPC������Ʈ
    private Dictionary<int, AcceptQuest> quests;    //������ ����Ʈ ���� <����ƮID, ������ ����Ʈ ������>
    public QuestDB QuestDB => questDb;
    public QuestObject QuestObject => questObject;
    public Dictionary<int, AcceptQuest> AccetpQuest => quests;

    public void Initialize()
    {
        questDb = new QuestDB();
        quests = new Dictionary<int, AcceptQuest>();
        questDb.Initialize();
    }

    public void SetQuestObject(QuestObject obj)
    { 
        questObject = obj;

        foreach (AcceptQuest obs in quests.Values)
        {
            questObject.SetKey = obs.Key;
            return;
        }
    }  //����Ʈ ������Ʈ ����

    /// <summary>
    /// key : ����Ʈ ������ Ű
    /// </summary>
    public bool AddQuest(int questID)
    {
        //����Ʈ �����͸� ã�� ���� ��� false
        if (!questDb.KeyCheck(questID)) return false;

        //����Ʈ ���� ��� ����
        if (!quests.ContainsKey(questID))
        {
            AcceptQuest addQuest = new AcceptQuest();
            quests[questID] = addQuest;
        }

        //������ ����
        quests[questID].SetData(questDb.GetQuest(questID));
        return true;
    }   //����Ʈ �߰�

    public void AddCount(QuestAddValueType type)
    {
        foreach (AcceptQuest obs in quests.Values)
        {
            obs.AddCount(type);
        }
    }    //Ÿ�Կ� ���� �� �� ����    //�÷��̾�� �۵�

    public void Save()
    {
        GameManager.Instance.SaveLoadManager.GetPlayerData().ResetQuedstData();
        foreach (AcceptQuest quest in quests.Values)
        {
            GameManager.Instance.SaveLoadManager.GetPlayerData().SaveQuest(quest.Key, quest.QuestCount);
        }
    }   //������ ����Ʈ ����

    public void Load(int questID, int count)
    {
        AddQuest(questID);
        quests[questID].SetCount = count;
        GameManager.Instance.GetUIManager().GetPlayerUI.GetPlayerPanel.GetAcceptQuest.AddQuestSlot(questID);
    }   //����Ʈ ������ �ε�

    public void UpdateQuest()
    {
        foreach (AcceptQuest obs in quests.Values)
        {
            obs.Notify();
        }
    }   //������ ������ Update
}

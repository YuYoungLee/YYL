using System.Collections.Generic;
using System.Text.RegularExpressions;

public enum QuestType
{
    QuestID,
    QuestTitle,
    QuestExplainText,
    NextQuestID,
    QuestClearValue,
    RewardType,
    RewardValue,
    QuestType_End
}

public class QuestDB
{
    private Dictionary<int, QuestData> questDB;     //����Ʈ Key, Quest ������

    public void Initialize()
    {
        questDB = new Dictionary<int, QuestData>();
        ReadFile();
    }

    private void ReadFile()
    {
        string[] lines = CSVReader.Read("Script/QuestSystem/QuestText");
        string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
        
        for (int i = 1; i < lines.Length; i++)
        {
            if (lines[i].Length == 0) continue;
            string[] values = Regex.Split(lines[i], SPLIT_RE);
            QuestData quest = new QuestData();
            quest.SetData(values);
            questDB.Add(quest.QuestID, quest);
        }
    }   //csv ���� �б�

    public bool KeyCheck(int key)
    {
        return questDB.ContainsKey(key) ? true : false;
    }   //Ű üũ

    public QuestData GetQuest(int key)
    {
        return questDB[key];
    }   //���Ϲ��� ����Ʈ

    public int GetReward(int key)
    {
        return questDB[key].RewardValue;
    }

    public int GetNextQuestID(int questkey)
    {
        foreach (int key in questDB.Keys)
        {
            if (questDB[key].PriviousID == questkey) { return questDB[key].QuestID; }
        }
        return 9999;    //����Ʈ�� ã�� �� ���� ��
    }
}
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
    private Dictionary<int, QuestData> questDB;     //퀘스트 Key, Quest 데이터

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
    }   //csv 파일 읽기

    public bool KeyCheck(int key)
    {
        return questDB.ContainsKey(key) ? true : false;
    }   //키 체크

    public QuestData GetQuest(int key)
    {
        return questDB[key];
    }   //리턴받을 퀘스트

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
        return 9999;    //퀘스트를 찾지 못 했을 때
    }
}
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private QuestDB questDb;                        //퀘스트 DB
    private QuestObject questObject;                //퀘스트 NPC오브젝트
    private Dictionary<int, AcceptQuest> quests;    //수락한 퀘스트 관리 <퀘스트ID, 수락한 퀘스트 데이터>
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
    }  //퀘스트 오브젝트 설정

    /// <summary>
    /// key : 퀘스트 구분할 키
    /// </summary>
    public bool AddQuest(int questID)
    {
        //퀘스트 데이터를 찾지 못할 경우 false
        if (!questDb.KeyCheck(questID)) return false;

        //퀘스트 없을 경우 생성
        if (!quests.ContainsKey(questID))
        {
            AcceptQuest addQuest = new AcceptQuest();
            quests[questID] = addQuest;
        }

        //데이터 설정
        quests[questID].SetData(questDb.GetQuest(questID));
        return true;
    }   //퀘스트 추가

    public void AddCount(QuestAddValueType type)
    {
        foreach (AcceptQuest obs in quests.Values)
        {
            obs.AddCount(type);
        }
    }    //타입에 맞을 때 값 증가    //플레이어에서 작동

    public void Save()
    {
        GameManager.Instance.SaveLoadManager.GetPlayerData().ResetQuedstData();
        foreach (AcceptQuest quest in quests.Values)
        {
            GameManager.Instance.SaveLoadManager.GetPlayerData().SaveQuest(quest.Key, quest.QuestCount);
        }
    }   //수락한 퀘스트 저장

    public void Load(int questID, int count)
    {
        AddQuest(questID);
        quests[questID].SetCount = count;
        GameManager.Instance.GetUIManager().GetPlayerUI.GetPlayerPanel.GetAcceptQuest.AddQuestSlot(questID);
    }   //퀘스트 데이터 로드

    public void UpdateQuest()
    {
        foreach (AcceptQuest obs in quests.Values)
        {
            obs.Notify();
        }
    }   //구독한 옵저버 Update
}

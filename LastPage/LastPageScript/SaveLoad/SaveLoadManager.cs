using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

public enum SaveSlot{
    Slot1,
    Slot2,
    Slot3,
}

[System.Serializable]
public class PlayerData
{
    public int level = 1;
    public int money = 0;
    public int exe = 0;

    public List<int> item = new List<int>();      //아이템 저장 key, count

    public List<int> acceptQuest = new List<int>(); //퀘스트 저장

    public void Save(PlayerStat stat)
    {
        level = stat.Level;
        money = stat.Money;
        exe = stat.Exe;
    }   //플레이어 데이터 저장
    public void ResetItemData()
    {
        item.Clear();
    }
    public void SaveItem(int key, int count)
    {
        item.Add(key);
        item.Add(count);
    }   //아이템 데이터 저장
    public void ResetQuedstData()
    {
        acceptQuest.Clear();
    }
    public void SaveQuest(int key, int count)
    {
        acceptQuest.Add(key);
        acceptQuest.Add(count);
    }   //퀘스트 데이터 저장
}
public class SaveLoadManager : MonoBehaviour
{
    private SaveSlot slotType;
    private PlayerData[] saveData = new PlayerData[3];
    private string path = Application.streamingAssetsPath + "/";

    public PlayerData GetPlayerData() 
    {
        if (saveData[(int)slotType] == null) saveData[(int)slotType] = new PlayerData();
        return saveData[(int)slotType]; 
    }

    public SaveSlot SetType { set { slotType = value; } }

    public void SaveFile()
    {
        GameManager.Instance.GetUIManager().StartSaveCoroutine();
        string data = JsonUtility.ToJson(saveData[(int)slotType]);
        File.WriteAllText(path + slotType.ToString(), data);
    }   //파일로 저장

    public void LoadFile()
    {
        for(int i = 0; i < saveData.Length; ++i)
        {
            //파일이 없으면 다음 파일을 찾기
            if (!FileCheck((SaveSlot)i)) continue;
            slotType = (SaveSlot)i;
            string data = File.ReadAllText(path + slotType.ToString());

            if (saveData[i] == null) saveData[i] = new PlayerData();
            saveData[i] = JsonUtility.FromJson<PlayerData>(data);
        }
    }   //세이브 파일 로드

    public void InitData()
    {
        if (!FileCheck(slotType)) return;   //로드할 파일이 없으면 리턴

        GameManager.Instance.GetPlayer().Load(saveData[(int)slotType]); //플레이어 데이터 로드

        GameManager.Instance.GetUIManager().GetPlayerUI.PlayerPanel.GetInventory.ClearSlot();
        for (int i = 0; i < saveData[(int)slotType].item.Count; i += 2)
        {
            GameManager.Instance.GetUIManager().GetPlayerUI.PlayerPanel.GetInventory.InitItem(
            GameManager.Instance.GetItemManager.GetItem(saveData[(int)slotType].item[i]), saveData[(int)slotType].item[i + 1]);
        }   //아이템 데이터 로드

        GameManager.Instance.GetUIManager().GetPlayerUI.PlayerPanel.GetAcceptQuest.ClearSlot();
        GameManager.Instance.GetQuestManager().AccetpQuest.Clear();
        for (int i = 0; i < saveData[(int)slotType].acceptQuest.Count; i+= 2)
        {
            GameManager.Instance.GetQuestManager().Load(saveData[(int)slotType].acceptQuest[i], saveData[(int)slotType].acceptQuest[i + 1]);
            //GameManager.Instance.GetQuestManager().QuestObserver.LoadData(GameManager.Instance.GetQuestManager().QuestDB.GetQuest(saveData[(int)slotType].acceptQuest[i]), saveData[(int)slotType].acceptQuest[i + 1]);
        }   //퀘스트 데이터 로드
    }   //데이터 삽입

    public bool FileCheck(SaveSlot slot)
    {
        return File.Exists(path + slot.ToString());
    }   //세이브 파일 있을시 true 리턴

    public void DeleteFile(SaveSlot slot)
    {
        if(FileCheck(slot))
            File.Delete(path + slot.ToString());
    }   //파일 삭제
}

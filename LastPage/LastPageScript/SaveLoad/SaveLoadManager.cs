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

    public List<int> item = new List<int>();      //������ ���� key, count

    public List<int> acceptQuest = new List<int>(); //����Ʈ ����

    public void Save(PlayerStat stat)
    {
        level = stat.Level;
        money = stat.Money;
        exe = stat.Exe;
    }   //�÷��̾� ������ ����
    public void ResetItemData()
    {
        item.Clear();
    }
    public void SaveItem(int key, int count)
    {
        item.Add(key);
        item.Add(count);
    }   //������ ������ ����
    public void ResetQuedstData()
    {
        acceptQuest.Clear();
    }
    public void SaveQuest(int key, int count)
    {
        acceptQuest.Add(key);
        acceptQuest.Add(count);
    }   //����Ʈ ������ ����
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
    }   //���Ϸ� ����

    public void LoadFile()
    {
        for(int i = 0; i < saveData.Length; ++i)
        {
            //������ ������ ���� ������ ã��
            if (!FileCheck((SaveSlot)i)) continue;
            slotType = (SaveSlot)i;
            string data = File.ReadAllText(path + slotType.ToString());

            if (saveData[i] == null) saveData[i] = new PlayerData();
            saveData[i] = JsonUtility.FromJson<PlayerData>(data);
        }
    }   //���̺� ���� �ε�

    public void InitData()
    {
        if (!FileCheck(slotType)) return;   //�ε��� ������ ������ ����

        GameManager.Instance.GetPlayer().Load(saveData[(int)slotType]); //�÷��̾� ������ �ε�

        GameManager.Instance.GetUIManager().GetPlayerUI.PlayerPanel.GetInventory.ClearSlot();
        for (int i = 0; i < saveData[(int)slotType].item.Count; i += 2)
        {
            GameManager.Instance.GetUIManager().GetPlayerUI.PlayerPanel.GetInventory.InitItem(
            GameManager.Instance.GetItemManager.GetItem(saveData[(int)slotType].item[i]), saveData[(int)slotType].item[i + 1]);
        }   //������ ������ �ε�

        GameManager.Instance.GetUIManager().GetPlayerUI.PlayerPanel.GetAcceptQuest.ClearSlot();
        GameManager.Instance.GetQuestManager().AccetpQuest.Clear();
        for (int i = 0; i < saveData[(int)slotType].acceptQuest.Count; i+= 2)
        {
            GameManager.Instance.GetQuestManager().Load(saveData[(int)slotType].acceptQuest[i], saveData[(int)slotType].acceptQuest[i + 1]);
            //GameManager.Instance.GetQuestManager().QuestObserver.LoadData(GameManager.Instance.GetQuestManager().QuestDB.GetQuest(saveData[(int)slotType].acceptQuest[i]), saveData[(int)slotType].acceptQuest[i + 1]);
        }   //����Ʈ ������ �ε�
    }   //������ ����

    public bool FileCheck(SaveSlot slot)
    {
        return File.Exists(path + slot.ToString());
    }   //���̺� ���� ������ true ����

    public void DeleteFile(SaveSlot slot)
    {
        if(FileCheck(slot))
            File.Delete(path + slot.ToString());
    }   //���� ����
}

using UnityEngine;

public class PlayerPanel : GUI
{
    [SerializeField] InventoryPanel inventory;    //인벤토리 패널
    [SerializeField] AcceptQuestPanel acceptQuest;//퀘스트 패널

    public InventoryPanel GetInventory => inventory;
    public AcceptQuestPanel GetAcceptQuest => acceptQuest;

    public void InventoryPanelActive()
    {
        acceptQuest.SetActive(false);
        inventory.SetActive(true);
    }   //인벤토리 버튼을 눌렀을 때

    public void QuestPanelActive()
    {
        inventory.SetActive(false);
        acceptQuest.SetActive(true);
    }   //퀘스트 버튼을 눌렀을 때
}

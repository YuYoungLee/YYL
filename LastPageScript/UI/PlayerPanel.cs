using UnityEngine;

public class PlayerPanel : GUI
{
    [SerializeField] InventoryPanel inventory;    //�κ��丮 �г�
    [SerializeField] AcceptQuestPanel acceptQuest;//����Ʈ �г�

    public InventoryPanel GetInventory => inventory;
    public AcceptQuestPanel GetAcceptQuest => acceptQuest;

    public void InventoryPanelActive()
    {
        acceptQuest.SetActive(false);
        inventory.SetActive(true);
    }   //�κ��丮 ��ư�� ������ ��

    public void QuestPanelActive()
    {
        inventory.SetActive(false);
        acceptQuest.SetActive(true);
    }   //����Ʈ ��ư�� ������ ��
}

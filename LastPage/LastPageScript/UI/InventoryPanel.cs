using System.Text;
using TMPro;
public class InventoryPanel : GUI
{
    public ItemSlot[] slot;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    private StringBuilder strBuilder = new StringBuilder();

    public void InitItem(Item item, int count = 1)
    {
        for (int i = 0; i < slot.Length; ++i)
        {
            if (slot[i].SlotCheck(item.Key))
            {
                slot[i].AddSlot(item, count);   //���� �������� �κ��丮�� ���� ���
                return;
            }
        }

        for (int i = 0; i < slot.Length; ++i)
        {
            if (slot[i].AddSlot(item, count)) break;
        }
    }   //������ ����
    public void ClearSlot()
    {
        for (int i = 0; i < slot.Length; ++i)
        {
            slot[i].Delete();
        }
    }   //������ ����

    public void SetItemText(Item item)
    {
        strBuilder.Clear();
        strBuilder.Append(item.Name);
        titleText.text = strBuilder.ToString();
        strBuilder.Clear();
        strBuilder.Append(item.Description);
        descriptionText.text = strBuilder.ToString();
    }   //������ ���� �ؽ�Ʈ ����

    public void SetItemTextClear()
    {
        strBuilder.Clear();
        titleText.text = strBuilder.ToString();
        descriptionText.text = strBuilder.ToString();
    }   //������ ���� �ؽ�Ʈ ����

    public void Save()
    {
        GameManager.Instance.SaveLoadManager.GetPlayerData().ResetItemData();    //������ ����Ʈ �ʱ�ȭ

        for (int i = 0; i < slot.Length; ++i)
        {
            if(slot[i].Type != ItemType.Default)    //�������� ���� �� �� ����
            {
                GameManager.Instance.SaveLoadManager.GetPlayerData().SaveItem(slot[i].Key, slot[i].Count);
            }
        }
    }
}

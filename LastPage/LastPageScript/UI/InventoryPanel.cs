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
                slot[i].AddSlot(item, count);   //같은 아이템이 인벤토리에 있을 경우
                return;
            }
        }

        for (int i = 0; i < slot.Length; ++i)
        {
            if (slot[i].AddSlot(item, count)) break;
        }
    }   //아이템 삽입
    public void ClearSlot()
    {
        for (int i = 0; i < slot.Length; ++i)
        {
            slot[i].Delete();
        }
    }   //아이템 제거

    public void SetItemText(Item item)
    {
        strBuilder.Clear();
        strBuilder.Append(item.Name);
        titleText.text = strBuilder.ToString();
        strBuilder.Clear();
        strBuilder.Append(item.Description);
        descriptionText.text = strBuilder.ToString();
    }   //아이템 설명 텍스트 설정

    public void SetItemTextClear()
    {
        strBuilder.Clear();
        titleText.text = strBuilder.ToString();
        descriptionText.text = strBuilder.ToString();
    }   //아이템 설명 텍스트 비우기

    public void Save()
    {
        GameManager.Instance.SaveLoadManager.GetPlayerData().ResetItemData();    //아이템 리스트 초기화

        for (int i = 0; i < slot.Length; ++i)
        {
            if(slot[i].Type != ItemType.Default)    //아이템이 존제 할 때 저장
            {
                GameManager.Instance.SaveLoadManager.GetPlayerData().SaveItem(slot[i].Key, slot[i].Count);
            }
        }
    }
}

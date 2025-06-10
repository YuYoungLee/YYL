using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private ItemType type = ItemType.Default;   //������ Ÿ��
    private int key = 0;           //������ Ű
    private int count = 0;         //������ ����
    private StringBuilder strBuilder = new StringBuilder();

    [SerializeField] Image image;             //������ �̹���
    [SerializeField] TextMeshProUGUI text;    //������ ���� ǥ���� �ؽ�Ʈ
    [SerializeField] GameObject icon;         //������ ǥ���� ������

    public int Key => key;
    public int Count => count;
    public ItemType Type => type;

    public bool SlotCheck(int key) { return this.key == key ? true : false; }
    /// <summary>
    /// item : DB�� ������ ������ ������
    /// </summary>
    public bool AddSlot(Item item, int count)
    {
        if(type == ItemType.Default || key == item.Key)                    //ó�� ��� �� ���
        {
            type = item.Type;
            key = item.Key;
            this.count += count;
            image.sprite = item.Icon;
            SetCountText();
            icon.SetActive(true);

            if (ItemType.Artifact == type)
            {
                GameManager.Instance.GetPlayer().UseItem(item, count);      //�÷��̾� ���� �߰�
            }
            return true;
        }
        else if (key == item.Key) 
        {
            
            this.count += count;  SetCountText(); 
            return true; 
        }     //���� �������� ���
        return false;
    }   //���� ������ �߰�

    public void Delete()
    {
        type = ItemType.Default;
        key = 0;
        count = 0;
        icon.SetActive(false);
        strBuilder.Clear();
        text.text = strBuilder.ToString();
        GameManager.Instance.GetUIManager().GetPlayerUI.GetPlayerPanel.GetInventory.SetItemTextClear();
    }   //���� ������ ����

    private void SetCountText()
    {
        strBuilder.Clear();
        if (count != 0) { strBuilder.Append(count); }   //0�� �ƴҶ��� �ؽ�Ʈ ǥ��
        text.text = strBuilder.ToString();
    }   //ī��Ʈ �ؽ�Ʈ ����

    public void OnPointerEnter(PointerEventData eventData)
    {
        //���Կ� ������ ������ && ������ Key�� DB�� ���� �� ��
        if (type != ItemType.Default && GameManager.Instance.GetItemManager.CheckItem(key))
        {
            GameManager.Instance.GetUIManager().GetPlayerUI.PlayerPanel.GetInventory.SetItemText(GameManager.Instance.GetItemManager.GetItem(key));
        }
    }   //���콺 �����Ͱ� �ش� ���Կ� ���� ��

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.GetUIManager().GetPlayerUI.PlayerPanel.GetInventory.SetItemTextClear();
    }   //���콺 �����Ͱ� �ش� ������ ���� �� ��

    public void OnPointerDown(PointerEventData eventData)
    {
        //�Һ��� ������ Ÿ�� �� ��
        if (type == ItemType.Consumption && count > 0)
        {
            --count;
            //Ű üũ
            if (GameManager.Instance.GetItemManager.CheckItem(key))
            {
                GameManager.Instance.GetPlayer().UseItem(GameManager.Instance.GetItemManager.GetItem(key), 1);      //�÷��̾� ���� �߰�
            }

            if (count == 0) Delete();
            SetCountText();
        }
    }   //�Һ��� �������� Ŭ�� ��
}

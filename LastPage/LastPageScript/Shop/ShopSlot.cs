using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private int key = 9999;
    private StringBuilder strBuilder = new StringBuilder();
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI priceText;

    /// <summary>
    /// key : ������ Ű
    /// </summary>
    public void SetData(int key)
    {
        //������ Ű�� ���� ���
        if(GameManager.Instance.GetItemManager.CheckItem(key))
        {
            Item item = GameManager.Instance.GetItemManager.GetItem(key);
            this.key = item.Key;
            image.sprite = item.Icon;
            strBuilder.Clear();
            strBuilder.Append(item.Price.ToString());
            strBuilder.Append(" ��");
            priceText.text = strBuilder.ToString();
        }
    }   //UI ������ ����

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.GetUIManager().GetPlayerUI.GetShopPanel.SetItemText(key);
    }   //���콺 �����Ͱ� �ش� ���Կ� ���� ��

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.GetUIManager().GetPlayerUI.GetShopPanel.ClearItemText();  //�ؽ�Ʈ ����
    }   //���콺 �����Ͱ� �ش� ������ ���� �� ��

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.GetUIManager().GetPlayerUI.GetShopPanel.ActiveBuyToolBox(key);
    }
}

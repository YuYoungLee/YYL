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
    /// key : 아이템 키
    /// </summary>
    public void SetData(int key)
    {
        //아이템 키가 있을 경우
        if(GameManager.Instance.GetItemManager.CheckItem(key))
        {
            Item item = GameManager.Instance.GetItemManager.GetItem(key);
            this.key = item.Key;
            image.sprite = item.Icon;
            strBuilder.Clear();
            strBuilder.Append(item.Price.ToString());
            strBuilder.Append(" 원");
            priceText.text = strBuilder.ToString();
        }
    }   //UI 데이터 삽입

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.GetUIManager().GetPlayerUI.GetShopPanel.SetItemText(key);
    }   //마우스 포인터가 해당 슬롯에 있을 때

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.GetUIManager().GetPlayerUI.GetShopPanel.ClearItemText();  //텍스트 비우기
    }   //마우스 포인터가 해당 슬롯을 벗어 날 때

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.GetUIManager().GetPlayerUI.GetShopPanel.ActiveBuyToolBox(key);
    }
}

using System;
using System.Diagnostics;
using System.Text;
using TMPro;
using UnityEngine;

public class BuyToolBox : GUI
{
    public TextMeshProUGUI descriptionText;
    public TMP_InputField inputField;
    private StringBuilder strBuilder = new StringBuilder();
    
    private int price = 0;      //지불 할 가격
    private int key = 0;        //클릭한 아이템 key

    public void SetActive(int key)
    {
        this.key = key;
        ClearText();
        strBuilder.Clear();
        inputField.text = strBuilder.ToString();
        SetActive(true);
    }

    public void OnBuy()
    {
        //아이템 키가 존제 할 때 && 가격이 맞을 때
        if(GameManager.Instance.GetItemManager.CheckItem(key) && GameManager.Instance.GetPlayer().BuyCheck(price))
        {
            GameManager.Instance.GetPlayer().BuyItem(price);
            GameManager.Instance.GetUIManager().GetPlayerUI.PlayerPanel.GetInventory.
                InitItem(GameManager.Instance.GetItemManager.GetItem(key), Int32.Parse(inputField.text));
            this.gameObject.SetActive(false);
        }
        else
        {
            strBuilder.Clear();
            strBuilder.Append("소지 금액이 부족 합니다.");
            descriptionText.text = strBuilder.ToString();
        }
        strBuilder.Clear();
        inputField.text = strBuilder.ToString();
    }   //구매버튼

    public void OnCancle()
    {
        this.gameObject.SetActive(false);
        strBuilder.Clear();
        inputField.text = strBuilder.ToString();
    }   //취소 버튼

    public void SetMoneyText()
    {
        //문자열 길이가 0 일 때 탈출
        if (inputField.text.Length == 0) { return; }  //올바르지 않은 상태
        strBuilder.Clear();
        strBuilder.Append("구매 가격 : ");
        price = GameManager.Instance.GetItemManager.GetItem(key).Price * Int32.Parse(inputField.text);
        strBuilder.Append(price.ToString());
        strBuilder.Append(" 원");
        descriptionText.text = strBuilder.ToString();
    }   //인풋필드 문자열

    private void ClearText()
    {
        strBuilder.Clear();
        strBuilder.Append("구매 가격 : 0 원");
        descriptionText.text = strBuilder.ToString() ;
    }
}

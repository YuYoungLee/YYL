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
    
    private int price = 0;      //���� �� ����
    private int key = 0;        //Ŭ���� ������ key

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
        //������ Ű�� ���� �� �� && ������ ���� ��
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
            strBuilder.Append("���� �ݾ��� ���� �մϴ�.");
            descriptionText.text = strBuilder.ToString();
        }
        strBuilder.Clear();
        inputField.text = strBuilder.ToString();
    }   //���Ź�ư

    public void OnCancle()
    {
        this.gameObject.SetActive(false);
        strBuilder.Clear();
        inputField.text = strBuilder.ToString();
    }   //��� ��ư

    public void SetMoneyText()
    {
        //���ڿ� ���̰� 0 �� �� Ż��
        if (inputField.text.Length == 0) { return; }  //�ùٸ��� ���� ����
        strBuilder.Clear();
        strBuilder.Append("���� ���� : ");
        price = GameManager.Instance.GetItemManager.GetItem(key).Price * Int32.Parse(inputField.text);
        strBuilder.Append(price.ToString());
        strBuilder.Append(" ��");
        descriptionText.text = strBuilder.ToString();
    }   //��ǲ�ʵ� ���ڿ�

    private void ClearText()
    {
        strBuilder.Clear();
        strBuilder.Append("���� ���� : 0 ��");
        descriptionText.text = strBuilder.ToString() ;
    }
}

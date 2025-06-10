using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class ShopPanel : GUI
{
    private StringBuilder strBuilder = new StringBuilder();
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] ShopSlot[] slot;
    [SerializeField] BuyToolBox buyToolBox;

    public void ActiveBuyToolBox(int key) { buyToolBox.SetActive(key); }
    public void SetSlot(int key)
    {
        //���� Ű�� ���� �� ��
        if(GameManager.Instance.GetItemManager.CheckShop(key))
        {
            List<int> itemList = GameManager.Instance.GetItemManager.GetItemList(key);

            for(int i = 0; i < itemList.Count; ++i)
            {
                slot[i].SetData(itemList[i]);
            }
            //���� �г� Ȱ��ȭ
            SetActive(true);
        }
    }

    public void SetItemText(int key)
    {
        //������ Ű�� ���� ���
        if (GameManager.Instance.GetItemManager.CheckItem(key))
        {
            strBuilder.Clear();
            strBuilder.Append(GameManager.Instance.GetItemManager.GetItem(key).Name);
            titleText.text = strBuilder.ToString();
            strBuilder.Append(GameManager.Instance.GetItemManager.GetItem(key).Description);
            descriptionText.text = strBuilder.ToString();
        }
    }

    public void ClearItemText()
    {
        strBuilder.Clear();
        titleText.text = strBuilder.ToString();
        descriptionText.text = strBuilder.ToString();
    }

    public void ButtonClickCancle()
    {
        GameManager.Instance.GetSoundManager.EffectPlayOneShot(SFXEffectType.ClickButton);
        GameManager.Instance.SetInputEnable();
        this.gameObject.SetActive(false); 
    }
}

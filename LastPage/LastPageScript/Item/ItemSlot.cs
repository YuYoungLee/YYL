using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private ItemType type = ItemType.Default;   //아이템 타입
    private int key = 0;           //아이템 키
    private int count = 0;         //아이템 갯수
    private StringBuilder strBuilder = new StringBuilder();

    [SerializeField] Image image;             //아이템 이미지
    [SerializeField] TextMeshProUGUI text;    //아이템 갯수 표시할 텍스트
    [SerializeField] GameObject icon;         //아이템 표시할 아이콘

    public int Key => key;
    public int Count => count;
    public ItemType Type => type;

    public bool SlotCheck(int key) { return this.key == key ? true : false; }
    /// <summary>
    /// item : DB로 생성한 아이템 데이터
    /// </summary>
    public bool AddSlot(Item item, int count)
    {
        if(type == ItemType.Default || key == item.Key)                    //처음 들어 올 경우
        {
            type = item.Type;
            key = item.Key;
            this.count += count;
            image.sprite = item.Icon;
            SetCountText();
            icon.SetActive(true);

            if (ItemType.Artifact == type)
            {
                GameManager.Instance.GetPlayer().UseItem(item, count);      //플레이어 스텟 추가
            }
            return true;
        }
        else if (key == item.Key) 
        {
            
            this.count += count;  SetCountText(); 
            return true; 
        }     //같은 아이템일 경우
        return false;
    }   //슬롯 데이터 추가

    public void Delete()
    {
        type = ItemType.Default;
        key = 0;
        count = 0;
        icon.SetActive(false);
        strBuilder.Clear();
        text.text = strBuilder.ToString();
        GameManager.Instance.GetUIManager().GetPlayerUI.GetPlayerPanel.GetInventory.SetItemTextClear();
    }   //슬롯 데이터 삭제

    private void SetCountText()
    {
        strBuilder.Clear();
        if (count != 0) { strBuilder.Append(count); }   //0이 아닐때만 텍스트 표시
        text.text = strBuilder.ToString();
    }   //카운트 텍스트 변경

    public void OnPointerEnter(PointerEventData eventData)
    {
        //슬롯에 아이템 있을때 && 아이템 Key가 DB에 존제 할 때
        if (type != ItemType.Default && GameManager.Instance.GetItemManager.CheckItem(key))
        {
            GameManager.Instance.GetUIManager().GetPlayerUI.PlayerPanel.GetInventory.SetItemText(GameManager.Instance.GetItemManager.GetItem(key));
        }
    }   //마우스 포인터가 해당 슬롯에 있을 때

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.GetUIManager().GetPlayerUI.PlayerPanel.GetInventory.SetItemTextClear();
    }   //마우스 포인터가 해당 슬롯을 벗어 날 때

    public void OnPointerDown(PointerEventData eventData)
    {
        //소비형 아이템 타입 일 때
        if (type == ItemType.Consumption && count > 0)
        {
            --count;
            //키 체크
            if (GameManager.Instance.GetItemManager.CheckItem(key))
            {
                GameManager.Instance.GetPlayer().UseItem(GameManager.Instance.GetItemManager.GetItem(key), 1);      //플레이어 스텟 추가
            }

            if (count == 0) Delete();
            SetCountText();
        }
    }   //소비형 아이템을 클릭 시
}

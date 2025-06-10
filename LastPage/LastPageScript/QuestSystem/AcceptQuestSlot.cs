using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AcceptQuestSlot : MonoBehaviour, IPointerDownHandler
{
    private int questKey = 9999;
    private StringBuilder strBuilder = new StringBuilder();
    [SerializeField] TextMeshProUGUI titleText;

    public int QuestKey => questKey;
    public void SetSlot(int key)
    {
        questKey = key;
        ClearSlot();
        //퀘스트 키가 존제 할 때 텍스트 슬롯 변경
        if (GameManager.Instance.GetQuestManager().QuestDB.KeyCheck(key))
        {
            //올바른 값일 때
            strBuilder.Clear();
            strBuilder.Append(GameManager.Instance.GetQuestManager().QuestDB.GetQuest(key).QuestTitle);
            titleText.text = strBuilder.ToString();
        }
    }   //퀘스트 타이틀 텍스트 변경

    public void ClearSlot()
    {
        strBuilder.Clear();
        titleText.text = strBuilder.ToString();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //퀘스트 설명 텍스트 변경
        GameManager.Instance.GetUIManager().GetPlayerUI.GetPlayerPanel.GetAcceptQuest.SetText(questKey);
    }   //퀘스트 슬롯을 클릭 시
}

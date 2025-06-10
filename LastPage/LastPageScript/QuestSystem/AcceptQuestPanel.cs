using System.Text;
using TMPro;
using UnityEngine;

public class AcceptQuestPanel : GUI
{
    [SerializeField] TextMeshProUGUI explaintionText;
    [SerializeField] TextMeshProUGUI targetText;
    [SerializeField] StringBuilder strBuilder = new StringBuilder();
    [SerializeField] AcceptQuestSlot[] slot;
    private int key = 9999;

    public void AddQuestSlot(int questKey)
    {
        for(int i = 0; i < slot.Length; ++i)
        {
            if (slot[i].QuestKey == questKey) return;
        }

        for (int i = 0; i < slot.Length; ++i)
        {
            if (slot[i].QuestKey == 9999)
            {
                slot[i].SetSlot(questKey);
                return;
            }
        }
    }    //퀘스트 슬롯 퀘스트 추가
    public void SlotRerocation(int questKey)
    {
        int slotIdx = 0;    //슬롯의 인덱스
        for (int i = 0; i < slot.Length; ++i)
        {
            //퀘스트 키가 같을 때 슬롯의 인덱스 저장
            if (slot[i].QuestKey == questKey)
            {
                slotIdx = i;
            }
        }

        //슬롯의 다음 위치 부터 시작
        //앞 슬롯으로 변경
        for(int i = slotIdx + 1; i < slot.Length; ++i)
        {
            slot[i-1].SetSlot(slot[i].QuestKey);
        }

        //마지막 퀘스트 슬롯 비우기
        key = 9999;
        slot[slot.Length - 1].SetSlot(key);
        SetText(key);
    }   //클리어 한 슬롯 재설정
    public void UpdateCurrentQuestText(int questKey)
    {
        SetText(key);
    }
    public void ClearSlot()
    {
        key = 9999;
        SetText(key);
        for (int i = 0; i < slot.Length; ++i)
        {
            slot[i].SetSlot(key);
        }
    }   //퀘스트 슬롯 데이터 제거
    /// <summary>
    /// key : 퀘스트 키
    /// </summary>
    public void SetText(int key)
    {
        //키가 존제 할 경우 실행
        if(GameManager.Instance.GetQuestManager().QuestDB.KeyCheck(key))
        {
            this.key = key;
            strBuilder.Clear();
            strBuilder.Append(GameManager.Instance.GetQuestManager().QuestDB.GetQuest(key).QuestExplainText);   //QuestDB에서 참조
            explaintionText.text = strBuilder.ToString();   //텍스트 변경

            strBuilder.Clear();
            strBuilder.Append("목표 : ");
            strBuilder.Append(GameManager.Instance.GetQuestManager().AccetpQuest[key].QuestCount.ToString());
            strBuilder.Append(" / ");
            strBuilder.Append(GameManager.Instance.GetQuestManager().AccetpQuest[key].ClearCount.ToString());
            targetText.text = strBuilder.ToString();
        }
        else
        {
            strBuilder.Clear();
            explaintionText.text = strBuilder.ToString();
            targetText.text = strBuilder.ToString();
        }
    }   //퀘스트 설명 텍스트 변경
}

using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.UI;

public class QuestPanel : GUI
{
    private int key = 9999;
    private StringBuilder strBuilder = new StringBuilder();
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI explainText;
    [SerializeField] Button acceptBtn;    //수락 버튼
    [SerializeField] Button cancleBtn;    //취소 버튼
    [SerializeField] Button clearBtn;     //완료 버튼

    /// <summary>
    /// key : 퀘스트의 ID값
    /// </summary>
    public bool KeyCheck(int key)
    {
        return this.key == key ? true : false;
    }   //현제 패널의 키와 같은지

    /// <summary>
    /// data : 퀘스트 데이터
    /// </summary>
    public void ChangeText(int key)
    {
        SetButtonStatus(key);       //버튼 활성화 변경
        if (this.key == key) return;

        //텍스트 변경
        QuestData data = GameManager.Instance.GetQuestManager().QuestDB.GetQuest(key);
        this.key = data.QuestID; //퀘스트패널 키값 변경
        strBuilder.Clear();
        strBuilder.Append(data.QuestTitle);
        titleText.text = strBuilder.ToString();
        strBuilder.Clear();
        strBuilder.Append(data.QuestExplainText);
        explainText.text = strBuilder.ToString();
    }   //퀘스트 텍스트 변경

    /// <summary>
    /// acceptStatus : 퀘스트 활성화 상태
    /// </summary>
    public void SetButtonStatus(int key)
    {
        //퀘스트 받은 상태일때
        bool acceptStatus = GameManager.Instance.GetQuestManager().AccetpQuest.ContainsKey(key);
        acceptBtn.gameObject.SetActive(!acceptStatus);       //수락버튼
        clearBtn.gameObject.SetActive(acceptStatus);         //완료버튼
    }   //퀘스트 상황에 따른 버튼의 활성화 변경

    public void AcceptClick()
    {
        GameManager.Instance.GetPlayer().AddQuest(key);     //플레이어의 퀘스트 구독
        GameManager.Instance.SetInputEnable();
        SetActive(false);
        return;
    }   //수락버튼을 눌렀을 때 처리

    public void CancleClick()
    {
        GameManager.Instance.SetInputEnable();
        SetActive(false);
    }   //취소 버튼을 눌렀을 때 처리

    public void ClearClick()
    {
        //해당 퀘스트가 클리어 조건을 만족 할 때
        if (GameManager.Instance.GetQuestManager().AccetpQuest[key].ClearQuestCheck())
        {
            GameManager.Instance.GetPlayer().RemoveQuest(key);      //플레이어 퀘스트 구독 취소
            GameManager.Instance.GetQuestManager().QuestObject.NextQuest();     //퀘스트 NPC다음 퀘스트로 변경
            GameManager.Instance.GetUIManager().GetPlayerUI.GetPlayerPanel.GetAcceptQuest.SlotRerocation(key);      //퀘스트 UI패널 리셋
        }
        GameManager.Instance.SetInputEnable();
        SetActive(false);
    }
}

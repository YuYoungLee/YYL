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
        //����Ʈ Ű�� ���� �� �� �ؽ�Ʈ ���� ����
        if (GameManager.Instance.GetQuestManager().QuestDB.KeyCheck(key))
        {
            //�ùٸ� ���� ��
            strBuilder.Clear();
            strBuilder.Append(GameManager.Instance.GetQuestManager().QuestDB.GetQuest(key).QuestTitle);
            titleText.text = strBuilder.ToString();
        }
    }   //����Ʈ Ÿ��Ʋ �ؽ�Ʈ ����

    public void ClearSlot()
    {
        strBuilder.Clear();
        titleText.text = strBuilder.ToString();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //����Ʈ ���� �ؽ�Ʈ ����
        GameManager.Instance.GetUIManager().GetPlayerUI.GetPlayerPanel.GetAcceptQuest.SetText(questKey);
    }   //����Ʈ ������ Ŭ�� ��
}

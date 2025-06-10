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
    }    //����Ʈ ���� ����Ʈ �߰�
    public void SlotRerocation(int questKey)
    {
        int slotIdx = 0;    //������ �ε���
        for (int i = 0; i < slot.Length; ++i)
        {
            //����Ʈ Ű�� ���� �� ������ �ε��� ����
            if (slot[i].QuestKey == questKey)
            {
                slotIdx = i;
            }
        }

        //������ ���� ��ġ ���� ����
        //�� �������� ����
        for(int i = slotIdx + 1; i < slot.Length; ++i)
        {
            slot[i-1].SetSlot(slot[i].QuestKey);
        }

        //������ ����Ʈ ���� ����
        key = 9999;
        slot[slot.Length - 1].SetSlot(key);
        SetText(key);
    }   //Ŭ���� �� ���� �缳��
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
    }   //����Ʈ ���� ������ ����
    /// <summary>
    /// key : ����Ʈ Ű
    /// </summary>
    public void SetText(int key)
    {
        //Ű�� ���� �� ��� ����
        if(GameManager.Instance.GetQuestManager().QuestDB.KeyCheck(key))
        {
            this.key = key;
            strBuilder.Clear();
            strBuilder.Append(GameManager.Instance.GetQuestManager().QuestDB.GetQuest(key).QuestExplainText);   //QuestDB���� ����
            explaintionText.text = strBuilder.ToString();   //�ؽ�Ʈ ����

            strBuilder.Clear();
            strBuilder.Append("��ǥ : ");
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
    }   //����Ʈ ���� �ؽ�Ʈ ����
}

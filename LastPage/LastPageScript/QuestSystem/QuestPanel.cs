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
    [SerializeField] Button acceptBtn;    //���� ��ư
    [SerializeField] Button cancleBtn;    //��� ��ư
    [SerializeField] Button clearBtn;     //�Ϸ� ��ư

    /// <summary>
    /// key : ����Ʈ�� ID��
    /// </summary>
    public bool KeyCheck(int key)
    {
        return this.key == key ? true : false;
    }   //���� �г��� Ű�� ������

    /// <summary>
    /// data : ����Ʈ ������
    /// </summary>
    public void ChangeText(int key)
    {
        SetButtonStatus(key);       //��ư Ȱ��ȭ ����
        if (this.key == key) return;

        //�ؽ�Ʈ ����
        QuestData data = GameManager.Instance.GetQuestManager().QuestDB.GetQuest(key);
        this.key = data.QuestID; //����Ʈ�г� Ű�� ����
        strBuilder.Clear();
        strBuilder.Append(data.QuestTitle);
        titleText.text = strBuilder.ToString();
        strBuilder.Clear();
        strBuilder.Append(data.QuestExplainText);
        explainText.text = strBuilder.ToString();
    }   //����Ʈ �ؽ�Ʈ ����

    /// <summary>
    /// acceptStatus : ����Ʈ Ȱ��ȭ ����
    /// </summary>
    public void SetButtonStatus(int key)
    {
        //����Ʈ ���� �����϶�
        bool acceptStatus = GameManager.Instance.GetQuestManager().AccetpQuest.ContainsKey(key);
        acceptBtn.gameObject.SetActive(!acceptStatus);       //������ư
        clearBtn.gameObject.SetActive(acceptStatus);         //�Ϸ��ư
    }   //����Ʈ ��Ȳ�� ���� ��ư�� Ȱ��ȭ ����

    public void AcceptClick()
    {
        GameManager.Instance.GetPlayer().AddQuest(key);     //�÷��̾��� ����Ʈ ����
        GameManager.Instance.SetInputEnable();
        SetActive(false);
        return;
    }   //������ư�� ������ �� ó��

    public void CancleClick()
    {
        GameManager.Instance.SetInputEnable();
        SetActive(false);
    }   //��� ��ư�� ������ �� ó��

    public void ClearClick()
    {
        //�ش� ����Ʈ�� Ŭ���� ������ ���� �� ��
        if (GameManager.Instance.GetQuestManager().AccetpQuest[key].ClearQuestCheck())
        {
            GameManager.Instance.GetPlayer().RemoveQuest(key);      //�÷��̾� ����Ʈ ���� ���
            GameManager.Instance.GetQuestManager().QuestObject.NextQuest();     //����Ʈ NPC���� ����Ʈ�� ����
            GameManager.Instance.GetUIManager().GetPlayerUI.GetPlayerPanel.GetAcceptQuest.SlotRerocation(key);      //����Ʈ UI�г� ����
        }
        GameManager.Instance.SetInputEnable();
        SetActive(false);
    }
}

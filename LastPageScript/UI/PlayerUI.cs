using System.Text;
using TMPro;
using UnityEngine;

public enum SkillKey
{ Skill_M1, Skill_M2, Skill_Q, Skill_E, Skill_R, Skill_Default }

public class PlayerUI : GUI
{
    [Header("ActionBar")]
    [SerializeField] ActionPanel actionPanel;             //�÷��̾� ���� �г�

    [Header("LevelUpPanel")]
    [SerializeField] LevelUpPanel levelUpPanel;             //�÷��̾� ���� �г�

    [Header("Interaction")]
    [SerializeField] GameObject ActionObject;             //�ൿ �ؽ�Ʈ ������Ʈ
    [SerializeField] TextMeshProUGUI ActionText;          //�÷��̾��� �ൿ �ؽ�Ʈ

    [Header("Inventory")]
    [SerializeField] PlayerPanel playerPanel;             //�κ��丮, ����Ʈ �г�

    [Header("Quest")]
    [SerializeField] QuestPanel questPanel;               //����Ʈ â �г�

    [Header("Shop")]
    [SerializeField] ShopPanel shopPanel;                 //���� â �г�

    [Header("MoneyText")]
    [SerializeField] TextMeshProUGUI moneyText;

    [Header("GameOption")]
    [SerializeField] GameOptionPanel gameOptionObj;

    [Header("GameOver")]
    [SerializeField] GameObject gameOverObj;
    private StringBuilder strBuilder = new StringBuilder();
    public ActionPanel GetActionPanel => actionPanel;
    public LevelUpPanel GetLevelUpPanel => levelUpPanel;
    public PlayerPanel GetPlayerPanel => playerPanel;
    public QuestPanel GetQuestPanel => questPanel;
    public ShopPanel GetShopPanel => shopPanel;
    public GameOptionPanel GetgameOptionPanel => gameOptionObj;
    public GameObject GetGameOverObj => gameOverObj;

    #region Inventory
    public PlayerPanel PlayerPanel => playerPanel;
    /// <summary>
    /// textStatus : �ؽ�Ʈ ������Ʈ Ȱ��ȭ, ��Ȱ��ȭ ����
    /// </summary>
    public void ActiveItemText(bool textStatus)
    {
        if(textStatus)
        {
            strBuilder.Clear();
            strBuilder.Append("FŰ�� ���� ��ȣ�ۿ�");
            ActionText.text = strBuilder.ToString();
        }
        ActionObject.SetActive(textStatus);
    }   //��ȣ�ۿ� �ؽ�Ʈ Ȱ��ȭ

    /// <summary>
    /// artifact : ��Ƽ��Ʈ ����
    /// sprite : ��Ƽ��Ʈ�� ��������Ʈ
    /// </summary>
    #endregion

    public void SetMoneyText(int money)
    {
        strBuilder.Clear();
        strBuilder.Append("�����ݾ� : ");
        strBuilder.Append(money.ToString());
        moneyText.text = strBuilder.ToString();
    }   //�����ݾ� �ؽ�Ʈ ����

    public void OptionButton()
    {
        gameOptionObj.SetActive(false);
        GameManager.Instance.GetUIManager().GetOptionPanel.SetActive(true);
    }

    public void RetryButton()
    {
        gameOverObj.SetActive(false);
        GameManager.Instance.ChangeScene(SceneLoad.RETRY);      //�����
    }

    public void ReturnButton()
    {
        gameOverObj.SetActive(false);
        //GameManager.Instance.SaveLoadManager.DeleteFile();
        GameManager.Instance.ChangeScene(SceneLoad.MAIN);       //���θŴ� ���ư���
    }
}
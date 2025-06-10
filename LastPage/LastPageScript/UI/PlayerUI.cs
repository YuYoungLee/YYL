using System.Text;
using TMPro;
using UnityEngine;

public enum SkillKey
{ Skill_M1, Skill_M2, Skill_Q, Skill_E, Skill_R, Skill_Default }

public class PlayerUI : GUI
{
    [Header("ActionBar")]
    [SerializeField] ActionPanel actionPanel;             //플레이어 엑션 패널

    [Header("LevelUpPanel")]
    [SerializeField] LevelUpPanel levelUpPanel;             //플레이어 엑션 패널

    [Header("Interaction")]
    [SerializeField] GameObject ActionObject;             //행동 텍스트 오브젝트
    [SerializeField] TextMeshProUGUI ActionText;          //플레이어의 행동 텍스트

    [Header("Inventory")]
    [SerializeField] PlayerPanel playerPanel;             //인벤토리, 퀘스트 패널

    [Header("Quest")]
    [SerializeField] QuestPanel questPanel;               //퀘스트 창 패널

    [Header("Shop")]
    [SerializeField] ShopPanel shopPanel;                 //상점 창 패널

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
    /// textStatus : 텍스트 오브젝트 활성화, 비활성화 여부
    /// </summary>
    public void ActiveItemText(bool textStatus)
    {
        if(textStatus)
        {
            strBuilder.Clear();
            strBuilder.Append("F키를 눌러 상호작용");
            ActionText.text = strBuilder.ToString();
        }
        ActionObject.SetActive(textStatus);
    }   //상호작용 텍스트 활성화

    /// <summary>
    /// artifact : 아티펙트 종류
    /// sprite : 아티펙트의 스프라이트
    /// </summary>
    #endregion

    public void SetMoneyText(int money)
    {
        strBuilder.Clear();
        strBuilder.Append("소지금액 : ");
        strBuilder.Append(money.ToString());
        moneyText.text = strBuilder.ToString();
    }   //소지금액 텍스트 설정

    public void OptionButton()
    {
        gameOptionObj.SetActive(false);
        GameManager.Instance.GetUIManager().GetOptionPanel.SetActive(true);
    }

    public void RetryButton()
    {
        gameOverObj.SetActive(false);
        GameManager.Instance.ChangeScene(SceneLoad.RETRY);      //재시작
    }

    public void ReturnButton()
    {
        gameOverObj.SetActive(false);
        //GameManager.Instance.SaveLoadManager.DeleteFile();
        GameManager.Instance.ChangeScene(SceneLoad.MAIN);       //메인매뉴 돌아가기
    }
}
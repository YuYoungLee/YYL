using System.Text;
using TMPro;
using UnityEngine;

public class SaveLoadPanel : GUI
{
    [SerializeField] TextMeshProUGUI[] saveText;
    [SerializeField] TextMeshProUGUI[] levelText;
    private StringBuilder strBuilder = new StringBuilder();

    public void Initialize()
    {
        GameManager.Instance.SaveLoadManager.LoadFile();
        SetData();
    }

    public void SetData()
    {
        for (int i = 0; i < saveText.Length; ++i)
        {

            if (GameManager.Instance.SaveLoadManager.FileCheck((SaveSlot)i))
            {
                GameManager.Instance.SaveLoadManager.SetType = (SaveSlot)i;     //���� ����
                strBuilder.Clear();
                strBuilder.Append("�ҷ�����");
                saveText[i].text = strBuilder.ToString();
                strBuilder.Clear();
                strBuilder.Append("Lv.");
                strBuilder.Append(GameManager.Instance.SaveLoadManager.GetPlayerData().level.ToString());
                levelText[i].text = strBuilder.ToString();
            }
            else
            {
                strBuilder.Clear();
                strBuilder.Append("������");
                saveText[i].text = strBuilder.ToString();
                strBuilder.Clear();
                levelText[i].text = strBuilder.ToString();
            }
        }
        GameManager.Instance.GetSoundManager.EffectPlayOneShot(SFXEffectType.MenuSelect);
    }

    public void StartGame(int i)
    {
        GameManager.Instance.SaveLoadManager.SetType = (SaveSlot)i;     //���� �ε� �� ���� ����
        GameManager.Instance.StartGame();
    }

    public void DeleteSave(int i)
    {
        GameManager.Instance.SaveLoadManager.DeleteFile((SaveSlot)i);
        SetData();
    }
}

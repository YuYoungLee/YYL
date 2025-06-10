using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class SkillSlot : MonoBehaviour
{
    public Image skillImage;                     //�÷��̾��� ��ų �̹���
    public TextMeshProUGUI cooltimeText;         //��Ÿ�� �ؽ�Ʈ
    public Image fillImage;                      //��Ÿ�� ȿ�� �̹���
    private StringBuilder strBuilder = new StringBuilder();

    public Sprite SetImage { set { skillImage.sprite = value; } }

    public void SetCooltime(int cooltime, float fillAmount)
    {
        fillImage.fillAmount = fillAmount;
        strBuilder.Clear();
        strBuilder.Append(cooltime);
        cooltimeText.text = strBuilder.ToString();
    }

    public void ResetTime()
    {
        strBuilder.Clear();
        cooltimeText.text = strBuilder.ToString();
        fillImage.fillAmount = 0;
    }
}

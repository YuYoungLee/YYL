using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class SkillSlot : MonoBehaviour
{
    public Image skillImage;                     //플레이어의 스킬 이미지
    public TextMeshProUGUI cooltimeText;         //쿨타임 텍스트
    public Image fillImage;                      //쿨타임 효과 이미지
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

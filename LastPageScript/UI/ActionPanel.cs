using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionPanel : MonoBehaviour
{
    [Header("PlayerUI")]
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI HpText;              //ü�� - �ִ�ü��/����ü��
    [SerializeField] Slider hpSlider;                     //ü�� �����̴� ��
    [SerializeField] TextMeshProUGUI exeText;             //����ġ - �ʿ� ����ġ/���� ����ġ
    [SerializeField] Slider exeSlider;                    //����ġ �����̴� ��
    [Header("Skill")]
    [SerializeField] SkillSlot[] skillSlots;              //��ų ����

    private StringBuilder strBuilder = new StringBuilder();

    #region PlayerUI
    /// <summary>
    /// hp : ���� ü��
    /// maxHp : �ִ� ü��
    public void SetHp(int hp, int maxHp)
    {
        hpSlider.value = hp;
        //�ؽ�Ʈ
        strBuilder.Clear();
        strBuilder.Append(hp);
        strBuilder.Append(" / ");
        strBuilder.Append(maxHp);
        strBuilder.Append(" HP");
        HpText.text = strBuilder.ToString();
    }   //UI ü�� ǥ��

    /// <summary>
    /// exe : ���� ����ġ
    /// maxExe : �ִ� ����ġ
    /// </summary>
    public void SetExe(int exe, int maxExe)
    {
        exeSlider.value = exe;
        //�ؽ�Ʈ
        strBuilder.Clear();
        strBuilder.Append(exe);
        strBuilder.Append(" / ");
        strBuilder.Append(maxExe);
        strBuilder.Append(" EXE");
        exeText.text = strBuilder.ToString();
    }   //UI ����ġ ǥ��

    public void SetLevel(int level)
    {
        strBuilder.Clear();
        strBuilder.Append("Lv.");
        strBuilder.Append(level.ToString());
        levelText.text = strBuilder.ToString();
    }   //���� ǥ��

    public void SetPlayerUIStat(PlayerStat stat)
    {
        SetLevel(stat.Level);
        hpSlider.maxValue = stat.HpMax;
        hpSlider.value = stat.Hp;
        exeSlider.maxValue = stat.ExeMax;
        exeSlider.value = stat.Exe;
        SetHp(stat.Hp, stat.HpMax);
        SetExe(stat.Exe, stat.ExeMax);
    }
    #endregion

    #region Skill

    /// <summary>
    /// key : ���� ��ų ��ǲ Ű
    /// sprite : ������ �̹���
    /// </summary>
    public void SetSkillImage(SkillKey key, Sprite sprite)
    {
        skillSlots[(int)key].SetImage = sprite;
    }

    /// <summary>
    /// key : ���� ��ų ��ǲ Ű
    /// cooltime : ���� ��Ÿ�� �ð�
    /// size : ���� ��Ÿ�� / ��Ÿ�� ��
    /// </summary>
    public void SkillCoolTimeUI(SkillKey key, int cooltime, float fillAmount)
    {
        skillSlots[(int)key].SetCooltime(cooltime, fillAmount);
    }   //��ų ��ǲ UI ����� ������ �� true ����

    public void ResetTime(SkillKey key)
    {
        skillSlots[(int)key].ResetTime();
    }
    #endregion
}

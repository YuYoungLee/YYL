using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionPanel : MonoBehaviour
{
    [Header("PlayerUI")]
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI HpText;              //체력 - 최대체력/현제체력
    [SerializeField] Slider hpSlider;                     //체력 슬라이더 바
    [SerializeField] TextMeshProUGUI exeText;             //경험치 - 필요 경험치/현제 경험치
    [SerializeField] Slider exeSlider;                    //경험치 슬라이더 바
    [Header("Skill")]
    [SerializeField] SkillSlot[] skillSlots;              //스킬 슬롯

    private StringBuilder strBuilder = new StringBuilder();

    #region PlayerUI
    /// <summary>
    /// hp : 현재 체력
    /// maxHp : 최대 체력
    public void SetHp(int hp, int maxHp)
    {
        hpSlider.value = hp;
        //텍스트
        strBuilder.Clear();
        strBuilder.Append(hp);
        strBuilder.Append(" / ");
        strBuilder.Append(maxHp);
        strBuilder.Append(" HP");
        HpText.text = strBuilder.ToString();
    }   //UI 체력 표시

    /// <summary>
    /// exe : 현재 경험치
    /// maxExe : 최대 경험치
    /// </summary>
    public void SetExe(int exe, int maxExe)
    {
        exeSlider.value = exe;
        //텍스트
        strBuilder.Clear();
        strBuilder.Append(exe);
        strBuilder.Append(" / ");
        strBuilder.Append(maxExe);
        strBuilder.Append(" EXE");
        exeText.text = strBuilder.ToString();
    }   //UI 경험치 표시

    public void SetLevel(int level)
    {
        strBuilder.Clear();
        strBuilder.Append("Lv.");
        strBuilder.Append(level.ToString());
        levelText.text = strBuilder.ToString();
    }   //레벨 표시

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
    /// key : 누른 스킬 인풋 키
    /// sprite : 변경할 이미지
    /// </summary>
    public void SetSkillImage(SkillKey key, Sprite sprite)
    {
        skillSlots[(int)key].SetImage = sprite;
    }

    /// <summary>
    /// key : 누른 스킬 인풋 키
    /// cooltime : 남은 쿨타임 시간
    /// size : 현제 쿨타임 / 쿨타임 값
    /// </summary>
    public void SkillCoolTimeUI(SkillKey key, int cooltime, float fillAmount)
    {
        skillSlots[(int)key].SetCooltime(cooltime, fillAmount);
    }   //스킬 인풋 UI 출력이 가능할 때 true 리턴

    public void ResetTime(SkillKey key)
    {
        skillSlots[(int)key].ResetTime();
    }
    #endregion
}

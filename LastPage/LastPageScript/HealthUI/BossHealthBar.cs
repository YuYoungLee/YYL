using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : GUI
{
    [SerializeField] TextMeshProUGUI HpText;              //체력 - 최대체력/현제체력
    [SerializeField] Slider hpSlider;
    private StringBuilder strBuilder = new StringBuilder();

    public void SetBossUI(int hp, int maxHp)
    {
        hpSlider.maxValue = maxHp;
        SetHp(hp, maxHp);
        SetActive(true);
    }
    public void SetHp(int hp, int maxHp)
    {
        if (hp == 0) SetActive(false);
        hpSlider.value = hp;
        //텍스트
        strBuilder.Clear();
        strBuilder.Append(hp.ToString());
        strBuilder.Append("/");
        strBuilder.Append(maxHp.ToString());
        strBuilder.Append("HP");
        HpText.text = strBuilder.ToString();
    }   //UI 체력 표시
}

using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : GUI
{
    [SerializeField] TextMeshProUGUI HpText;              //ü�� - �ִ�ü��/����ü��
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
        //�ؽ�Ʈ
        strBuilder.Clear();
        strBuilder.Append(hp.ToString());
        strBuilder.Append("/");
        strBuilder.Append(maxHp.ToString());
        strBuilder.Append("HP");
        HpText.text = strBuilder.ToString();
    }   //UI ü�� ǥ��
}

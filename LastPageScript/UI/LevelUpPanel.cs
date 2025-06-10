using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class LevelUpPanel : GUI
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] GameObject effectObject;
    private StringBuilder strBuilder = new StringBuilder();
    private Coroutine levelUpCoroutine = null;
    private float timeCount = 0.0f;

    public void PlayLevelUp(int level)
    {
        timeCount = 3.0f;
        SetLevelUpText(level);
        if (levelUpCoroutine == null)
        {
            SetActive(true);
            levelUpCoroutine = StartCoroutine(LevelUpCoroutine());
        }
    }

    public void StopLevelUpCoroutine()
    {
        levelUpCoroutine = null;
        StopCoroutine(LevelUpCoroutine());
        SetActive(false);
        effectObject.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void SetLevelUpText(int level)
    {
        strBuilder.Clear();
        strBuilder.Append("���� ��! Lv.");
        strBuilder.Append(level.ToString());
        levelText.text = strBuilder.ToString();
    }

    private IEnumerator LevelUpCoroutine()
    {
        //Ÿ��ī��Ʈ�� 0���� Ŭ �� ����
        while(timeCount > 0)
        {
            timeCount -= 0.1f;  //Ÿ�� ī��Ʈ ����
            effectObject.transform.rotation *= Quaternion.Euler(0.0f, 0.0f, 300.0f * Time.deltaTime);   //����Ʈ ȸ��
            yield return new WaitForSeconds(0.1f);
        }

        StopLevelUpCoroutine();     //�ڷ�ƾ ����
    }
}

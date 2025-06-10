using System.Collections;
using UnityEngine;

public class QuestObject : InteractionObject
{
    [SerializeField] RectTransform textRectTransform;
    private Coroutine lookCoroutine = null;
    private Vector3 target;
    private int key = 1;            //����Ʈ key

    public int Key => key;
    public int SetKey { set { key = value; } }
    public override void InteractionPlayer()
    {
        //��ȣ�ۿ� ������ ���� && ����Ʈ DB�� Ű�� ���� �� ���
        if (InteractionStatus && GameManager.Instance.GetQuestManager().QuestDB.KeyCheck(key))
        {
            GameManager.Instance.SetInputDisable();
            GameManager.Instance.GetUIManager().GetPlayerUI.GetQuestPanel.ChangeText(key);
            GameManager.Instance.GetUIManager().GetPlayerUI.GetQuestPanel.SetActive(true);     //����Ʈ �г� Ȱ��ȭ
        }
    }
    public void NextQuest()
    {
        key = GameManager.Instance.GetQuestManager().QuestDB.GetNextQuestID(key);
    }   //���� ����Ʈ ����
    public void StartCoroutine()
    {
        if (lookCoroutine == null)  { lookCoroutine = StartCoroutine(LookCoroutine()); }
    }

    public void StopCoroutine()
    {
        StopCoroutine(LookCoroutine());
        lookCoroutine = null;
    }

    private IEnumerator LookCoroutine()
    {
        while(true)
        {
            target = textRectTransform.position - GameManager.Instance.GetCameraController.GetPos();
            target.y = 0.0f;
            textRectTransform.rotation = Quaternion.LookRotation(target.normalized);
            yield return new WaitForSeconds(0.2f);
        }
    }
}

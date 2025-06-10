using System.Collections;
using UnityEngine;

public class QuestObject : InteractionObject
{
    [SerializeField] RectTransform textRectTransform;
    private Coroutine lookCoroutine = null;
    private Vector3 target;
    private int key = 1;            //퀘스트 key

    public int Key => key;
    public int SetKey { set { key = value; } }
    public override void InteractionPlayer()
    {
        //상호작용 가능한 상태 && 퀘스트 DB에 키가 존제 할 경우
        if (InteractionStatus && GameManager.Instance.GetQuestManager().QuestDB.KeyCheck(key))
        {
            GameManager.Instance.SetInputDisable();
            GameManager.Instance.GetUIManager().GetPlayerUI.GetQuestPanel.ChangeText(key);
            GameManager.Instance.GetUIManager().GetPlayerUI.GetQuestPanel.SetActive(true);     //퀘스트 패널 활성화
        }
    }
    public void NextQuest()
    {
        key = GameManager.Instance.GetQuestManager().QuestDB.GetNextQuestID(key);
    }   //다음 퀘스트 변경
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

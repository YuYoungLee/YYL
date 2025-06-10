using System.Collections;
using UnityEngine;

public class ShopObject : InteractionObject
{
    [SerializeField] RectTransform textTransform;
    private Coroutine lookCoroutine = null;
    private Vector3 target;
    private int key = 1;

    public override void InteractionPlayer()
    {
        //���� Ű�� ���� �� ��
        if(interactionStatus && GameManager.Instance.GetItemManager.CheckShop(key))
        {
            //������ ǥ��
            GameManager.Instance.GetUIManager().GetPlayerUI.GetShopPanel.SetSlot(key);
            GameManager.Instance.SetInputDisable();
        }
    }

    public void StartCoroutine()
    {
        if (lookCoroutine == null) { lookCoroutine = StartCoroutine(LookCoroutine()); }
    }

    public void StopCoroutine()
    {
        StopCoroutine(LookCoroutine());
        lookCoroutine = null;
    }

    IEnumerator LookCoroutine()
    {
        while (true)
        {
            target = textTransform.position - GameManager.Instance.GetCameraController.GetPos();
            target.y = 0.0f;
            textTransform.rotation = Quaternion.LookRotation(target.normalized);
            yield return new WaitForSeconds(0.2f);
        }
    }
}

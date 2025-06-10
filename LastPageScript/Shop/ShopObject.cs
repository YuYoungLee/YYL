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
        //상인 키가 존제 할 때
        if(interactionStatus && GameManager.Instance.GetItemManager.CheckShop(key))
        {
            //아이템 표시
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

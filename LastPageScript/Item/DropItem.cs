using System.Collections;
using UnityEngine;

public class DropItem : InteractionObject
{
    [SerializeField] Transform trans;
    [SerializeField] ParticleSystem itemEffect;
    [SerializeField] SpriteRenderer sprite;
    private Vector3 target;
    protected Coroutine effectCoroutine = null;
    protected int key;                          //������ Ű

    public event System.Action returnPool;      //Ǯ ���� �̺�Ʈ
    public int Key => key;   //������ Ű

    public override void InteractionPlayer()
    {
        itemEffect.Stop();
        interactionStatus = false;
        if (effectCoroutine != null)
        {
            StopCoroutine(EffectCoroutine());
            effectCoroutine = null;
        }

        Item item = GameManager.Instance.GetItemManager.GetItem(key);
        GameManager.Instance.GetUIManager().GetPlayerUI.PlayerPanel.GetInventory.InitItem(item);   //�κ��丮 UI ������ �߰�
        returnPool?.Invoke();
        SetActive(false);
    }

    public void SetItem(Item item, Vector3 pos)
    {
        interactionStatus = true;                   //��ȣ�ۿ� ������ ���� ����
        trans.position = pos;

        key = item.Key;
        sprite.sprite = item.Icon;
        this.SetActive(true);
        
        //�÷��̾ �ٶ󺸴� �ڷ�ƾ
        if (effectCoroutine == null) { effectCoroutine = StartCoroutine(EffectCoroutine()); }
    }   //������ ��ӽ� ����

    private void LookPlayer()
    {
        target =  trans.position - GameManager.Instance.GetCameraController.GetPos();
        target.y = 0.0f;
        trans.rotation = Quaternion.LookRotation(target.normalized);
    }

    private IEnumerator EffectCoroutine()
    {
        itemEffect.Play();
        Vector3 move = Vector3.up * 0.05f;
        while (true)
        {
            for(int i = 0; i < 5; ++i)
            {
                trans.position += move;
                LookPlayer();
                yield return new WaitForSeconds(0.1f);
            }

            for (int i = 0; i < 5; ++i)
            {
                trans.position -= move;
                LookPlayer();
                yield return new WaitForSeconds(0.1f);
            }
        }
    }   //�÷��̾ �ٶ󺸴� �ڷ�ƾ
}

using System.Collections;
using UnityEngine;

public class DropItem : InteractionObject
{
    [SerializeField] Transform trans;
    [SerializeField] ParticleSystem itemEffect;
    [SerializeField] SpriteRenderer sprite;
    private Vector3 target;
    protected Coroutine effectCoroutine = null;
    protected int key;                          //아이템 키

    public event System.Action returnPool;      //풀 리턴 이벤트
    public int Key => key;   //아이템 키

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
        GameManager.Instance.GetUIManager().GetPlayerUI.PlayerPanel.GetInventory.InitItem(item);   //인벤토리 UI 아이템 추가
        returnPool?.Invoke();
        SetActive(false);
    }

    public void SetItem(Item item, Vector3 pos)
    {
        interactionStatus = true;                   //상호작용 가능한 상태 변경
        trans.position = pos;

        key = item.Key;
        sprite.sprite = item.Icon;
        this.SetActive(true);
        
        //플레이어를 바라보는 코루틴
        if (effectCoroutine == null) { effectCoroutine = StartCoroutine(EffectCoroutine()); }
    }   //아이템 드롭시 설정

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
    }   //플래이어를 바라보는 코루틴
}

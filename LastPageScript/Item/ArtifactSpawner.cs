using System.Collections;
using UnityEngine;

public class ArtifactSpawner : InteractionObject
{
    private Animator anim;

    //효과
    private Coroutine particleCoroutine = null;
    [SerializeField] GameObject particleObject;
    [SerializeField] ParticleSystem particle;
    [SerializeField] Collider trigerCollider;

    public void Initialize()
    {
        anim = GetComponent<Animator>();
        particle.Stop();
        particleObject.SetActive(false);
        ResetObject();
    }

    public void ResetObject()
    {
        interactionStatus = true;       //상호작용 상태 활성화
        anim.SetTrigger("close");       //상자 닫기 상태
        trigerCollider.enabled = true;
    }   //아티팩트 스포너 리셋

    public override void InteractionPlayer ()
    {
        //상호 작용 가능 한 상태 일 때
        if (interactionStatus)
        {
            //파티클 효과 재생
            if (particleCoroutine == null) particleCoroutine = StartCoroutine(ParticleCoroutine());
            interactionStatus = false;          //상호작용 상태 비활성화
            trigerCollider.enabled = false;
            anim.SetTrigger("open");
            Vector3 createItemPos = this.gameObject.transform.position + this.gameObject.transform.forward;     //상자 앞방향
            int randomKey = GameManager.Instance.GetItemManager.GetRandomItemKey();   //랜덤 키
            GameManager.Instance.GetItemManager.CreateDropItem(createItemPos, randomKey);  //랜덤 아이템 생성
            //키가 눌렸을 때 아이템 뽑기
        }
    }   //플레이어 상호작용

    private void Stop()
    {
        StopCoroutine(ParticleCoroutine());
        particleCoroutine = null;
    }

    private IEnumerator ParticleCoroutine()
    {
        particleObject.SetActive(true);
        particle.Play();
        yield return new WaitForSeconds(1.0f);
        particle.Stop();
        particleObject.SetActive(false);
        Stop();
    }
}

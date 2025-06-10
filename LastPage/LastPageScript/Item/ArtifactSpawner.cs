using System.Collections;
using UnityEngine;

public class ArtifactSpawner : InteractionObject
{
    private Animator anim;

    //ȿ��
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
        interactionStatus = true;       //��ȣ�ۿ� ���� Ȱ��ȭ
        anim.SetTrigger("close");       //���� �ݱ� ����
        trigerCollider.enabled = true;
    }   //��Ƽ��Ʈ ������ ����

    public override void InteractionPlayer ()
    {
        //��ȣ �ۿ� ���� �� ���� �� ��
        if (interactionStatus)
        {
            //��ƼŬ ȿ�� ���
            if (particleCoroutine == null) particleCoroutine = StartCoroutine(ParticleCoroutine());
            interactionStatus = false;          //��ȣ�ۿ� ���� ��Ȱ��ȭ
            trigerCollider.enabled = false;
            anim.SetTrigger("open");
            Vector3 createItemPos = this.gameObject.transform.position + this.gameObject.transform.forward;     //���� �չ���
            int randomKey = GameManager.Instance.GetItemManager.GetRandomItemKey();   //���� Ű
            GameManager.Instance.GetItemManager.CreateDropItem(createItemPos, randomKey);  //���� ������ ����
            //Ű�� ������ �� ������ �̱�
        }
    }   //�÷��̾� ��ȣ�ۿ�

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

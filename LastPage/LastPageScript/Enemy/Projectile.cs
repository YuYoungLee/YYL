using System.Collections;
using UnityEngine;

//����ü ����
public class Projectile : MonoBehaviour
{
    Coroutine fireCoroutine = null;                     //����ü �ڷ�ƾ
    Vector3 startPos = new Vector3(0, 0.8f, 1f);        //�߻� ������ ��ġ
    Vector3[] curveFire = new Vector3[32];              //Ŀ�� ������ ����
    bool fireCheck = false;     //�߻� �غ� �Ǹ� true
    int damage = 0;             //������ ������
    int iCurveSize = 0;         //�ڷ�ƾ ��ȸ�� ����� index

    public void Initialize(int damage)
    {
        this.gameObject.SetActive(false);
        this.damage = damage;                 //����ü�� ���ݷ�
    }

    public bool ReadyFire(Vector3 targetPos, Vector3 StartPos)
    {
        if (fireCheck) return false;        //�߻����϶� false ����
        transform.position = startPos;      //����ü ��ġ �ʱ�ȭ
        curveFire = GameManager.Instance.GetPath(StartPos, targetPos, 3);  //Curve ã��
        return true;
    }   //�߻� �غ� �ɶ� �߻� ������ �߰� true ����

    public void Fire(int damage)
    {
        fireCheck = true;                                      //�߻� �غ� ��Ȱ��ȭ
        this.gameObject.SetActive(true);                       //Ȱ��ȭ
        if (fireCoroutine != null) return;
        fireCoroutine = StartCoroutine(CurveCorutine());
    }   //���� �ʱ�ȭ �� �ڷ�ƾ ����

    public void Stop()
    {
        StopCoroutine(CurveCorutine());         //�ڷ�ƾ ����
        fireCoroutine = null;                   //�ڷ�ƾ ����
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player")) other.GetComponent<Player>().Damaged(damage);   //other.SendMessage("Damaged", damaged);
        else return;

        this.gameObject.SetActive(false);       //����ü ��Ȱ��ȭ
        fireCheck = false;                      //�߻�����
        iCurveSize = 0;                         //index 0 �ʱ�ȭ
        StopCoroutine("CurveCorutine");         //�ڷ�ƾ ����
        fireCoroutine = null;                   //�ڷ�ƾ ����
    }   //����ü �浹�� �� ó��

    IEnumerator CurveCorutine()
    {
        while(iCurveSize < curveFire.Length)
        {
            this.transform.position = curveFire[iCurveSize];
            ++iCurveSize;
            yield return new WaitForSeconds(0.1f);
        }

        fireCheck = false;                      //�߻�����
        this.gameObject.SetActive(false);       //����ü ��Ȱ��ȭ
        iCurveSize = 0;                         //index 0 �ʱ�ȭ
        Stop();
    }   //����ü Ŀ�� �ڷ�ƾ
}

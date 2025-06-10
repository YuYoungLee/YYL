using System.Collections;
using UnityEngine;

public class Bomb : Skill
{
    private Vector3 targetPos = Vector3.zero;
    private Vector3 targetUpperPos = Vector3.zero;
    private Vector3 startPos = Vector3.zero;
    private Vector3 startUpperPos = Vector3.zero;

    protected override void Damage()
    {
        //Ÿ���� �� �ݰ� �ȿ� ������ ������ ������
        //��ġ �̵���ų ��ġ �ʱ�ȭ
        position = this.transform.position;
        Collider[] targets = Physics.OverlapSphere(position, data.AttackRange);
        if (targets != null)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i].CompareTag("Player") && HitCheckCircle(targets[i].transform.position))
                {
                    targets[i].GetComponent<Player>().Damaged(damage);
                }
            }
        }
    }

    /// <summary>
    /// p1 : ������ǥ
    /// p2 : ���� �� ��ǥ
    /// p3 : Ÿ�� �� ��ǥ
    /// p4 : Ÿ�� ��ǥ
    /// </summary>
    protected Vector3 BazierCurve(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float value)
    {
        Vector3 a = Vector3.Lerp(p1, p2, value);
        Vector3 b = Vector3.Lerp(p2, p3, value);
        Vector3 c = Vector3.Lerp(p3, p4, value);

        Vector3 d = Vector3.Lerp(a, b, value);
        Vector3 e = Vector3.Lerp(b, c, value);

        Vector3 f = Vector3.Lerp(d, e, value);
        return f;
    }//Ŀ�� ��ǥ ����

    protected override IEnumerator SkillCoroutine()
    {
        //��ǥ �ʱ�ȭ Lerp ���
        targetPos = GameManager.Instance.GetPlayer().transform.position;
        targetUpperPos = targetPos + Vector3.up * 3.0f;
        startPos = position + forward;
        startUpperPos = startPos + Vector3.up * 5.0f;

        PlayParticle(false, 0, 5);

        Vector3 nextPos = Vector3.zero;
        float sec = 0.0f;
        for (int i = 0; i < 50; ++i)
        {
            nextPos = BazierCurve(position, startUpperPos, targetUpperPos, targetPos, sec);
            this.transform.position = nextPos;
            particle[0].transform.rotation = Quaternion.LookRotation(nextPos.normalized);     //������������ ȸ���ϱ�
            sec += 0.02f;
            yield return new WaitForSeconds(0.02f);
        }
        particle[0].transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);  //ȸ�� �ʱ�ȭ
        StopParticle(0, 5);

        //��ź ������ ȿ��

        PlayParticle(false, 5, 6);
        Damage();
        yield return new WaitForSeconds(1.0f);
        StopParticle(5, 6);
        StopSkill();
    }
}

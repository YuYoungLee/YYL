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
        //타겟이 원 반경 안에 있을때 데미지 입히기
        //위치 이동스킬 위치 초기화
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
    /// p1 : 시작좌표
    /// p2 : 시작 위 좌표
    /// p3 : 타겟 위 좌표
    /// p4 : 타겟 좌표
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
    }//커브 좌표 연산

    protected override IEnumerator SkillCoroutine()
    {
        //좌표 초기화 Lerp 계산
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
            particle[0].transform.rotation = Quaternion.LookRotation(nextPos.normalized);     //다음방향으로 회전하기
            sec += 0.02f;
            yield return new WaitForSeconds(0.02f);
        }
        particle[0].transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);  //회전 초기화
        StopParticle(0, 5);

        //폭탄 터지는 효과

        PlayParticle(false, 5, 6);
        Damage();
        yield return new WaitForSeconds(1.0f);
        StopParticle(5, 6);
        StopSkill();
    }
}

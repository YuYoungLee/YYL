using System.Collections;
using UnityEngine;

public class RotatingSlash : Skill
{
    protected override void Damage()
    {
        //타겟이 원 반경 안에 있을때 데미지 입히기
        Collider[] targets = Physics.OverlapSphere(position, data.AttackRange);
        PlayParticle(false, 0, 1);
        if (targets != null)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i].CompareTag("Enemy") && HitCheckCircle(targets[i].transform.position))
                {
                    targets[i].GetComponent<Enemy>().Damaged(damage);
                }
            }
        }
    }
    protected override IEnumerator SkillCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        Damage();
        yield return new WaitForSeconds(data.Cooltime - 0.1f);
        StopSkill();
    }
}

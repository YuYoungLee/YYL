using UnityEngine;
using System.Collections;

public class SwordAttack : Skill
{
    /*
    public void Damage()
    {
        int damage = mStat.Atk + mDataMgr().GetSkillData(miKey).Damage;    //공격력 계산 스텟데미지 + 스킬 데미지
        Collider[] collider = Physics.OverlapSphere(mStartPos, mDataMgr().GetSkillData(miKey).EndDist, miTargetLayer);     //범위안 콜라이더
        Vector3 enemyPos;
        for (int i = 0; i < collider.Length; ++i)
        {
            if (collider[i].gameObject.layer != miTargetLayer) continue;  //타겟이 아니라면 다음으로

            enemyPos = collider[i].transform.position;      //위치 얻기
            //if (AngleCheck(enemyPos) && DistCheck(enemyPos))        //각도 거리 체크
            //{
            //    collider[i].GetComponent<IDamageable>().Damaged(damage, mStartPos - collider[i].transform.position);  //공격하기
            //}
        }
    }
    public IEnumerator PlaySkillCoroutine()
    {
        yield return new WaitForSeconds(1.0f);
        Damage();
    }
    */
}

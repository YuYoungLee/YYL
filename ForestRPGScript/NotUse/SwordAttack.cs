using UnityEngine;
using System.Collections;

public class SwordAttack : Skill
{
    /*
    public void Damage()
    {
        int damage = mStat.Atk + mDataMgr().GetSkillData(miKey).Damage;    //���ݷ� ��� ���ݵ����� + ��ų ������
        Collider[] collider = Physics.OverlapSphere(mStartPos, mDataMgr().GetSkillData(miKey).EndDist, miTargetLayer);     //������ �ݶ��̴�
        Vector3 enemyPos;
        for (int i = 0; i < collider.Length; ++i)
        {
            if (collider[i].gameObject.layer != miTargetLayer) continue;  //Ÿ���� �ƴ϶�� ��������

            enemyPos = collider[i].transform.position;      //��ġ ���
            //if (AngleCheck(enemyPos) && DistCheck(enemyPos))        //���� �Ÿ� üũ
            //{
            //    collider[i].GetComponent<IDamageable>().Damaged(damage, mStartPos - collider[i].transform.position);  //�����ϱ�
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

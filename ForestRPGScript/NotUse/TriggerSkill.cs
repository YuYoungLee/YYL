using System.Collections;
using UnityEngine;

public class TriggerSkill : Skill
{
    /*
    public IEnumerator PlaySkillCoroutine()
    {
        mbPlaySkill = true;
        //Ǯ���ִ� ������Ʈ ��������
        yield return new WaitForSeconds(mDataMgr().GetSkillData(miKey).StartTime);    //�����ð� (�ִϸ��̼�)

        //Ǯ���� ������Ʈ ��������
        //int pooledCount = mpDataMgr().GetSkillData(miKey).PooledObjectKey;
        PooledObject pooledObj;     //Ǯ����ų

        //����Ʈ key�� ��ȸ
        foreach (int iKey in mDataMgr().GetSkillData(miKey).PooledObjectKey)
        {
            SoundManager.Instance.PlaySFXSound(mDataMgr().GetSkillData(miKey).SkillSoundKey);       //��ų ���� ���
            pooledObj = mOBJPoolMgr().GetObject(iKey);
            if (pooledObj.EPooledObject == EPooledObject.Skill_Strike)
            {
                Strike strikeObj = pooledObj as Strike; //�ٿ��ɽ���
                int iDamage = mStat.Atk + mDataMgr().GetSkillData(miKey).Damage;    //������ ��� (���� ���ݷ� + ��ų ���ݷ�)
                strikeObj.Play(mStartPos, mDir, mDataMgr().GetSkillData(miKey).EndDist, miTargetLayer, iDamage);       //�÷���
            }
        }

        //��ƼŬ ����ó��
        StopParticle();
        mbPlaySkill = false;
        //��Ÿ�� ���
        float fMaxCoolTime = mfCooltime;
        float fAmount = 0;
        while (mfCooltime >= 0.0f)
        {
            yield return null;  //������ ���� ����
            mfCooltime -= Time.deltaTime;
            fAmount = mfCooltime / fMaxCoolTime;
            //��Ÿ�� ǥ�� delegate
            if (mCoolTimeEvent != null)
            {
                mCoolTimeEvent(mfCooltime, fAmount);
            }
        }
        mfCooltime = 0.0f;      //��Ÿ�� �ʱ�ȭ

        //��Ÿ�� ���� ����
        if (mSetCoolTime != null)
        {
            mSetCoolTime(false);       //��Ÿ�� UI ǥ�� ��Ȱ��ȭ
        }
    }
    */
}

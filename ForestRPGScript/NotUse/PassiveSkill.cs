using System.Collections;
using UnityEngine;

public class PassiveSkill : Skill
{
    /*
    public IEnumerator PlaySkillCoroutine()
    {
        //�ʵ� ��ƼŬ ����
        PlayFieldParticle();

        //�����ð� (�ִϸ��̼�)
        yield return new WaitForSeconds(mDataMgr().GetSkillData(miKey).StartTime);

        SoundManager.Instance.PlaySFXSound(mDataMgr().GetSkillData(miKey).SkillSoundKey);       //��ų ���� ���
        //���� ���ϱ�
        mStat.Add(mDataMgr().GetSkillData(miKey).StatDataType, mDataMgr().GetSkillData(miKey).Damage);

        //���� �ð�
        yield return new WaitForSeconds(mDataMgr().GetSkillData(miKey).RepeatTime);

        //���� ����
        mStat.Sum(mDataMgr().GetSkillData(miKey).StatDataType, mDataMgr().GetSkillData(miKey).Damage);

        //��ƼŬ ����ó��
        StopParticle();

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

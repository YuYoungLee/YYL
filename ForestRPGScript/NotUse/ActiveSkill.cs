using System.Collections;
using UnityEngine;

public class ActiveSkill : Skill
{
/*
    public IEnumerator PlaySkillCoroutine()
    {
        mbPlaySkill = true;
        //�ʵ� ��ƼŬ ����
        PlayFieldParticle();

        yield return new WaitForSeconds(mDataMgr().GetSkillData(miKey).StartTime);     //�����ð� (�ִϸ��̼�)

        //Ƚ����ŭ ������ ������
        switch (mDataMgr().GetSkillData(miKey).AttackType)
        {
            case EAttackType.Attack:
                break;
            case EAttackType.Buff:
                break;
            case EAttackType.Magic:
                break;
            default:
                break;
        }

        for (int i = 0, count = mDataMgr().GetSkillData(miKey).DurationCount; i < count; ++i)
        {
            SoundManager.Instance.PlaySFXSound(mDataMgr().GetSkillData(miKey).SkillSoundKey);       //��ų ���� ���
            //Damage();    //������ ������
            PlaySkillParticle();
            yield return new WaitForSeconds(mDataMgr().GetSkillData(miKey).RepeatTime);     //�ݺ� �����ð�
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
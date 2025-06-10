using System.Collections;
using UnityEngine;

public class ActiveSkill : Skill
{
/*
    public IEnumerator PlaySkillCoroutine()
    {
        mbPlaySkill = true;
        //필드 파티클 실행
        PlayFieldParticle();

        yield return new WaitForSeconds(mDataMgr().GetSkillData(miKey).StartTime);     //지연시간 (애니메이션)

        //횟수만큼 데미지 입히기
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
            SoundManager.Instance.PlaySFXSound(mDataMgr().GetSkillData(miKey).SkillSoundKey);       //스킬 사운드 재생
            //Damage();    //데미지 입히기
            PlaySkillParticle();
            yield return new WaitForSeconds(mDataMgr().GetSkillData(miKey).RepeatTime);     //반복 지연시간
        }

        //파티클 멈춤처리
        StopParticle();
        mbPlaySkill = false;
        //쿨타임 계산
        float fMaxCoolTime = mfCooltime;
        float fAmount = 0;
        while (mfCooltime >= 0.0f)
        {
            yield return null;  //프레임 별로 리턴
            mfCooltime -= Time.deltaTime;
            fAmount = mfCooltime / fMaxCoolTime;
            //쿨타임 표시 delegate
            if (mCoolTimeEvent != null)
            {
                mCoolTimeEvent(mfCooltime, fAmount);
            }
        }
        mfCooltime = 0.0f;      //쿨타임 초기화

        //쿨타임 설정 끄기
        if (mSetCoolTime != null)
        {
            mSetCoolTime(false);       //쿨타임 UI 표시 비활성화
        }
    }
*/
}
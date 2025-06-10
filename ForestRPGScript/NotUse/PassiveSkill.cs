using System.Collections;
using UnityEngine;

public class PassiveSkill : Skill
{
    /*
    public IEnumerator PlaySkillCoroutine()
    {
        //필드 파티클 실행
        PlayFieldParticle();

        //지연시간 (애니메이션)
        yield return new WaitForSeconds(mDataMgr().GetSkillData(miKey).StartTime);

        SoundManager.Instance.PlaySFXSound(mDataMgr().GetSkillData(miKey).SkillSoundKey);       //스킬 사운드 재생
        //스텟 더하기
        mStat.Add(mDataMgr().GetSkillData(miKey).StatDataType, mDataMgr().GetSkillData(miKey).Damage);

        //지연 시간
        yield return new WaitForSeconds(mDataMgr().GetSkillData(miKey).RepeatTime);

        //스텟 빼기
        mStat.Sum(mDataMgr().GetSkillData(miKey).StatDataType, mDataMgr().GetSkillData(miKey).Damage);

        //파티클 멈춤처리
        StopParticle();

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

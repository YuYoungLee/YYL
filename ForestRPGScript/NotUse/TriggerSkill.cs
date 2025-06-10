using System.Collections;
using UnityEngine;

public class TriggerSkill : Skill
{
    /*
    public IEnumerator PlaySkillCoroutine()
    {
        mbPlaySkill = true;
        //풀에있는 오브젝트 가져오기
        yield return new WaitForSeconds(mDataMgr().GetSkillData(miKey).StartTime);    //지연시간 (애니메이션)

        //풀에서 오브젝트 꺼내오기
        //int pooledCount = mpDataMgr().GetSkillData(miKey).PooledObjectKey;
        PooledObject pooledObj;     //풀링스킬

        //리스트 key값 순회
        foreach (int iKey in mDataMgr().GetSkillData(miKey).PooledObjectKey)
        {
            SoundManager.Instance.PlaySFXSound(mDataMgr().GetSkillData(miKey).SkillSoundKey);       //스킬 사운드 재생
            pooledObj = mOBJPoolMgr().GetObject(iKey);
            if (pooledObj.EPooledObject == EPooledObject.Skill_Strike)
            {
                Strike strikeObj = pooledObj as Strike; //다운케스팅
                int iDamage = mStat.Atk + mDataMgr().GetSkillData(miKey).Damage;    //데미지 계산 (스텟 공격력 + 스킬 공격력)
                strikeObj.Play(mStartPos, mDir, mDataMgr().GetSkillData(miKey).EndDist, miTargetLayer, iDamage);       //플레이
            }
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

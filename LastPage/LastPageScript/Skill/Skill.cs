using System.Collections;
using UnityEngine;


public abstract class Skill : MonoBehaviour
{
    protected SkillData data = null;                   //스킬의 데이터

    public ParticleSystem[] particle;               //파티클

    protected Coroutine cooltimeCoroutine = null;   //쿨타임 코루틴
    protected Coroutine skillCoroutine = null;      //스킬 사용 코루틴 - 이펙트, 데미지 입히기

    protected int damage = 0;                           //데미지

    protected Vector3 forward = Vector3.zero;           //자신의 앞방향
    protected Vector3 position = Vector3.zero;          //자신의 현제 위치
    protected Quaternion rotation = Quaternion.identity;//자신의 방향

    #region Init
    public void Initialize(SkillData data)
    {
        this.data = data;
        StopParticle(0, particle.Length);
        //Defualt가 아닐때만 이미지 삽입
        if (this.data.SkillKey != SkillKey.Skill_Default)
            GameManager.Instance.GetUIManager().GetPlayerUI.GetActionPanel.SetSkillImage(this.data.SkillKey, this.data.SpriteImage); //UI에 스킬 이미지 삽입
    }   //데이터 삽입

    #endregion
    protected abstract void Damage();


    #region PlayStopLogic
    /// <summary>
    /// position : 자기 자신의 오브젝트 위치
    /// </summary>
    public bool Play(Vector3 position, int damage, Vector3 forward, Quaternion rotation)
    {
        //스킬이 사용중일 경우 false
        if (cooltimeCoroutine != null) return false;

        this.position = position;
        this.damage = damage;
        this.forward = forward;
        cooltimeCoroutine = StartCoroutine(CoolTimeCoroutine());
        skillCoroutine = StartCoroutine(SkillCoroutine());
        return true;
    }   //스킬 작동 했을 때 true 리턴
    public bool CooltimeCheck()
    {
        if (cooltimeCoroutine != null) return true;
        return false;
    }   //쿨타임 중이 아닐 때
    public bool PlayCheck()
    {
        if (skillCoroutine == null) return false;
        return true;
    }   //스킬 플레이 중일때 true 리턴
    protected void StopSkill()
    {
        StopCoroutine(SkillCoroutine());
        skillCoroutine = null;
    }    //공격 코루틴 비우기
    protected void StopCoolTime()
    {
        StopCoroutine(CoolTimeCoroutine());
        cooltimeCoroutine = null;
    }   //스킬 쿨타임 이후 코루틴 비우기

    protected void PlayParticle(bool targetStart, int start, int endLength)
    {
        if(targetStart) this.gameObject.transform.position = GameManager.Instance.GetPlayer().transform.position;

        //파티클 재생
        for (int i = start; i < endLength; ++i)
        {
            particle[i].gameObject.SetActive(true);
            particle[i].Play();
        }
    }   //파티클 재생

    protected void StopParticle(int start, int endLength)
    {
        for (int i = start; i < endLength; ++i)
        {
            particle[i].Stop();
            particle[i].gameObject.SetActive(false);
        }
    }   //파티클 멈춤
    #endregion

    #region HitCarculate
    protected bool HitCheckCircle(Vector3 targetPosition)
    {
        Vector3 distTarget = targetPosition - position;         //타겟으로 향하는 벡터
        float sqrDistTarget = Vector3.SqrMagnitude(distTarget);

        //거리, 각도 안에 있을 시 true 리턴
        if (sqrDistTarget < data.AttackRange * data.AttackRange &&
            GetTargetAngle(distTarget.normalized) <= data.Angle) { return true; }
        return false;
    }   //피격범위, 각도 안에 있을때 true 리턴
    protected float GetTargetAngle(Vector3 targetPosition)
    {
        return Vector3.Angle(forward, targetPosition);
    }   //각도
    #endregion
    protected abstract IEnumerator SkillCoroutine();     //스킬 코루틴
    protected IEnumerator CoolTimeCoroutine()
    {
        if (data.SkillUiStatus)
        {   //플레이어 스킬 사용 일 때
            float sec = 0.0f;
            int time = data.Cooltime;
            while (sec < data.Cooltime)
            {
                sec += 0.1f;
                GameManager.Instance.GetUIManager().GetPlayerUI.GetActionPanel.SkillCoolTimeUI(data.SkillKey, data.Cooltime - (int)sec, sec / data.Cooltime);
                yield return new WaitForSeconds(0.1f);
            }
            GameManager.Instance.GetUIManager().GetPlayerUI.GetActionPanel.ResetTime(data.SkillKey);
        }
        else
        {   //플레이어 스킬 사용이 아닐 때
            yield return new WaitForSeconds(data.Cooltime); 
        }
        
        StopCoolTime();
    }   //쿨타임 코루틴
}

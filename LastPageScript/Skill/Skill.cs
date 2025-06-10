using System.Collections;
using UnityEngine;


public abstract class Skill : MonoBehaviour
{
    protected SkillData data = null;                   //��ų�� ������

    public ParticleSystem[] particle;               //��ƼŬ

    protected Coroutine cooltimeCoroutine = null;   //��Ÿ�� �ڷ�ƾ
    protected Coroutine skillCoroutine = null;      //��ų ��� �ڷ�ƾ - ����Ʈ, ������ ������

    protected int damage = 0;                           //������

    protected Vector3 forward = Vector3.zero;           //�ڽ��� �չ���
    protected Vector3 position = Vector3.zero;          //�ڽ��� ���� ��ġ
    protected Quaternion rotation = Quaternion.identity;//�ڽ��� ����

    #region Init
    public void Initialize(SkillData data)
    {
        this.data = data;
        StopParticle(0, particle.Length);
        //Defualt�� �ƴҶ��� �̹��� ����
        if (this.data.SkillKey != SkillKey.Skill_Default)
            GameManager.Instance.GetUIManager().GetPlayerUI.GetActionPanel.SetSkillImage(this.data.SkillKey, this.data.SpriteImage); //UI�� ��ų �̹��� ����
    }   //������ ����

    #endregion
    protected abstract void Damage();


    #region PlayStopLogic
    /// <summary>
    /// position : �ڱ� �ڽ��� ������Ʈ ��ġ
    /// </summary>
    public bool Play(Vector3 position, int damage, Vector3 forward, Quaternion rotation)
    {
        //��ų�� ������� ��� false
        if (cooltimeCoroutine != null) return false;

        this.position = position;
        this.damage = damage;
        this.forward = forward;
        cooltimeCoroutine = StartCoroutine(CoolTimeCoroutine());
        skillCoroutine = StartCoroutine(SkillCoroutine());
        return true;
    }   //��ų �۵� ���� �� true ����
    public bool CooltimeCheck()
    {
        if (cooltimeCoroutine != null) return true;
        return false;
    }   //��Ÿ�� ���� �ƴ� ��
    public bool PlayCheck()
    {
        if (skillCoroutine == null) return false;
        return true;
    }   //��ų �÷��� ���϶� true ����
    protected void StopSkill()
    {
        StopCoroutine(SkillCoroutine());
        skillCoroutine = null;
    }    //���� �ڷ�ƾ ����
    protected void StopCoolTime()
    {
        StopCoroutine(CoolTimeCoroutine());
        cooltimeCoroutine = null;
    }   //��ų ��Ÿ�� ���� �ڷ�ƾ ����

    protected void PlayParticle(bool targetStart, int start, int endLength)
    {
        if(targetStart) this.gameObject.transform.position = GameManager.Instance.GetPlayer().transform.position;

        //��ƼŬ ���
        for (int i = start; i < endLength; ++i)
        {
            particle[i].gameObject.SetActive(true);
            particle[i].Play();
        }
    }   //��ƼŬ ���

    protected void StopParticle(int start, int endLength)
    {
        for (int i = start; i < endLength; ++i)
        {
            particle[i].Stop();
            particle[i].gameObject.SetActive(false);
        }
    }   //��ƼŬ ����
    #endregion

    #region HitCarculate
    protected bool HitCheckCircle(Vector3 targetPosition)
    {
        Vector3 distTarget = targetPosition - position;         //Ÿ������ ���ϴ� ����
        float sqrDistTarget = Vector3.SqrMagnitude(distTarget);

        //�Ÿ�, ���� �ȿ� ���� �� true ����
        if (sqrDistTarget < data.AttackRange * data.AttackRange &&
            GetTargetAngle(distTarget.normalized) <= data.Angle) { return true; }
        return false;
    }   //�ǰݹ���, ���� �ȿ� ������ true ����
    protected float GetTargetAngle(Vector3 targetPosition)
    {
        return Vector3.Angle(forward, targetPosition);
    }   //����
    #endregion
    protected abstract IEnumerator SkillCoroutine();     //��ų �ڷ�ƾ
    protected IEnumerator CoolTimeCoroutine()
    {
        if (data.SkillUiStatus)
        {   //�÷��̾� ��ų ��� �� ��
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
        {   //�÷��̾� ��ų ����� �ƴ� ��
            yield return new WaitForSeconds(data.Cooltime); 
        }
        
        StopCoolTime();
    }   //��Ÿ�� �ڷ�ƾ
}

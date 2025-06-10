using UnityEditor;
using UnityEngine;

public enum EPlayerAnim
{
    Attack0,
    Attack1,
    Attack2,
    Attack3,
}

public class PlayerAnim : IPlayerAnim
{
    private Animator anim;

    public Animator SetAnim
    { set { anim = value; } }   //���ϸ��̼�

    public void Move(float xDir, float yDir)
    {
        anim.SetFloat("xDir", xDir);
        anim.SetFloat("yDir", yDir);
        anim.SetBool("isMove", true);
    }

    public void Stop()
    {
        anim.SetBool("isMove", false);
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
    }

    /// <summary>
    /// iSkillKey : ��ų Ű��
    /// </summary>
    public void Attack(int iSkillKey)
    {
        //���ϸ��̼� Ÿ�� �ҷ�����
        switch (GameManager.Instance.GetDataMgr().GetSkillData(iSkillKey).PlayerAnim)
        {
            case EPlayerAnim.Attack0:
                anim.SetTrigger("Attack0");
                break;
            case EPlayerAnim.Attack1:
                anim.SetTrigger("Attack1");
                break;
            case EPlayerAnim.Attack2:
                anim.SetTrigger("Attack2");
                break;
            case EPlayerAnim.Attack3:
                anim.SetTrigger("Attack3");
                break;
        }
    }   //�÷��̾� ���� ���ϸ��̼�

    public void Damaged()
    {
        anim.SetTrigger("Damaged");
    }

    public void Dead()
    {
        anim.SetTrigger("Die");
    }

    public void Reset()
    {
        anim.SetFloat("xDir", 0.0f);
        anim.SetFloat("yDir", 0.0f);
        anim.Play("Idle");
    }
}

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
    { set { anim = value; } }   //에니메이션

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
    /// iSkillKey : 스킬 키값
    /// </summary>
    public void Attack(int iSkillKey)
    {
        //에니메이션 타입 불러오기
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
    }   //플레이어 공격 에니매이션

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

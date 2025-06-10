using UnityEngine;

public class EnemyAnim : IEnemyAnim
{
    private Animator anim;

    public Animator SetAnim 
    { set { anim = value; } }   //에니메이션


    public void Reset()
    {
        anim.Play("IdleMove");
        anim.SetFloat("Speed", 0.0f);
    }   //에니메이션 초기화
    /// <summary>
    /// speed : 0.0f 멈춤, 1.0f 움직임
    /// </summary>
    public void Move(float speed)
    {
        anim.SetFloat("Speed", speed);
    }   //움직이는 모션

    public void Stop()
    {
        anim.SetFloat("Speed", 0.0f);
    }   //멈춤 모션

    public void Attack()
    {
        anim.SetTrigger("Attack");
    }   //공격 모션

    public void Damaged()
    {
        anim.SetTrigger("Damaged");
    }   //데미지 입는 모션

    public void Die()
    {
        anim.SetTrigger("Die");
    }   //사망시 모션
}

using UnityEngine;

public class EnemyAnim : IEnemyAnim
{
    private Animator anim;

    public Animator SetAnim 
    { set { anim = value; } }   //���ϸ��̼�


    public void Reset()
    {
        anim.Play("IdleMove");
        anim.SetFloat("Speed", 0.0f);
    }   //���ϸ��̼� �ʱ�ȭ
    /// <summary>
    /// speed : 0.0f ����, 1.0f ������
    /// </summary>
    public void Move(float speed)
    {
        anim.SetFloat("Speed", speed);
    }   //�����̴� ���

    public void Stop()
    {
        anim.SetFloat("Speed", 0.0f);
    }   //���� ���

    public void Attack()
    {
        anim.SetTrigger("Attack");
    }   //���� ���

    public void Damaged()
    {
        anim.SetTrigger("Damaged");
    }   //������ �Դ� ���

    public void Die()
    {
        anim.SetTrigger("Die");
    }   //����� ���
}

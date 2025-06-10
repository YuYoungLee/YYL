using UnityEngine;

[CreateAssetMenu(fileName = "DamagedState", menuName = "ScriptableObject/FSM Asset/DamagedState", order = 4)]
public class DamagedState : ScriptableObject, IState
{
    public void Enter(Enemy owner)
    {
        owner.Stop();
        owner.SetDamaged();
    }   //������ ���� �ʱ�ȭ

    public void Excute(Enemy owner)
    {//Ÿ����  ��ó�� ������ �ڦi�� ���� ��ȯ
        //������ ���� �� ��
        if (owner.HitDelayCheck()) return;

        //���� ���� �ȿ� ������, ���� ������Ʈ�� ����
        if (owner.AttackRangeCheck())
        { owner.ChangeState(EnemyState.Attack); }
        else { owner.ChangeState(EnemyState.Chase); }   //�ڦi�� ���·� ��ȯ
    }   //�¾��� �� �ð�üũ

    public void Exit(Enemy owner)
    {

    }
}

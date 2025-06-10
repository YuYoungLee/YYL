using UnityEngine;

[CreateAssetMenu(fileName = "ChaseState", menuName = "ScriptableObject/FSM Asset/ChaseState", order = 2)]
public class ChaseState : ScriptableObject, IState
{
    //�ڦi�� ���� Ŭ����

    public void Enter(Enemy owner)
    {
        owner.Stop();
    }   //������ ���� �ʱ�ȭ

    public void Excute(Enemy owner)
    {
        //Ÿ���� ���� ���� �ȿ� �ִ���
        if (owner.AttackRangeCheck()) owner.ChangeState(EnemyState.Attack);
        else if(owner.SearchRangeCheck()) owner.MoveToTarget();
        else owner.ChangeState(EnemyState.Idle);    //���̵� ����
    }   //�ش� ������Ʈ�� ���ݹ��� �ȿ� ������ ������ȯ, �ƴҽ� ��� �����̱�

    public void Exit(Enemy owner)
    {

    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "IdleState", menuName = "ScriptableObject/FSM Asset/IdleState", order = 1)]
public class IdleState : ScriptableObject, IState
{
    public void Enter(Enemy owner)
    {
        owner.Stop();
    }   //������ ���� �ʱ�ȭ

    public void Excute(Enemy owner)
    {//Ÿ����  ��ó�� ������ �ڦi�� ���� ��ȯ
        if (owner.SearchRangeCheck()) owner.ChangeState(EnemyState.Chase);
        else owner.Stop();  //�´� ������ ���� �� Idle ����
    }   //Ž������ �ȿ� ������ �ڦi��, ������ ������ ���� �ִ� ����

    public void Exit(Enemy owner)
    {

    }
}

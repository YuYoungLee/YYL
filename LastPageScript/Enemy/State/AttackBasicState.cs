using UnityEngine;

[CreateAssetMenu(fileName = "AttackBasicState", menuName = "ScriptableObject/FSM Asset/AttackBasicState", order = 3)]
public class AttackBasicState : ScriptableObject, IState
{
    public void Enter(Enemy owner)
    {
        owner.ReadyAttack();
    }   //������ ���� �ʱ�ȭ, ���ݽ� ���� �ʱ�ȭ

    public void Excute(Enemy owner)
    {
        //������ ���ݼӵ� �غ������� �˻�
        if (owner.AttackDurationCheck()) return;

        //�Ÿ� �ۿ� ������ �ڦi�� ���·� ��ȯ
        if (!owner.AttackRangeCheck())
        { owner.ChangeState(EnemyState.Chase); return; }

        owner.AttackToTarget();    //�÷��̾� �������� ����
    }   //���ݻ���

    public void Exit(Enemy owner)
    {

    }
}

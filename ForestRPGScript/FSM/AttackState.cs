public class AttackState : IState
{
    public void Enter(FSMAble owner)
    {
        owner.AttackEnter();
    }//State ���� ���� ��
    public void Excute(FSMAble owner)
    {
        owner.AttackState();
    }//State �ֱ⺰ ����
    public void Exit(FSMAble owner)
    {
        owner.AttackExit();
    }//State ���� �� ��
}

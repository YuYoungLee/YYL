public class IdleState : IState
{
    public void Enter(FSMAble owner)
    {
        owner.IdleStateEnter();
    }//State ���� ���� ��
    public void Excute(FSMAble owner)
    {
        owner.IdleState();
    }//State �ֱ⺰ ����
    public void Exit(FSMAble owner)
    {

    }//State ���� �� ��
}

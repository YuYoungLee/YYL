public class DeadState : IState
{
    public void Enter(FSMAble owner)
    {
        owner.DeadStateEnter();
    }//State ���� ���� ��
    public void Excute(FSMAble owner)
    {
    }//State �ֱ⺰ ����
    public void Exit(FSMAble owner)
    {
    }//State ���� �� ��
}

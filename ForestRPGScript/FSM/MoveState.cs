public class MoveState : IState
{
    public void Enter(FSMAble owner)
    {
        //������ ���� �״��
        owner.MoveEnter();
    }//State ���� ���� ��
    public void Excute(FSMAble owner)
    {
        //������ ���� �״��
        owner.MoveState();
    }//State �ֱ⺰ ����
    public void Exit(FSMAble owner)
    {
        owner.MoveExit();
    }//State ���� �� ��
}

public class DamagedState : IState
{
    public void Enter(FSMAble owner)
    {
        owner.DamagedStateEnter();
    }//State ���� ���� ��
    public void Excute(FSMAble owner)
    {
        owner.DamagedState();
    }//State �ֱ⺰ ����
    public void Exit(FSMAble owner)
    {
        owner.DamagedExit();
    }//State ���� �� ��
}

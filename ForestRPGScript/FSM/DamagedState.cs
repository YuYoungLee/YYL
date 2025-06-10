public class DamagedState : IState
{
    public void Enter(FSMAble owner)
    {
        owner.DamagedStateEnter();
    }//State 변경 했을 때
    public void Excute(FSMAble owner)
    {
        owner.DamagedState();
    }//State 주기별 실행
    public void Exit(FSMAble owner)
    {
        owner.DamagedExit();
    }//State 변경 할 때
}

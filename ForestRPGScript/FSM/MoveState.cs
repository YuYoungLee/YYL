public class MoveState : IState
{
    public void Enter(FSMAble owner)
    {
        //움직임 상태 그대로
        owner.MoveEnter();
    }//State 변경 했을 때
    public void Excute(FSMAble owner)
    {
        //움직임 상태 그대로
        owner.MoveState();
    }//State 주기별 실행
    public void Exit(FSMAble owner)
    {
        owner.MoveExit();
    }//State 변경 할 때
}

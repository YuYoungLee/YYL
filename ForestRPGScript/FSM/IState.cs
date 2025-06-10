public interface IState
{
    public void Enter(FSMAble owner);    //State 변경 했을 때
    public void Excute(FSMAble owner);   //State 주기별 실행
    public void Exit(FSMAble owner);     //State 변경 할 때
}

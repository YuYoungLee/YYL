public class ReturnState : IState
{
    public void Enter(FSMAble owner)
    {
        owner.ReturnStateEnter();
    }

    public void Excute(FSMAble owner)
    {
        owner.ReturnState();
    }

    public void Exit(FSMAble owner)
    {
        owner.ReturnExit();
    }
}

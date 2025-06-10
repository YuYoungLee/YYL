public enum FSMState
{
    Idle,      //멈춤 상태
    Move,      //움직임 상태
    Attack,    //공격 상태
    Return,    //돌아가는 상태
    Damaged,   //피해입는 상태
    Dead,      //사망한 상태
}
public class FSM
{
    private FSMAble owner = null;       //동작의 주체
    private IState currState;           //상태

    /// <summary>
    /// own : 동작할 자신
    /// </summary>
    public void Initialize(FSMAble own)
    {
        if (own == null)
        {
            UnityEngine.Debug.LogError("owner is null");
            return;
        }
        owner = own;
        currState = GameManager.Instance.GetDataMgr().GetState(FSMState.Idle);

        if(currState == null)
        {
            UnityEngine.Debug.LogError("currStateSet is null");
        }
    }

    public void UpdateState()
    {
        currState.Excute(owner);
    }   //상태 실행

    /// <summary>
    /// eState : 변경할 상태
    /// </summary>
    public void ChangeState(FSMState eState)
    {
        currState.Exit(owner);
        currState = GameManager.Instance.GetDataMgr().GetState(eState);     //스테이트 변경
        currState.Enter(owner);
    }   //FSM 스테이트 변경
}

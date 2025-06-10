public enum FSMState
{
    Idle,      //���� ����
    Move,      //������ ����
    Attack,    //���� ����
    Return,    //���ư��� ����
    Damaged,   //�����Դ� ����
    Dead,      //����� ����
}
public class FSM
{
    private FSMAble owner = null;       //������ ��ü
    private IState currState;           //����

    /// <summary>
    /// own : ������ �ڽ�
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
    }   //���� ����

    /// <summary>
    /// eState : ������ ����
    /// </summary>
    public void ChangeState(FSMState eState)
    {
        currState.Exit(owner);
        currState = GameManager.Instance.GetDataMgr().GetState(eState);     //������Ʈ ����
        currState.Enter(owner);
    }   //FSM ������Ʈ ����
}

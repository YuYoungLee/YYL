using UnityEngine;

public interface IState
{
    void Enter(Enemy owner);
    void Excute(Enemy owner);
    void Exit(Enemy owner);
}

public class EnemyStateData
{
    public IState idle = null;         //���� ���� ����
    public IState die = null;          //���� ��� ����
    public IState attackBasic = null;  //���� �⺻ ���� ����
    public IState chase = null;        //���� �ڦi�� ����
    public IState damaged = null;      //���� �´� ����

    public EnemyStateData()
    {
        idle = Resources.Load<IdleState>("ScriptableObject/IdleState");
        attackBasic = Resources.Load<AttackBasicState>("ScriptableObject/AttackBasicState");
        chase = Resources.Load<ChaseState>("ScriptableObject/ChaseState");
        damaged = Resources.Load<DamagedState>("ScriptableObject/DamagedState");
        die = Resources.Load<DieState>("ScriptableObject/DieState");
        //Todo Die State �����
    }   //�����ڿ��� �ε�
}

public class FSM
{
    Enemy owner = null;         //FSM�� ������
    IState currState = null;    //���� State

    public FSM(Enemy owner)
    { this.owner = owner; } //FSM�� owner ����
    
    public void ChangeState(IState state)
    {
        //�Ű������� null�Ͻ� ����
        if (state == null) { Debug.LogError("FSM.ChangeState() : state is null"); return; }         
        currState.Exit(owner);              //������ State.Exit ����
        currState = state;                  //���� ��ȯ ����
        currState.Enter(owner);             //����� State.Enter ����
    }   //���� ��ȯ

    public void StateUpdate()
    {
        if(currState != null)
            currState.Excute(owner);
    }   //CurrState�� null�� �ƴҶ��� �۵�

    public void SetState(IState state)
    {
        currState = state;
    }   //State ù ������ �ʱ�ȭ

    public void DeleteState()
    {
        currState = null;
    }   //State ��� ���ҽ� ����
}

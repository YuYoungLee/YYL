using UnityEngine;

public interface IState
{
    void Enter(Enemy owner);
    void Excute(Enemy owner);
    void Exit(Enemy owner);
}

public class EnemyStateData
{
    public IState idle = null;         //적의 유휴 상태
    public IState die = null;          //적의 사망 상태
    public IState attackBasic = null;  //적의 기본 공격 상태
    public IState chase = null;        //적의 뒤쫒는 상태
    public IState damaged = null;      //적의 맞는 상태

    public EnemyStateData()
    {
        idle = Resources.Load<IdleState>("ScriptableObject/IdleState");
        attackBasic = Resources.Load<AttackBasicState>("ScriptableObject/AttackBasicState");
        chase = Resources.Load<ChaseState>("ScriptableObject/ChaseState");
        damaged = Resources.Load<DamagedState>("ScriptableObject/DamagedState");
        die = Resources.Load<DieState>("ScriptableObject/DieState");
        //Todo Die State 만들기
    }   //생성자에서 로드
}

public class FSM
{
    Enemy owner = null;         //FSM의 소유주
    IState currState = null;    //현재 State

    public FSM(Enemy owner)
    { this.owner = owner; } //FSM의 owner 삽입
    
    public void ChangeState(IState state)
    {
        //매개변수가 null일시 리턴
        if (state == null) { Debug.LogError("FSM.ChangeState() : state is null"); return; }         
        currState.Exit(owner);              //기존의 State.Exit 실행
        currState = state;                  //상태 전환 변경
        currState.Enter(owner);             //변경된 State.Enter 실행
    }   //상태 전환

    public void StateUpdate()
    {
        if(currState != null)
            currState.Excute(owner);
    }   //CurrState가 null이 아닐때만 작동

    public void SetState(IState state)
    {
        currState = state;
    }   //State 첫 설정시 초기화

    public void DeleteState()
    {
        currState = null;
    }   //State 사용 안할시 삭제
}

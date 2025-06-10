using UnityEngine;

public abstract class FSMAble : PooledObject
{
    protected FSM mFSM = null;    //상태머신
    public override void Initialize()
    {
        mFSM = new FSM();
        mFSM.Initialize(this);
    }
    public abstract void IdleStateEnter();
    public abstract void IdleState();       //아이들 상태 동작
    public abstract void DamagedStateEnter();     //데미지 상태 진입
    public abstract void DamagedState();    //데미지 상태 동작
    public abstract void DamagedExit();     //데미지 상태 탈출
    public abstract void DeadStateEnter();  //사망 상태 진입
    public abstract void MoveEnter();       //움직임 상태 진입
    public abstract void MoveState();       //움직임 상태 동작
    public abstract void MoveExit();        //움직임 상태 탈출
    public abstract void AttackEnter();     //공격 상태 진입
    public abstract void AttackState();     //공격 상태 동작
    public abstract void AttackExit();      //공격 상태 탈출
    public abstract void ReturnStateEnter();     //돌아가는 상태 진입
    public abstract void ReturnState();          //돌아가는 상태 동작
    public abstract void ReturnExit();          //돌아가는 상태 탈출
}

using UnityEngine;

public abstract class FSMAble : PooledObject
{
    protected FSM mFSM = null;    //���¸ӽ�
    public override void Initialize()
    {
        mFSM = new FSM();
        mFSM.Initialize(this);
    }
    public abstract void IdleStateEnter();
    public abstract void IdleState();       //���̵� ���� ����
    public abstract void DamagedStateEnter();     //������ ���� ����
    public abstract void DamagedState();    //������ ���� ����
    public abstract void DamagedExit();     //������ ���� Ż��
    public abstract void DeadStateEnter();  //��� ���� ����
    public abstract void MoveEnter();       //������ ���� ����
    public abstract void MoveState();       //������ ���� ����
    public abstract void MoveExit();        //������ ���� Ż��
    public abstract void AttackEnter();     //���� ���� ����
    public abstract void AttackState();     //���� ���� ����
    public abstract void AttackExit();      //���� ���� Ż��
    public abstract void ReturnStateEnter();     //���ư��� ���� ����
    public abstract void ReturnState();          //���ư��� ���� ����
    public abstract void ReturnExit();          //���ư��� ���� Ż��
}

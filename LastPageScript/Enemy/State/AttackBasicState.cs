using UnityEngine;

[CreateAssetMenu(fileName = "AttackBasicState", menuName = "ScriptableObject/FSM Asset/AttackBasicState", order = 3)]
public class AttackBasicState : ScriptableObject, IState
{
    public void Enter(Enemy owner)
    {
        owner.ReadyAttack();
    }   //이전의 상태 초기화, 공격시 변수 초기화

    public void Excute(Enemy owner)
    {
        //공격전 공격속도 준비중인지 검사
        if (owner.AttackDurationCheck()) return;

        //거리 밖에 있을시 뒤쫒는 상태로 전환
        if (!owner.AttackRangeCheck())
        { owner.ChangeState(EnemyState.Chase); return; }

        owner.AttackToTarget();    //플레이어 방향으로 공격
    }   //공격상태

    public void Exit(Enemy owner)
    {

    }
}

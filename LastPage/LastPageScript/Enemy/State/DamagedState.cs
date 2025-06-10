using UnityEngine;

[CreateAssetMenu(fileName = "DamagedState", menuName = "ScriptableObject/FSM Asset/DamagedState", order = 4)]
public class DamagedState : ScriptableObject, IState
{
    public void Enter(Enemy owner)
    {
        owner.Stop();
        owner.SetDamaged();
    }   //이전의 상태 초기화

    public void Excute(Enemy owner)
    {//타겟이  근처에 있을때 뒤쫒기 상태 전환
        //딜레이 상태 일 때
        if (owner.HitDelayCheck()) return;

        //공격 범위 안에 있을때, 공격 스테이트로 변경
        if (owner.AttackRangeCheck())
        { owner.ChangeState(EnemyState.Attack); }
        else { owner.ChangeState(EnemyState.Chase); }   //뒤쫒는 상태로 변환
    }   //맞았을 때 시간체크

    public void Exit(Enemy owner)
    {

    }
}

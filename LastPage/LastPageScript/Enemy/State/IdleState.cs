using UnityEngine;

[CreateAssetMenu(fileName = "IdleState", menuName = "ScriptableObject/FSM Asset/IdleState", order = 1)]
public class IdleState : ScriptableObject, IState
{
    public void Enter(Enemy owner)
    {
        owner.Stop();
    }   //이전의 상태 초기화

    public void Excute(Enemy owner)
    {//타겟이  근처에 있을때 뒤쫒기 상태 전환
        if (owner.SearchRangeCheck()) owner.ChangeState(EnemyState.Chase);
        else owner.Stop();  //맞는 조건이 없을 시 Idle 상태
    }   //탐색범위 안에 있을때 뒤쫒기, 조건이 없을때 쉬고 있는 상태

    public void Exit(Enemy owner)
    {

    }
}

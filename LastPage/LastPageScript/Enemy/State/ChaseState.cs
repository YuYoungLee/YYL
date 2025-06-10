using UnityEngine;

[CreateAssetMenu(fileName = "ChaseState", menuName = "ScriptableObject/FSM Asset/ChaseState", order = 2)]
public class ChaseState : ScriptableObject, IState
{
    //뒤쫒는 상태 클래스

    public void Enter(Enemy owner)
    {
        owner.Stop();
    }   //이전의 상태 초기화

    public void Excute(Enemy owner)
    {
        //타겟이 공격 범위 안에 있는지
        if (owner.AttackRangeCheck()) owner.ChangeState(EnemyState.Attack);
        else if(owner.SearchRangeCheck()) owner.MoveToTarget();
        else owner.ChangeState(EnemyState.Idle);    //아이들 상태
    }   //해당 오브젝트가 공격범위 안에 있을때 상태전환, 아닐시 계속 움직이기

    public void Exit(Enemy owner)
    {

    }
}

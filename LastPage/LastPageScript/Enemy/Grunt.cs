using UnityEngine;
using UnityEngine.AI;
//TODO FSM 만들기

public class Grunt : Enemy
{    
    float attackDuration = 0;                     //공격 속도
    float hitDelay = 0;
    
    readonly float attackDurationTime = 1.0f;        //공격시간
    readonly float hitDelayTime = 0.5f;              //맞을때 딜레이 시간
    
    public override void Initialize(Monster monster)
    {
        this.monster = monster;
        anim = GetComponent<Animator>();            //그런트 에니매이션
        agent = GetComponent<NavMeshAgent>();       //AI
        agent.enabled = false;
        data = Resources.Load<EnemyData>("ScriptableObject/GruntData");    //GruntData
        enemyStateData = new EnemyStateData();      //State 데이터 모음
        stateMachine = new FSM(this);               //FSM State 설정
        audioSource = gameObject.AddComponent<AudioSource>();
        soundData = Resources.Load<SoundClipData>("ScriptableObject/SoundData/GruntSoundData");
        this.gameObject.SetActive(false);           //해당 오브젝트 비활성화 처리
    }
    //FSM 그런트 몬스터의 행동관련 함수
    #region GruntFSM
    public override void AttackToTarget()
    {
        //플레이어 방향으로 바라본 후 초기화, 공격처리
        if (!LookRotationToTarget()) return;
        Stop();
        anim.SetTrigger("Attack");
        Invoke("Attack", 0.35f);  //0,37초 지연, 0,5초마다 반복
    }   //그런트의 공격
    
    public override void MoveToTarget()
    {
        if(!targetRigid) return;
        agent.isStopped = false;                                    //AI 에이전트의 움직임 처리
        agent.SetDestination(targetRigid.position);
        anim.SetFloat("MoveFront", agent.velocity.magnitude);       //에니매이션 Walk 상태
    }   //그런트의 움직임 처리
    
    public override void Stop()
    {
        anim.SetFloat("MoveFront", 0.0f);       //에니매이션 Idle 상태
        agent.isStopped = true;
        agent.ResetPath();                      //AI 길찾기 리셋
    }   //그런트의 멈춤 처리
    
    public override void ReadyAttack()
    {
        attackDuration = 0;
    }   //공격전 변수 초기화

    public override void SetDamaged()
    {
        hitDelay = data.HitDelay;
        anim.SetTrigger("Damaged");
    }   //데미지를 입었을때
    #endregion

    //FSM 거리, 각도, 시간 체크 함수
    #region FSMcheck
    public override bool SearchRangeCheck()
    {
        return TargetInRange(data.SearchRangeSquare);
    }   //적이 탐색범위 안에 있을때 true 리턴
    public override bool AttackRangeCheck()
    {
        return TargetInRange(data.AttackRangeSquare);
    }   //적이 공격범위 안에 있을때 true 리턴
    public override bool AttackDurationCheck()
    {
        if (0 < attackDuration)
        {
            attackDuration -= attackDurationTime;
            return true;
        }
        return false;
    }   //공격 속도 시간이 준비 안될시 true 리턴, 준비될시 false 리턴
    public override bool HitDelayCheck()
    {
        if (0 < hitDelay) 
        {
            hitDelay -= hitDelayTime;
            return true;
        }
        return false;
    }   //맞았을 때 딜레이
    #endregion

    protected override void Attack()
    {
        attackDuration = data.AttackDelay;
        //TargetMask == enemy, 공격범위, 각도 안에 있을때
        Collider[] targets = Physics.OverlapSphere(transform.position, data.AttackRange, data.TargetMask);
        if (targets != null && TargetInRange(data.AttackRangeSquare) && TargetInAngle(data.AttackAngle))
        { targets[0].GetComponent<Player>().Damaged(data.Damege); }
    }   //공격 판정이 맞을때 플레이어에게 대미지를 입힌다.

    protected override void StopCoroutine()
    {

    }
}

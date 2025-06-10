using UnityEngine;
using UnityEngine.AI;

public class Spider : Enemy
{
    public Projectile projectile;                     //투사체 이펙트
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
        data = Resources.Load<EnemyData>("ScriptableObject/SpiderData");    //GruntData
        projectile.Initialize(data.Damege);         //투사체 Init
        enemyStateData = new EnemyStateData();     //State 데이터 모음
        stateMachine = new FSM(this);               //FSM State 설정
        audioSource = gameObject.AddComponent<AudioSource>();
        soundData = Resources.Load<SoundClipData>("ScriptableObject/SoundData/SpiderSoundData");
        this.gameObject.SetActive(false);           //해당 오브젝트 비활성화 처리
    }

    #region SpiderFSM
    public override void AttackToTarget()
    {
        //플레이어 방향으로 바라본 후 초기화, 공격처리
        if (!LookRotationToTarget()) return;
        Stop();
        if (projectile.ReadyFire(targetRigid.transform.position, gameObject.transform.position))
        {
            anim.SetTrigger("Attack");
            Invoke("Attack", 0.35f);  //0,35초 지연
        }
    }     //타겟으로 공격
    public override void MoveToTarget()
    {
        if (!targetRigid) return;
        agent.isStopped = false;                                    //AI 에이전트의 움직임 처리
        agent.SetDestination(targetRigid.position);
        anim.SetFloat("MoveFront", agent.velocity.magnitude);       //에니매이션 Walk 상태
    }       //타겟 방향으로 움직임
    public override void Stop()
    {
        anim.SetFloat("MoveFront", 0.0f);       //에니매이션 Idle 상태
        agent.isStopped = true;
        agent.ResetPath();                      //AI 길찾기 리셋
    }               //그런트의 멈춤 처리
    public override void ReadyAttack()
    {
        attackDuration = 0;
    }        //공격전 변수 초기화
    public override void SetDamaged()
    {
        hitDelay = data.HitDelay;
        anim.SetTrigger("Damaged");
    }         //데미지를 입었을때
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
        projectile.Fire(data.Damege);
    }    //투사체 공격

    protected override void StopCoroutine()
    {
        projectile.Stop();
    }
}

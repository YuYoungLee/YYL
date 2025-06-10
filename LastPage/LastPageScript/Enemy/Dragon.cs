using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

enum AttackPattern
{
    FireWall,
    Bomb,
    AttackPatternSize,
}

public class Dragon : Enemy
{
    private float attackDuration = 0;                //공격 속도
    private float hitDelay = 0;

    readonly float attackDurationTime = 1.0f;        //공격시간
    readonly float hitDelayTime = 0.5f;              //맞을때 딜레이 시간
    protected Skill[] skill = new Skill[2];          //스킬

    public override void Initialize(Monster monster)
    {
        this.monster = monster;
        anim = GetComponent<Animator>();            //에니매이션
        agent = GetComponent<NavMeshAgent>();       //AI
        agent.enabled = false;
        data = Resources.Load<EnemyData>("ScriptableObject/DragonData");    //데이터 삽입

        for(int i = 0; i < 2; ++i)
        {
            skill[i] = Instantiate(data.SkillData[i].Skill);    //스킬소유
            skill[i].gameObject.transform.SetParent(this.gameObject.transform);
            skill[i].Initialize(data.SkillData[i]);             //스킬 데이터 삽입
        }

        enemyStateData = new EnemyStateData();      //State 데이터 모음
        stateMachine = new FSM(this);               //FSM State 설정
        audioSource = gameObject.AddComponent<AudioSource>();
        soundData = Resources.Load<SoundClipData>("ScriptableObject/SoundData/DragonSoundData");
        this.gameObject.SetActive(false);           //해당 오브젝트 비활성화 처리
    }
    //FSM 행동관련 함수
    #region FSM
    public override void AttackToTarget()
    {
        //플레이어 방향으로 바라본 후 초기화, 공격처리
        if (!LookRotationToTarget()) return;
        Stop();
        switch ((AttackPattern)Random.Range(0, 2))
        {
            case AttackPattern.FireWall:
                anim.SetTrigger("AttackBasic");
                skill[0].gameObject.SetActive(true);
                skill[0].Play(this.gameObject.transform.position, data.Damege, this.gameObject.transform.forward, this.gameObject.transform.rotation);
                audioSource.PlayOneShot(soundData.GetAudioClip(SFXBossSoundType.Attack));
                break;
            case AttackPattern.Bomb:
                anim.SetTrigger("HornAttack");
                skill[1].gameObject.SetActive(true);
                skill[1].Play(this.gameObject.transform.position, data.Damege, this.gameObject.transform.forward, this.gameObject.transform.rotation);
                audioSource.PlayOneShot(soundData.GetAudioClip(SFXBossSoundType.Attack));
                break;
        }
    }   //공격 처리

    public override void MoveToTarget()
    {
        if (!targetRigid) return;
        agent.isStopped = false;                                    //AI 에이전트의 움직임 처리
        agent.SetDestination(targetRigid.position);
        anim.SetFloat("MoveFront", 1.0f);       //에니매이션 Walk 상태
    }   //움직임 처리

    public override void Stop()
    {
        anim.SetFloat("MoveFront", 0.0f);       //에니매이션 Idle 상태
        agent.isStopped = true;
        agent.ResetPath();                      //AI 길찾기 리셋
    }   //멈춤 처리

    public override void ReadyAttack()
    {
        attackDuration = 0;
    }   //공격전 변수 초기화

    public override void SetDamaged()
    {
        hitDelay = data.HitDelay;
        //anim.SetTrigger("Damaged");
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
        //스킬이 사용 중이라면 true
        for(int i = 0; i < skill.Length; ++i)
        { if (skill[i].PlayCheck()) { return true; } }
        
        //스킬이 사용하지 않을 때 공격 시간 감소
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
        Collider[] targets = Physics.OverlapSphere(transform.position, data.AttackRange, data.TargetMask);
        if (targets != null && TargetInRange(data.AttackRangeSquare) && TargetInAngle(data.AttackAngle))
        { targets[0].GetComponent<Player>().Damaged(data.Damege); }
    }   //공격 판정이 맞을때 플레이어에게 대미지를 입힌다.

    protected override void StopCoroutine()
    {

    }
}

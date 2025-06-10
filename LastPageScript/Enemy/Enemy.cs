using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    Chase,
    Attack,
    Damaged,
    Die
}   //ChangeState시 받을것

public abstract class Enemy : MonoBehaviour
{
    protected bool DieStatus = false;   //사망 상태
    protected Monster monster;          //몬스터타입
    protected int hp;                   //체력
    protected float hitTimer;           //맞았을때 딜레이 타이머

    protected EnemyHealthBar healthBar = null;  //몬스터의 체력바

    protected Rigidbody targetRigid = null;     //플레이어의 rigidbody
    protected NavMeshAgent agent;               //AI 메시 에이전트
    protected Animator anim;                    //에니메이션

    protected EnemyData data = null;            //ScriptableObject Data
    protected EnemyStateData enemyStateData = null;       //그런트의 StateData

    protected FSM stateMachine = null;          //FSM 머신
    readonly float waitUpdateTime = 0.5f;       //FSM 대기시간
    protected Coroutine coroutineFSM = null;    //FSM의 코루틴

    protected AudioSource audioSource = null;   //오디오 재생
    protected SoundClipData soundData = null;   //사운드 데이터

    #region data Init
    public abstract void Initialize(Monster monster);
    #endregion

    #region FSM Func
    //지연 < 실행하는쪽에서 float값 넘겨주기
    protected IEnumerator OnUpdate(float waitSec = 0.0f)
    {
        yield return new WaitForSeconds(waitSec);
        while (true)
        {
            stateMachine.StateUpdate();
            yield return new WaitForSeconds(waitUpdateTime);
        }   //FSM 실행
    }   //FSM Update
    public abstract void AttackToTarget();
    public abstract void MoveToTarget();
    public abstract void Stop();
    public abstract void ReadyAttack();                     //공격전 변수 초기화
    public void ChangeState(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.Idle: { stateMachine.ChangeState(enemyStateData.idle); break; }
            case EnemyState.Chase: { stateMachine.ChangeState(enemyStateData.chase); break; }
            case EnemyState.Attack: { stateMachine.ChangeState(enemyStateData.attackBasic); break; }
            case EnemyState.Damaged: { stateMachine.ChangeState(enemyStateData.damaged); break; }
            case EnemyState.Die: { stateMachine.ChangeState(enemyStateData.die); break; }
        }
    }   //StateMachine 상태 전환 호출
    public abstract void SetDamaged();
    public void Die()
    {
        anim.SetTrigger("Die");
        GameManager.Instance.GetPlayer().InteractionType(QuestAddValueType.Kill);
        GameManager.Instance.GetPlayer().AddExe(data.GetExe);
        DieStatus = true;
        Invoke("SetDisable", 5f);
    }
    #endregion

    //FSM bool 함수
    #region FSMcheck
    public abstract bool SearchRangeCheck();        //플레이어가 탐색 범위 안에 있을때 true
    public abstract bool AttackRangeCheck();        //플레이어가 공격 범위 안에 있을때 true
    public abstract bool AttackDurationCheck();     //공격속도 체크
    public abstract bool HitDelayCheck();           //공격 딜레이
    #endregion

    protected abstract void Attack();       //플레이어 공격
    public void Damaged(int damage)
    {
        if (DieStatus) return;

        //죽을때 처리할것
        hp = Mathf.Max(hp - damage, 0);                         //데미지 계산

        //몬스터 타입이 보스일 때 체력바 설정
        if (monster == Monster.Monster_Boss)
        { GameManager.Instance.GetUIManager().GetEnemyUI.HealthBar.SetHp(hp, data.HP); }
        else  //몬스터 타입의 체력바 일 경우
        { 
            healthBar.StartHitCoroutine();
            healthBar.UpdateUI(hp);           //체력바 값 설정
        }
        
        GameManager.Instance.CallDamageTextPool(damage, this.transform.position);   //데미지 텍스트 표시

        if (hp < 1)
        {
            if (monster != Monster.Monster_Boss) { audioSource.PlayOneShot(soundData.GetAudioClip(SFXEnemySoundType.Damaged)); }
            ChangeState(EnemyState.Die);
        }
        else
        {
            ChangeState(EnemyState.Damaged);    //상태 전환 데미지를 입을 때
        }
    }           //적의 맞았을때 처리
    public void SetActive(Vector3 SpawnPos, EnemyHealthBar healthBar, Rigidbody targetRigid)
    {
        hp = data.HP;                                       //체력 초기화
        this.targetRigid = targetRigid;
        this.transform.position = SpawnPos;                 //스폰 위치로 변경
        LookRotationToTarget();                             //플레이어 방향으로 바라보기

        this.gameObject.SetActive(true);                    //오브젝트 활성화
        agent.enabled = true;
        stateMachine.SetState(enemyStateData.idle);         //idle State로 설정
        //체력바가 없을때, 보스 몬스터 타입이 아닐 때
        if (healthBar != null && monster != Monster.Monster_Boss)
        {
            this.healthBar = healthBar;   //체력바
            this.healthBar.SetData(SpawnPos, this.gameObject.transform, data.HP);
            coroutineFSM = StartCoroutine(OnUpdate());          //코루틴 활성화
        }
        else
        {
            anim.SetTrigger("Scream");
            GameManager.Instance.GetUIManager().GetEnemyUI.HealthBar.SetBossUI(hp, data.HP);
            coroutineFSM = StartCoroutine(OnUpdate(3.0f));     //3초지연 후 코루틴 활성화
        }
    }   //스폰시 실행할 함수
    public void SetDisable()
    {
        if (monster == Monster.Monster_Boss) 
        {
            GameManager.Instance.GetTeleporter.SetActive(this.gameObject.transform.position);
            GameManager.Instance.GetUIManager().GetEnemyUI.HealthBar.SetActive(false); //보스 체력바 비활성화
        }
        else { healthBar.ReturnPool(); }
        Stop();                             //멈춤처리
        StopCoroutine(OnUpdate());          //코루틴 정지
        coroutineFSM = null;
        StopCoroutine();                    //코루틴 있을경우 멈춤처리
        stateMachine.DeleteState();         //State 삭제
        agent.enabled = false;
        this.gameObject.SetActive(false);   //비활성화 처리

        GameManager.Instance.GetEnemyPool.ReturnPool(monster, this);
    }   //오브젝트 비활성화 시 실행
    protected abstract void StopCoroutine();
    protected bool TargetInRange(float sqrRange)
    {
        float dir = Vector3.SqrMagnitude(targetRigid.position - transform.position);

        if (dir < sqrRange)
        { return true; }
        return false;
    }   //타겟이 radius 범위 안에 있을때 true 리턴
    protected bool TargetInAngle(float angle)
    {
        if (!targetRigid) return false; //타겟이 없을때 계산이 불가능하므로 false 리턴
        Vector3 dir = (targetRigid.position - transform.position).normalized;
        if (Vector3.Angle(transform.forward, dir) < angle) { return true; }
        return false;
    }   //타겟이 angle 각도 안에 있을때 true 리턴
    protected bool LookRotationToTarget()
    {
        Vector3 dir = (targetRigid.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(dir);
        return true;
    }   //플레이어 방향으로 바라보기
}

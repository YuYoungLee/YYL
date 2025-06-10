using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public enum EEnemyType
{
    Monster,    //일반 몬스터
    Boss,       //보스
}

public class Enemy : FSMAble, IDamageable
{
    public override event System.Action ReturnPool;      //풀로 돌아가기
    [SerializeField] private EEnemyType meEnemyType;     //몬스터 타입
    [SerializeField] private int miKey = 0;     //몬스터 키
    private EnemyAnim mAnim = null;             //에니매이션
    private Rigidbody mTargetRigid = null;      //타겟의 rigidbody
    private List<Skill> mSkill;         //스킬
    private Stat mStat;                 //몬스터 스텟
    private EventManager mEventMgr;     //이벤트 매니저

    private Coroutine fsmCoroutine;       //fsm 코루틴
    private NavMeshAgent mNAVAgent = null;
    private readonly int miTargetLayerMask = (1 << 9);     //타겟 레이어마스크

    [SerializeField] private Vector3 mHeight;  //에너미의 높이
    private Vector3 mHalfHeight;               //절반크기 높이
    private float mfAtkDelay = 0.0f;           //공격 딜레이
    private Vector3 mStartPos = Vector3.zero;  //시작위치
    private HPBar mBarSlider = null;    //HP 슬라이더

    private bool mbIsDie = false;

    private FieldSpawnPoint fieldSpawnPoint;

    public EEnemyType GetEnemyType => meEnemyType;     //에너미 타입


    //private void OnDrawGizmos()
    //{
    //    Handles.color = Color.red;
    //    Handles.DrawWireArc(mTransform.position, Vector3.up, Vector3.right, 360, 6);

    //    Handles.color = Color.yellow;
    //    Handles.DrawWireArc(mTransform.position, Vector3.up, Vector3.right, 360, 8);

    //    Handles.color = Color.blue;
    //    Handles.DrawWireArc(mTransform.position, Vector3.up, Vector3.right, 360, 10);
    //}   //기즈모

    public override void Initialize()
    {
        base.Initialize();  //fsm Init

        if(mEventMgr == null)
        {
            mEventMgr = EventManager.Instance;
        }

        //트렌스폼
        if (mTransform == null)
        {
            mTransform = GetComponent<Transform>();
        }

        //에니메이션
        if (mAnim == null)
        {
            mAnim = new EnemyAnim();     //에너미 에니매
            mAnim.SetAnim = GetComponent<Animator>();    //에니매이션 설정
        }

        //스텟
        if(mStat == null)
        {
            mStat = new Stat();
            mStat.Initialize(miKey);
        }
        
        if(mNAVAgent == null)
        {
            mNAVAgent = GetComponent<NavMeshAgent>();    //Nav메시
            mNAVAgent.enabled = false;
        }

        //스킬
        if (mSkill == null)
        {
            mSkill = new List<Skill>();     //스킬 리스트 생성
            DataManager dataMgr = GameManager.Instance.GetDataMgr();    //데이터 메니저 임시변수

            foreach (int iSkillKey in dataMgr.GetEnemyData(miKey).SkillKey)
            {
                Skill skill = new Skill();    //스킬담을 임시변수
                skill.SetData(iSkillKey, miTargetLayerMask, mStat, mTransform);
                mSkill.Add(skill);
            }
        }

        mHalfHeight = mHeight * 0.5f;
        gameObject.SetActive(false);
    }
    public void Active(Vector3 startPos)
    {
        fieldSpawnPoint = null;
        mBarSlider = null;
        GameManager gameMgr = GameManager.Instance;     //게임 메니저
        mfAtkDelay = 0.0f;      //공격 딜레이 초기화
        mStartPos = startPos;   //시작 위치 데이터 저장
        transform.position = startPos;    //시작위치
        mStat.ResetData();      //스텟 초기화
        gameObject.SetActive(true);     //활성화

        PlayFSM();    //FSM 플레이
        mAnim.Reset();          //에니메이션 초기화
        mFSM.ChangeState(FSMState.Idle);    //StateIdle 전환
        mTransform.LookAt(GameManager.Instance.GetPlayer.GetPos);    //타겟 방향 회전
        mbIsDie = false;     //사망처리 초기화

        //HPBar
        TrackingBarSlider hpSlider;
        switch (meEnemyType)
        {
            case EEnemyType.Monster:
                //트래킹 HP 슬라이더
                hpSlider = UIManager.Instance.GetUIObjectPool.GetHp() as TrackingBarSlider;
                hpSlider.SetSlider(mTransform, mStat.HP, mStat.HP, mHeight);
                mBarSlider = hpSlider;
                break;
            case EEnemyType.Boss:
                mBarSlider = (UIManager.Instance.GetGUI(EGUI.Stage) as StagePanel).GetBossHPSlider;
                mBarSlider.SetSlider(mStat.HP, mStat.HP);
                mBarSlider.SetActive(true);
                break;
        }

        //필드 스폰포인트 있을때(마을 에너미 스폰시 설정)
        SpawnPointManager spawnPointManager;
        if(gameMgr.GetFieldSpawnPoint(out spawnPointManager))
        {
            spawnPointManager.GetNearSpawnPoint(startPos, out fieldSpawnPoint);     //스폰 포인트 가져오기
        }
    }   //활성화
    public override void UnActive()
    {
        if(fieldSpawnPoint != null)
        {
            fieldSpawnPoint.SumEnemyCount();
        }

        //FSM 멈춤처리
        StopFSM();

        //스킬 파티클 멈춤처리
        for (int i = 0; i < mSkill.Count; ++i)
        {
            StopCoroutine(mSkill[i].SkillCoroutine());
            mSkill[i].StopParticle();
        }

        //HP슬라이더 리턴
        if(mBarSlider != null)
        {
            mBarSlider.StopAnim();      //애니메이션 멈춤 처리

            switch (meEnemyType)
            {
                case EEnemyType.Monster:
                    UIManager.Instance.GetUIObjectPool.ReturnPool(mBarSlider);      //UI 풀로 돌아가기
                    break;
                case EEnemyType.Boss:
                    mBarSlider.SetActive(false);
                    break;
                default:
                    break;
            }
        }

        mBarSlider = null;

        ReturnPool?.Invoke();   //풀로 리턴
    }   //비활성화
    private void PlayFSM()
    {
        mNAVAgent.enabled = true;
        fsmCoroutine = StartCoroutine(FSMCoroutine());
    }   //에너미 작동
    private void StopFSM()
    {
        //코루틴이 작동중일 때 멈추기
        if (fsmCoroutine != null) 
        { 
            StopCoroutine(FSMCoroutine());
            mNAVAgent.enabled = false;
        }
        fsmCoroutine = null;
    }   //에너미 멈춤
    //FSMable 상속 함수
    #region FSMstate
    protected IEnumerator FSMCoroutine()
    {
        while (true)
        {
            mFSM.UpdateState();
            yield return new WaitForSeconds(0.5f);
        }
    }    //FSM 코루틴
    public override void IdleStateEnter()
    {
        mTargetRigid = null;  //타겟이 없을때 처리
        Stop();
    }   //아이들 상태 진입 시
    public override void IdleState() 
    {
        if (IsChangeState(FSMState.Return))
        {
            mFSM.ChangeState(FSMState.Return);    //Return 상태 전환
            return;
        }
        else if (IsChangeState(FSMState.Move))
        {
            mFSM.ChangeState(FSMState.Move);      //Move 상태 전환
            return;
        }
    }       //아이들 상태 동작
    public override void MoveEnter()
    {
    }
    public override void MoveState() 
    {
        //Attack 상태 전환
        if (IsChangeState(FSMState.Attack))
        {
            mFSM.ChangeState(FSMState.Attack);   //Attck 상태 전환
            return;
        }
        else if (IsChangeState(FSMState.Idle))
        {
            mFSM.ChangeState(FSMState.Idle);     //Idle 상태 전환
            return;
        }

        Move(mTargetRigid.transform.position);
    }       //움직임 상태 동작
    public override void MoveExit()
    {
        Stop();
    }         //움직임 상태 탈출 시
    public override void AttackEnter()
    {}      //공격 상태 진입 시
    public override void AttackState() 
    {
        //공격 딜레이 상태가 아니면 공격 진행
        mfAtkDelay -= 1.0f;       //딜레이 값 감소
        if (mfAtkDelay <= 0.0f)
        {
            mfAtkDelay = 4.0f;    //딜레이 값 초기화
            Attack();
        }
        else if (IsChangeState(FSMState.Move))      //Move상태 전환
        {
            mFSM.ChangeState(FSMState.Move);
        }
    }     //공격 상태 동작
    public override void AttackExit()
    {
        Stop();
    }       //공격 상태 탈출 시
    public override void DamagedStateEnter()
    {
        mAnim.Damaged();
    }
    public override void DamagedState()
    {
        //Attack 상태 전환
        if (IsChangeState(FSMState.Attack))
        {
            mFSM.ChangeState(FSMState.Attack);   //Attck 상태 전환
            return;
        }
        else if (IsChangeState(FSMState.Idle))
        {
            mFSM.ChangeState(FSMState.Idle);     //Idle 상태 전환
            return;
        }
    }   //아이들 상태 동작
    public override void DamagedExit()
    {
        Stop();     //멈춤처리
    }
    public override void DeadStateEnter()
    {
        StopFSM();
        mbIsDie = true;

        //스테이지 메니저 있을 경우
        StageManager stageMgr = null;
        if (GameManager.Instance.GetStageMgr(out stageMgr))
        {
            stageMgr.SumEnemyCount();
        }

        mEventMgr.AddValueEvent(EEventMessage.DeadEnemy, miKey);     //적 사망시 경험치 증가 이벤트
        mEventMgr.CreateEvent(EEventMessage.CreateTableRandomItem, mTransform.position, miKey);   //랜덤 아이탬 드롭
        mAnim.Die();

        if(meEnemyType == EEnemyType.Monster)
        {
            StartCoroutine(DeadCoroutine());
        }
        else
        {
            Invoke("UnActive", 10.0f);   //10초 후 비활성화 처리
        }
    }
    public override void ReturnStateEnter()
    {
        Move(mStartPos);
    }   //돌아가는 상태 진입 시
    public override void ReturnState()
    {
        //시작위치 근처에 온다면 IdleState 전환
        if(IsDistLong(mTransform.position, mStartPos, 3.0f))
        { 
            mFSM.ChangeState(FSMState.Idle); 
        }
    }       //돌아가는 상태 동작
    public override void ReturnExit()
    {
        Stop();
    }       //돌아가는 상태 탈출 시
    #endregion

    //State 에서 사용할 함수
    #region FSMLogic
    /// <summary>
    /// targetPos : 이동할 위치
    /// </summary>
    protected void Move(Vector3 targetPos)
    {
        mNAVAgent.isStopped = false;                                //agent 움직임 처리
        mNAVAgent.SetDestination(targetPos);                        //agent 타겟의 Path 찾기
        mAnim.Move(1.0f);    //움직이는 에니메이션
    }

    protected void Stop()
    {
        mAnim.Stop();
        mNAVAgent.isStopped = true;
        mNAVAgent.ResetPath();          //길 정보 삭제
    }   //멈춤 동작

    protected void Attack()
    {
        int iSkillIdx = -1;
        float fSkillDistance = -1.0f;
        float targetDist = Vector3.Distance(mTransform.position, mTargetRigid.position);
        DataManager dataMgr = GameManager.Instance.GetDataMgr();

        for (int i = 0; i < mSkill.Count; ++i)
        {
            //쿨타임 아닌지 체크 && 거리가 더먼 스킬인지 && 플레이어 사정거리 안에 있는지
            float fSkillEndDist = dataMgr.GetSkillData(mSkill[i].SkillKey).EndDist;
            if (!mSkill[i].IsCoolTime &&
                fSkillEndDist > fSkillDistance &&
                fSkillEndDist > targetDist)
            {
                iSkillIdx = i;
                fSkillDistance = fSkillEndDist;
            }
        }

        //조건에 부합하는 스킬 있을때 && 스킬 플레이 될 때
        if(iSkillIdx != -1 &&
            mSkill[iSkillIdx].Play(mTransform.position, mTransform.forward))
        {
            mTransform.rotation = Quaternion.LookRotation((mTargetRigid.transform.position - mTransform.position).normalized);
            StartCoroutine(mSkill[iSkillIdx].SkillCoroutine());
            mAnim.Attack();      //공격 에니매이션
        }
    }   //공격 시 
    #endregion

    private bool IsChangeState(FSMState state)
    {
        switch (state)
        {
            case FSMState.Idle:
                { return !FindTarget(10.0f); }      //멀리 있을 때     
            case FSMState.Move:
                { return FindTarget(9.0f); }        //안에 있을 때
            case FSMState.Attack:
                { return FindTarget(5.0f); }        //안에 있을 때
            case FSMState.Return:
                { return !IsDistLong(mTransform.position, mStartPos, 15.0f); }      //멀리 있는지
            case FSMState.Damaged:
                return meEnemyType == EEnemyType.Monster;    //몬스터 타입일 때만
            default:
                Debug.LogError("ChangeStateCheck state is Default");
                break;
        }
        return false;
    }   //State별 체크

    /// <summary>
    /// fRange : 타겟 찾을 범위
    /// </summary>
    private bool FindTarget(float fRange)
    {
        Collider[] targetCollider = Physics.OverlapSphere(mTransform.position, fRange, miTargetLayerMask);

        //타겟이 들어 있다면
        if(targetCollider != null && targetCollider.Length > 0)
        {
            mTargetRigid = targetCollider[0].GetComponent<Rigidbody>();
            return true;
        }

        return false;
    }   //타겟이 있을 때 true 리턴

    /// <summary>
    /// aPosition : 위치 a
    /// bPosition : 위치 b
    /// dist : a~b 지정할 길이
    /// </summary>
    private bool IsDistLong(Vector3 aPosition, Vector3 bPosition, float dist)
    {
        //dist 가 a~b의 사이 의 길이보다 크다면
        if(dist >= Vector3.Distance(aPosition, bPosition))
        { 
            return true; 
        }

        return false;
    }   //두지점의 길이보다 dist가 더 클 때 true

    /// <summary>
    /// iDamage : 데미지
    /// dir : 맞은 방향
    /// </summary>
    public void Damaged(int iDamage, Vector3 dir)
    {
        //죽었을 때 리턴
        if (mbIsDie)
        {
            return;
        }

        Vector3 hitPos = mTransform.position - (dir * 2.0f);    //앞에서 표시할 위치 = 몬스터 위치 - 맞은방향 * 2.0f
        float randomDist = Random.Range(-0.2f, 0.2f);    //랜덤한 추가할 길이

        //힛 파티클
        hitPos.x += randomDist;    //랜덤 가로
        FloatingText floatingText = GameManager.Instance.GetObjPoolManager().GetObject(3003) as FloatingText;
        floatingText.Play(hitPos + mHeight, iDamage);

        hitPos.y += randomDist;    //랜덤 높이
        Particle hitParticle = GameManager.Instance.GetObjPoolManager().GetObject(4002) as Particle;
        hitParticle.Active(hitPos + mHalfHeight, mTransform.forward);     //에너미 힛 파티클

        //데미지를 입고 체력이 없다면
        if (mStat.Damaged(iDamage))
        {
            mFSM.ChangeState(FSMState.Dead);    //사망 상태 전환
        }
        else if (meEnemyType == EEnemyType.Monster)     //몬스터 타입 일경우
        {
            mFSM.ChangeState(FSMState.Damaged);     //Hit 상태 전환
        }

        if(mBarSlider != null)
        {
            mBarSlider.SliderAnim(mStat.HP);        //HPSlider
        }
    }   //데미지

    private IEnumerator DeadCoroutine()
    {
        //에너미 높이만큼 밑으로 이동
        for (float fHeight = 0; fHeight < mHeight.y; fHeight += 0.1f)
        {
            Vector3 enemyPos = mTransform.position;
            enemyPos.y -= 0.1f;
            mTransform.position = enemyPos;
            yield return new WaitForSeconds(0.1f);
        }

        Invoke("UnActive", 4.0f);   //4초 후 비활성화 처리
    }

    //타임라인 함수
    #region Timeline
    public void StartBossTimeline(Vector3 startPos)
    {
        GameManager gameMgr = GameManager.Instance;     //게임 메니저
        fieldSpawnPoint = null;
        mBarSlider = null;
        mfAtkDelay = 0.0f;      //공격 딜레이 초기화
        mStartPos = startPos;   //시작 위치 데이터 저장
        transform.position = startPos;    //시작위치

        mTransform.LookAt(gameMgr.GetPlayer.GetPos);    //플레이어 방향 바라보기
        gameObject.SetActive(true);     //활성화

        mAnim.Attack();     //공격 에니메이션 진행
    }

    public void EndBossTimeLine()
    {
        //보스 HP바 활성화
        mBarSlider = (UIManager.Instance.GetGUI(EGUI.Stage) as StagePanel).GetBossHPSlider;
        mBarSlider.SetSlider(mStat.HP, mStat.HP);
        mBarSlider.SetActive(true);

        mStat.ResetData();      //스텟 초기화
        mbIsDie = false;     //사망처리 초기화
        PlayFSM();    //FSM 플레이
        mAnim.Reset();          //에니메이션 초기화
        mFSM.ChangeState(FSMState.Attack);    //AttackState 전환
        mTransform.LookAt(GameManager.Instance.GetPlayer.GetPos);    //타겟 방향 회전

    }
    #endregion
}

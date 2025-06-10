using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : FSMAble, IDamageable
{
    public override event System.Action ReturnPool;

    [SerializeField] private int miPlayerKey = 0;     //플레이어 키값
    private Rigidbody rigid = null;         //플레이어 rigidbody

    [Header("Slope")]
    private RaycastHit slopeHit;

    private readonly float mfSlopeAngle = 45.0f;
    private readonly int miGroundLayerMask = (1 << 8); //레이어 마스크

    [Header("CameraGeometry")]
    [SerializeField] private Geometry geometry;     //카메라 타겟 지오메트리

    [Header("Anim")]
    private PlayerAnim mAnim = null;     //플레이어 에니메이션

    [Header("Stat")]
    private PlayerStat mStat = null;    //플레이어 스텟

    [Header("Sound")]
    private AudioSource mPlayerSFX = null;

    [Header("Movement")]
    private Vector2 moveDir;        //방향
    private Vector2 rotateDir;      //회전
    private Vector3 velocity;       //속력
    private float mfYaw;              //y축 회전
    private float mfPitch;            //x축 회전
    private const float maxRotateDir = 1.0f;
    private Vector2 mMouseSensivity;   //마우스 감도
    public bool isGround = true;                       //땅 위에 있는 경우 true
    private readonly float mfGravity = 9.81f;           //중력값 9.81f
    private readonly int miTargetLayerMask = (1 << 7);  //레이어 마스크
    private readonly float mfDownForce = 5.0f;          //아래로 내려가는 힘 (중력에 * 할 값)
    private float jumpForce;        //점프 힘

    private Vector3 mHeight;       //플레이어 높이값
    private float speed;           //속도 test 값

    private List<Skill> mSkill;     //스킬
    private Skill mAttack;    //기본공격
    private Coroutine mAttackCoroutine;    //공격 코루틴
    private Coroutine mInputStopMoveCoroutine;      //인풋 멈추기

    private bool mbIsAttack;
    private bool mbIsDie;

    public bool mbGizmoDraw = false;
    public float mfGizmoAngle = 0.0f;
    public float GizmoDist;
    public Vector3 forwardGizmo;


    [SerializeField] private PlayerPivot mPivot;     //피봇    

    private float mfRotateYDir = 1;    //Y축 회전 방향

    [Header("Interaction")]
    private List<IInteractAble> mInteractOBJ;   //상호작용 오브젝트

    public int PlayerKey => miPlayerKey;    //플레이어 키값

    public Vector3 GetPos
    {
        get { return rigid.position; }
    }    //플레이어 위치

    public PlayerStat PlayerStat => mStat;      //플레이어 스텟

    public bool IsInteraction => mInteractOBJ.Count > 0;    //상호작용

    public void SetActive(bool bActiveStatu) => this.gameObject.SetActive(bActiveStatu);

    private void FixedUpdate()
    {
        mFSM.UpdateState();
        Gravity();
        if (isGround)
        {
            Slope();
            Move();
        }
    }

    private void Update()
    {
        Rotate();   //회전
    }

    public override void Initialize()
    {
        DataManager dataMgr = GameManager.Instance.GetDataMgr();    //데이터 메니저 임시변수

        base.Initialize();
        geometry.Initialize();      //카메라 타겟 지오메트리

        mAnim = new PlayerAnim();    //플레이어 애니메이션
        mAnim.SetAnim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        mTransform = GetComponent<Transform>();

        //Data 값 초기화
        mbIsAttack = false;
        mbIsDie = false;
        velocity = Vector2.zero;
        moveDir = Vector2.zero;
        rotateDir = Vector2.zero;
        mfPitch = mTransform.rotation.z;
        mfYaw = mTransform.rotation.y;
        mMouseSensivity = new Vector2(100.0f, 100.0f);  //마우스 민감도
        jumpForce = 10.0f;       //점프 힘
        speed = 5.0f;           //곱할 속도

        //플레이어 스텟
        if (mStat == null)
        {
            mStat = new PlayerStat();
        }
        mStat.Initialize(miPlayerKey);

        //사운드
        if (mPlayerSFX == null)
        {
            mPlayerSFX = GetComponent<AudioSource>();
        }

        //Skill
        if (mSkill == null)
        {
            mSkill = new List<Skill>();     //스킬 리스트 생성


            foreach (int iSkillKey in dataMgr.GetPlayerData(miPlayerKey).SkillKeyList)
            {
                Skill skill = new Skill();    //스킬담을 임시변수
                skill.SetData(iSkillKey, miTargetLayerMask, mStat, mTransform);
                mSkill.Add(skill);
            }
        }

        //플레이어 기본공격
        if (mAttack == null)
        {
            mAttack = new Skill();
            mAttack.SetData(9, miTargetLayerMask, mStat, mTransform);
        }

        //상호작용 오브젝트
        if (mInteractOBJ == null)
        {
            mInteractOBJ = new List<IInteractAble>();
        }

        EventManager.Instance.Event(EEventMessage.SetMoneyUI);
    }

    public override void UnActive()
    {
        ReturnPool?.Invoke();
    }

    //플레이어 인풋
    #region Input
    public void AttackStart(InputAction.CallbackContext context)
    {
        //땅 위에 있을 때 공격 진행 아닐때만
        if (isGround && !mbIsAttack)
        {
            mFSM.ChangeState(FSMState.Attack);
        }
    }

    public void AttackStop(InputAction.CallbackContext context)
    {
        mFSM.ChangeState(FSMState.Idle);
    }

    public void InputMove(InputAction.CallbackContext context)
    {
        if (mbIsAttack)
        {
            velocity = Vector3.zero;
            return;
        }

        //땅에 있을때
        if (isGround)
        {
            moveDir = context.ReadValue<Vector2>(); //움직이는 방향
            mFSM.ChangeState(FSMState.Move);
        }
        else
        {
            moveDir = Vector2.zero;     //멈춰야 할 경우
        }

        velocity = rigid.transform.forward * moveDir.y + rigid.transform.right * moveDir.x; //방향값 더하기
        velocity = velocity.normalized * speed;             //속력 = 방향 * 속도;
    }   //플레이어 움직임 인풋 처리

    public void InputRotate(InputAction.CallbackContext context)
    {
        rotateDir = context.ReadValue<Vector2>();
        rotateDir *= 0.1f;

        //방향 1, -1, 0 값으로 설정
        rotateDir.x = Mathf.Clamp(rotateDir.x, -maxRotateDir, maxRotateDir);
        rotateDir.y = Mathf.Clamp(rotateDir.y, -maxRotateDir, maxRotateDir);
        rotateDir.y *= mfRotateYDir;     //상하 반전 시 1, 정방향 시 -1값 Y축 곱하기

        //회전 할 때 방향 전환
        velocity = rigid.transform.forward * moveDir.y + rigid.transform.right * moveDir.x; //방향값 더하기
        velocity = velocity.normalized * speed;             //속력 = 방향 * 속도;
    }   //회전 인풋

    public void InputStopRotate(InputAction.CallbackContext context)
    {
        rotateDir = Vector2.zero;
    }

    public void InputStopMove(InputAction.CallbackContext context)
    {
        moveDir = Vector2.zero;
        velocity.x = 0.0f;
        velocity.z = 0.0f;
        if (isGround)
        {
            velocity.y = 0.0f; //땅에 있을 때는 y값 0으로
        }
        mFSM.ChangeState(FSMState.Idle);
    }   //움직임방향 초기화

    /// <summary>
    /// iSkillKey : 스킬 키값
    /// </summary>
    public void InputSkill(int iSkillKey)
    {
        //기본 공격을 하고 있지 않을때, 땅 위에 있을 때 리턴
        if (mbIsAttack || !isGround)
        {
            return;
        }

        //스킬 사용 했을 때
        for (int i = 0; i < mSkill.Count; ++i)
        {
            //키값이 일치한 스킬이 있을 때, 스킬 사용 할 수 있을 때
            if (iSkillKey == mSkill[i].SkillKey &&
                mSkill[i].Play(mTransform.position, mTransform.forward))
            {
                DataManager dataMgr = GameManager.Instance.GetDataMgr();
                float fStopTime = 0.0f;

                //밀리 스킬 지연시간 추가
                switch (dataMgr.GetSkillData(iSkillKey).AttackType)
                {
                    case EAttackType.Melee:
                        fStopTime =
                            dataMgr.GetSkillData(iSkillKey).StartTime +
                            dataMgr.GetSkillData(iSkillKey).DurationCount * dataMgr.GetSkillData(iSkillKey).RepeatTime;    //기본 스킬 지연시간
                        break;
                    default:
                        fStopTime = dataMgr.GetSkillData(iSkillKey).StartTime;
                        break;
                }
                //Invoke("Gizemo", fStopTime);

                mInputStopMoveCoroutine = StartCoroutine(InputStopMoveCoroutine(fStopTime));
                StartCoroutine(mSkill[i].SkillCoroutine());    //코루틴 실행
                mAnim.Attack(mSkill[i].SkillKey);    //애니메이션 실행
            }
        }
    }   //스킬 인풋

    public void InputJump(InputAction.CallbackContext context)
    {
        //땅을 밟고 있을 때 점프 사용 가능
        if (!isGround)
        {
            return;
        }
        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);    //순간적인 힘 적용
        PlayOneShot("PlayerJump");
    }   //점프 인풋

    public void InputInteraction(InputAction.CallbackContext context)
    {
        if (mInteractOBJ.Count > 0)
        {
            IInteractAble interaction = mInteractOBJ[0];
            //제거 해야 할 타입 일 때
            if (mInteractOBJ[0] is DropItem || mInteractOBJ[0] is SceneTeleport)
            {
                mInteractOBJ.RemoveAt(0);
            }

            interaction.Interaction();  //상호작용

            //상호작용 가능한 오브젝트가 없다면, NPC 일 때
            //상호작용 표시 끄기
            if (mInteractOBJ.Count == 0 || mInteractOBJ[0] is NPC)
            {
                (UIManager.Instance.GetGUI(EGUI.Player) as PlayerPanel).InteractionUIObject.SetActive(false);     //끄기
            }
        }
    }   //상호작용 인풋
    #endregion

    //플레이어 움직임 계산
    #region Update
    private void Move()
    {
        rigid.velocity = velocity;
    }   //움직임 계산
    private void Rotate()
    {
        mfYaw += rotateDir.x * mMouseSensivity.x * Time.deltaTime;     //마우스 감도 값만큼 회전
        mfPitch += rotateDir.y * mMouseSensivity.y * Time.deltaTime;

        mfYaw = ClampDgree(mfYaw, -360, 360);     //360 ~ -360회전값 범위 안으로 값 증감
        mfPitch = Mathf.Clamp(mfPitch, -90, 80);     //-90 80도 값 고정

        mTransform.rotation = Quaternion.Euler(0.0f, mfYaw, 0.0f);     //플레이어 Yaw 회전
        geometry.SetRotate(mfPitch, mfYaw);     //카메라 타겟의 지오메트리 회전
    }   //회전 계산
    private void Gravity()
    {
        RaycastHit hit;
        // SphereCast로 땅 확인
        if (Physics.Raycast(mTransform.position + Vector3.up * 0.3f, Vector3.down, out hit, 0.4f, miGroundLayerMask))
        {
            velocity.y = 0.0f;    //땅일 때 y축 속도 0 설정
            isGround = true;
            return;
        }

        // 땅에 닿지 않았을 경우 중력 적용
        rigid.velocity += Vector3.down * mfGravity * mfDownForce * Time.deltaTime;
        isGround = false;

    }//중력 적용
    private void Slope()
    {
        //수평 이 아닐 때 && 오르막기준 각도 45도 아닐 때
        if (Physics.Raycast(mTransform.position + velocity.normalized * 0.3f, Vector3.down, out slopeHit, 0.3f, miGroundLayerMask) &&
            slopeHit.normal != Vector3.up &&
            Vector3.Angle(Vector3.up, slopeHit.normal) < mfSlopeAngle)
        {
            velocity = Vector3.ProjectOnPlane(velocity, slopeHit.normal);   //투영 백터로 이동
        }
    }   //굴곡진 땅 처리
    #endregion

    //FSMable Override 함수들
    #region FSMstate
    public override void IdleStateEnter()
    {
        mAnim.Stop();                //에니메이션 멈춤 설정
    }
    public override void IdleState() { }     //아이들 상태 동작
    public override void DamagedStateEnter()
    {
        PlayOneShot("PlayerDamaged");    //데미지 사운드

        //공격 멈춤상태 일 때만
        if (mInputStopMoveCoroutine != null || mAttackCoroutine != null)
        {
            return;
        }

        mInputStopMoveCoroutine = StartCoroutine(InputStopMoveCoroutine(1.0f));
        mAnim.Damaged();
        //에니메이션 실행
    }
    public override void DamagedState()
    {
    }
    public override void DamagedExit()
    {
    }
    public override void DeadStateEnter()
    {
        GameManager.Instance.GetInputMgr.SetActionMap(EActionMap.NoInput);      //인풋 막기
        mAnim.Dead();
        PlayOneShot("PlayerDead");    //사망 사운드
        //부활패널 열기
        (UIManager.Instance.GetGUI(EGUI.Player) as PlayerPanel).RevivalPlayer.SetActive(true);

    }       //사망 상태 동작
    public override void MoveEnter()
    {
        StopLoop();
        PlayLoop("PlayerWalk");    //움직이기 사운드
    }
    public override void MoveState()
    {
        mAnim.Move(moveDir.x, moveDir.y);
    }       //움직임 상태 동작
    public override void MoveExit()
    {
        StopLoop();
    }
    public override void AttackEnter()
    {
        velocity = Vector3.zero;    //속력 0으로 설정
        mbIsAttack = true;     //공격상태 전환
        //기본공격 시작
        if (mAttackCoroutine == null)
        {
            mAttackCoroutine = StartCoroutine(AttackCoroutine());
        }
    }   //공격 상태 진입 시
    public override void AttackState()
    {
    }     //공격 상태 동작
    public override void AttackExit()
    {
        if (mAttackCoroutine != null)
        {
            StopCoroutine(mAttackCoroutine);
        }
        mbIsAttack = false;
        mAttackCoroutine = null;
    }       //공격 상태 탈출 시
    public override void ReturnStateEnter() { }
    public override void ReturnState() { }
    public override void ReturnExit() { }

    #endregion

    //연산 함수
    #region CarculateMath
    /// <summary>
    /// dir : 방향값
    /// </summary>
    private float ClampDir(float dir)
    {
        if (dir > maxRotateDir) return maxRotateDir;            //0보다 클때
        else if (dir < -maxRotateDir) return -maxRotateDir;     //0보다 작을때
        return dir;
    }   //방향 값을 받으면 1, -1, 0 값으로 리턴

    private float ClampDgree(float value, float min, float max)
    {
        while (value < min)
        {
            value += 360.0f;
        }

        while (value > max)
        {
            value -= 360.0f;
        }
        return value;
    }
    #endregion

    #region Sound
    private void PlayOneShot(string strKey)
    {
        mPlayerSFX.PlayOneShot(SoundManager.Instance.AudioData(strKey));    //사운드 한번 재생
    }   //사운드 한번 재생

    private void PlayLoop(string strKey)
    {
        mPlayerSFX.clip = SoundManager.Instance.AudioData(strKey);
        mPlayerSFX.loop = true;
        mPlayerSFX.Play();
    }   //루프 사운드 재생

    private void StopLoop()
    {
        mPlayerSFX.clip = null;
        mPlayerSFX.loop = false;
        mPlayerSFX.Stop();
    }   //루프 사운드 멈춤 설정
    #endregion

    /// <summary>
    /// iDamage : 데미지
    /// dir : 맞은 방향
    /// </summary>
    public void Damaged(int iDamage, Vector3 dir)
    {
        //사망 시 리턴
        if (mbIsDie)
        {
            return;
        }

        //힛 파티클
        Vector3 hitPos = mTransform.position - (dir * 2.0f);    //앞에서 표시할 위치 = 플레이어 위치 - 맞은방향 * 2.0f
        float randomDist = Random.Range(-0.2f, 0.2f);    //랜덤한 추가할 길이
        hitPos.y += randomDist;    //랜덤 높이
        hitPos.x += randomDist;    //랜덤 가로
        Particle hitParticle = GameManager.Instance.GetObjPoolManager().GetObject(4002) as Particle;
        hitParticle.Active(hitPos + mHeight, mTransform.forward);

        //mStat.Hp
        if (mStat.Damaged(iDamage))
        {
            mbIsDie = true;
            //사망 시 처리
            mFSM.ChangeState(FSMState.Dead);    //에니메이션 사망 처리
        }

        mFSM.ChangeState(FSMState.Damaged);

        PlayerPanel playerPanel = UIManager.Instance.GetGUI(EGUI.Player) as PlayerPanel;
        playerPanel.SliderAnim(EPlayerSlider.HP, (float)mStat.HP);
    }   //데미지 입을때 처리

    //트리거 함수
    #region Trigger
    public void OnTriggerEnter(Collider other)
    {
        //상호작용 가능한 오브젝트 일 때
        if (other.tag == "InteractAble")
        {
            IInteractAble interactOBJ = other.GetComponent<IInteractAble>();    //컴포넌트 받기
            //리스트에 포함되어 있지 않다면
            if (!mInteractOBJ.Contains(interactOBJ))
            {
                mInteractOBJ.Add(interactOBJ);      //삽입
                (UIManager.Instance.GetGUI(EGUI.Player) as PlayerPanel).InteractionUIObject.SetActive(true);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        //상호작용 오브젝트일때
        if (other.tag == "InteractAble")
        {
            IInteractAble interactOBJ = other.GetComponent<IInteractAble>();    //컴포넌트 받기
            //리스트에 포함하고 있으면
            if (mInteractOBJ.Contains(interactOBJ))
            {
                mInteractOBJ.Remove(interactOBJ);      //제거
            }

            //상호작용 가능한 오브젝트가 없을 때
            if (mInteractOBJ.Count == 0)
            {
                (UIManager.Instance.GetGUI(EGUI.Player) as PlayerPanel).InteractionUIObject.SetActive(false);     //끄기
            }
        }
    }
    #endregion

    //플레이어 설정
    #region SetPlayer
    public void SetMove(Vector3 position, float yaw)
    {
        mfYaw = yaw;
        mfPitch = -30.0f;
        mTransform.position = position;
    }   //플레이어 좌표 이동, 바라보기

    public void SetLookAt(Vector3 pos)
    {
        Vector3 dir = (pos - mTransform.position).normalized;
        dir.y = 0f;
        Quaternion rot = Quaternion.LookRotation(dir);
        Vector3 rotate = rot.eulerAngles;
        mfYaw = rotate.y;
    }   //플레이어 방향 바라보기

    public void SetAxisYInverse(bool bAxisYInverseStatu)
    {
        //역방향 1, 정방향 -1
        if (bAxisYInverseStatu)
        {
            mfRotateYDir = 1;
        }
        else
        {
            mfRotateYDir = -1;
        }
    }   //Y축 반전 설정

    public void SetMouseSencivity(float fSensivityX, float fSensivityY)
    {
        mMouseSensivity.x = fSensivityX;
        mMouseSensivity.y = fSensivityY;
    }   //마우스 민감도 설정

    /// <summary>
    /// eStat : 스텟의 종류
    /// iStat : 더할 스텟 값
    /// </summary>
    public void AddStat(EStatDataType eStat, int iStat)
    {
        mStat.Add(eStat, iStat);
    }   //플레이어 스텟 증가

    /// <summary>
    /// eStat : 스텟의 종류
    /// iStat : 뺄 스텟 값
    /// </summary>
    public bool SumStat(EStatDataType eType, int iStat)
    {
        return mStat.Sum(eType, iStat);
    }   //플레이어 스텟 감소

    /// <summary>
    /// iSkillKey : 스킬 키값
    /// iSkillLevel : 적용할 스킬 레벨
    /// </summary>
    public void SetSkillPower(int iSkillKey, int iSkillLevel)
    {
        for (int i = 0; i < mSkill.Count; ++i)
        {
            //스킬 키가 같으면 파워 증가
            if (mSkill[i].SkillKey == iSkillKey)
            {
                mSkill[i].SetSkillPower(iSkillLevel);
                break;
            }
        }
    }   //스킬파워 증가

    /// <summary>
    /// iSkillKey : 스킬 키값
    /// quickSlot : 퀵슬롯
    /// </summary>
    public void SetTargetQuickSlot(int iSkillKey, QuickSlot quickSlot)
    {
        for (int i = 0; i < mSkill.Count; ++i)
        {
            //스킬 키가 같다면 퀵슬롯 등록
            if (mSkill[i].SkillKey == iSkillKey)
            {
                if (quickSlot == null)
                {
                    mSkill[i].ResetQuickSlot();
                }
                else
                {
                    mSkill[i].SetTargetQuickSlot(quickSlot);
                }
            }
        }
    }   //퀵슬롯 설정
    #endregion

    public void LoadData(SaveData saveData)
    {
        mInteractOBJ.Clear();   //상호작용 초기화
        (UIManager.Instance.GetGUI(EGUI.Player) as PlayerPanel).InteractionUIObject.SetActive(false);
        mStat.ResetData(miPlayerKey);   //스텟리셋
        mStat.LoadData(saveData);     //데이터 불러오기
    }   //플레이어 데이터 로드

    private IEnumerator AttackCoroutine()
    {
        while (true)
        {
            //공격 재생
            if (mAttack.Play(mTransform.position, mTransform.forward))
            {
                StartCoroutine(mAttack.SkillCoroutine());       //코루틴
                mAnim.Attack(mAttack.SkillKey);     //공격
                PlayOneShot("SwordAttack");     //사운드 재생
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    private IEnumerator InputStopMoveCoroutine(float fTime)
    {
        mbIsAttack = true;    //공격상태 전환
        velocity = Vector3.zero;    //속력 0으로 설정
        yield return new WaitForSeconds(fTime);     //시간지연
        mbIsAttack = false;     //공격상태 해제

        mInputStopMoveCoroutine = null;
    }   //fTime 만큼 플레이어 무브 인풋 멈추기

    public void ResetPlayer()
    {
        mInteractOBJ.Clear();   //상호작용 비우기
        (UIManager.Instance.GetGUI(EGUI.Player) as PlayerPanel).InteractionUIObject.SetActive(false);   //상호작용 표시 끄기

        if (mAttackCoroutine != null)
        {
            StopCoroutine(mAttackCoroutine);    //공격
            mAttackCoroutine = null;
        }

        if (mInputStopMoveCoroutine != null)
        {
            StopCoroutine(mInputStopMoveCoroutine);     //지연시간
            mInputStopMoveCoroutine = null;
        }

        for (int i = 0; i < mSkill.Count; ++i)
        {
            mSkill[i].StopParticle();
        }

        isGround = true;
        mbIsAttack = false;

    }   //플레이어 데이터 리셋

    public void RevivalPlayer()
    {
        ResetPlayer();    //리셋
        mFSM.ChangeState(FSMState.Idle);    //Idle
        //체력
        AddStat(EStatDataType.HP, mStat.MaxHP);
        AddStat(EStatDataType.MP, mStat.MaxMP);
        mAnim.Reset();
        mbIsDie = false;
    }   //플레이어 부활 시
}

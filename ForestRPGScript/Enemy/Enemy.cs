using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public enum EEnemyType
{
    Monster,    //�Ϲ� ����
    Boss,       //����
}

public class Enemy : FSMAble, IDamageable
{
    public override event System.Action ReturnPool;      //Ǯ�� ���ư���
    [SerializeField] private EEnemyType meEnemyType;     //���� Ÿ��
    [SerializeField] private int miKey = 0;     //���� Ű
    private EnemyAnim mAnim = null;             //���ϸ��̼�
    private Rigidbody mTargetRigid = null;      //Ÿ���� rigidbody
    private List<Skill> mSkill;         //��ų
    private Stat mStat;                 //���� ����
    private EventManager mEventMgr;     //�̺�Ʈ �Ŵ���

    private Coroutine fsmCoroutine;       //fsm �ڷ�ƾ
    private NavMeshAgent mNAVAgent = null;
    private readonly int miTargetLayerMask = (1 << 9);     //Ÿ�� ���̾��ũ

    [SerializeField] private Vector3 mHeight;  //���ʹ��� ����
    private Vector3 mHalfHeight;               //����ũ�� ����
    private float mfAtkDelay = 0.0f;           //���� ������
    private Vector3 mStartPos = Vector3.zero;  //������ġ
    private HPBar mBarSlider = null;    //HP �����̴�

    private bool mbIsDie = false;

    private FieldSpawnPoint fieldSpawnPoint;

    public EEnemyType GetEnemyType => meEnemyType;     //���ʹ� Ÿ��


    //private void OnDrawGizmos()
    //{
    //    Handles.color = Color.red;
    //    Handles.DrawWireArc(mTransform.position, Vector3.up, Vector3.right, 360, 6);

    //    Handles.color = Color.yellow;
    //    Handles.DrawWireArc(mTransform.position, Vector3.up, Vector3.right, 360, 8);

    //    Handles.color = Color.blue;
    //    Handles.DrawWireArc(mTransform.position, Vector3.up, Vector3.right, 360, 10);
    //}   //�����

    public override void Initialize()
    {
        base.Initialize();  //fsm Init

        if(mEventMgr == null)
        {
            mEventMgr = EventManager.Instance;
        }

        //Ʈ������
        if (mTransform == null)
        {
            mTransform = GetComponent<Transform>();
        }

        //���ϸ��̼�
        if (mAnim == null)
        {
            mAnim = new EnemyAnim();     //���ʹ� ���ϸ�
            mAnim.SetAnim = GetComponent<Animator>();    //���ϸ��̼� ����
        }

        //����
        if(mStat == null)
        {
            mStat = new Stat();
            mStat.Initialize(miKey);
        }
        
        if(mNAVAgent == null)
        {
            mNAVAgent = GetComponent<NavMeshAgent>();    //Nav�޽�
            mNAVAgent.enabled = false;
        }

        //��ų
        if (mSkill == null)
        {
            mSkill = new List<Skill>();     //��ų ����Ʈ ����
            DataManager dataMgr = GameManager.Instance.GetDataMgr();    //������ �޴��� �ӽú���

            foreach (int iSkillKey in dataMgr.GetEnemyData(miKey).SkillKey)
            {
                Skill skill = new Skill();    //��ų���� �ӽú���
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
        GameManager gameMgr = GameManager.Instance;     //���� �޴���
        mfAtkDelay = 0.0f;      //���� ������ �ʱ�ȭ
        mStartPos = startPos;   //���� ��ġ ������ ����
        transform.position = startPos;    //������ġ
        mStat.ResetData();      //���� �ʱ�ȭ
        gameObject.SetActive(true);     //Ȱ��ȭ

        PlayFSM();    //FSM �÷���
        mAnim.Reset();          //���ϸ��̼� �ʱ�ȭ
        mFSM.ChangeState(FSMState.Idle);    //StateIdle ��ȯ
        mTransform.LookAt(GameManager.Instance.GetPlayer.GetPos);    //Ÿ�� ���� ȸ��
        mbIsDie = false;     //���ó�� �ʱ�ȭ

        //HPBar
        TrackingBarSlider hpSlider;
        switch (meEnemyType)
        {
            case EEnemyType.Monster:
                //Ʈ��ŷ HP �����̴�
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

        //�ʵ� ��������Ʈ ������(���� ���ʹ� ������ ����)
        SpawnPointManager spawnPointManager;
        if(gameMgr.GetFieldSpawnPoint(out spawnPointManager))
        {
            spawnPointManager.GetNearSpawnPoint(startPos, out fieldSpawnPoint);     //���� ����Ʈ ��������
        }
    }   //Ȱ��ȭ
    public override void UnActive()
    {
        if(fieldSpawnPoint != null)
        {
            fieldSpawnPoint.SumEnemyCount();
        }

        //FSM ����ó��
        StopFSM();

        //��ų ��ƼŬ ����ó��
        for (int i = 0; i < mSkill.Count; ++i)
        {
            StopCoroutine(mSkill[i].SkillCoroutine());
            mSkill[i].StopParticle();
        }

        //HP�����̴� ����
        if(mBarSlider != null)
        {
            mBarSlider.StopAnim();      //�ִϸ��̼� ���� ó��

            switch (meEnemyType)
            {
                case EEnemyType.Monster:
                    UIManager.Instance.GetUIObjectPool.ReturnPool(mBarSlider);      //UI Ǯ�� ���ư���
                    break;
                case EEnemyType.Boss:
                    mBarSlider.SetActive(false);
                    break;
                default:
                    break;
            }
        }

        mBarSlider = null;

        ReturnPool?.Invoke();   //Ǯ�� ����
    }   //��Ȱ��ȭ
    private void PlayFSM()
    {
        mNAVAgent.enabled = true;
        fsmCoroutine = StartCoroutine(FSMCoroutine());
    }   //���ʹ� �۵�
    private void StopFSM()
    {
        //�ڷ�ƾ�� �۵����� �� ���߱�
        if (fsmCoroutine != null) 
        { 
            StopCoroutine(FSMCoroutine());
            mNAVAgent.enabled = false;
        }
        fsmCoroutine = null;
    }   //���ʹ� ����
    //FSMable ��� �Լ�
    #region FSMstate
    protected IEnumerator FSMCoroutine()
    {
        while (true)
        {
            mFSM.UpdateState();
            yield return new WaitForSeconds(0.5f);
        }
    }    //FSM �ڷ�ƾ
    public override void IdleStateEnter()
    {
        mTargetRigid = null;  //Ÿ���� ������ ó��
        Stop();
    }   //���̵� ���� ���� ��
    public override void IdleState() 
    {
        if (IsChangeState(FSMState.Return))
        {
            mFSM.ChangeState(FSMState.Return);    //Return ���� ��ȯ
            return;
        }
        else if (IsChangeState(FSMState.Move))
        {
            mFSM.ChangeState(FSMState.Move);      //Move ���� ��ȯ
            return;
        }
    }       //���̵� ���� ����
    public override void MoveEnter()
    {
    }
    public override void MoveState() 
    {
        //Attack ���� ��ȯ
        if (IsChangeState(FSMState.Attack))
        {
            mFSM.ChangeState(FSMState.Attack);   //Attck ���� ��ȯ
            return;
        }
        else if (IsChangeState(FSMState.Idle))
        {
            mFSM.ChangeState(FSMState.Idle);     //Idle ���� ��ȯ
            return;
        }

        Move(mTargetRigid.transform.position);
    }       //������ ���� ����
    public override void MoveExit()
    {
        Stop();
    }         //������ ���� Ż�� ��
    public override void AttackEnter()
    {}      //���� ���� ���� ��
    public override void AttackState() 
    {
        //���� ������ ���°� �ƴϸ� ���� ����
        mfAtkDelay -= 1.0f;       //������ �� ����
        if (mfAtkDelay <= 0.0f)
        {
            mfAtkDelay = 4.0f;    //������ �� �ʱ�ȭ
            Attack();
        }
        else if (IsChangeState(FSMState.Move))      //Move���� ��ȯ
        {
            mFSM.ChangeState(FSMState.Move);
        }
    }     //���� ���� ����
    public override void AttackExit()
    {
        Stop();
    }       //���� ���� Ż�� ��
    public override void DamagedStateEnter()
    {
        mAnim.Damaged();
    }
    public override void DamagedState()
    {
        //Attack ���� ��ȯ
        if (IsChangeState(FSMState.Attack))
        {
            mFSM.ChangeState(FSMState.Attack);   //Attck ���� ��ȯ
            return;
        }
        else if (IsChangeState(FSMState.Idle))
        {
            mFSM.ChangeState(FSMState.Idle);     //Idle ���� ��ȯ
            return;
        }
    }   //���̵� ���� ����
    public override void DamagedExit()
    {
        Stop();     //����ó��
    }
    public override void DeadStateEnter()
    {
        StopFSM();
        mbIsDie = true;

        //�������� �޴��� ���� ���
        StageManager stageMgr = null;
        if (GameManager.Instance.GetStageMgr(out stageMgr))
        {
            stageMgr.SumEnemyCount();
        }

        mEventMgr.AddValueEvent(EEventMessage.DeadEnemy, miKey);     //�� ����� ����ġ ���� �̺�Ʈ
        mEventMgr.CreateEvent(EEventMessage.CreateTableRandomItem, mTransform.position, miKey);   //���� ������ ���
        mAnim.Die();

        if(meEnemyType == EEnemyType.Monster)
        {
            StartCoroutine(DeadCoroutine());
        }
        else
        {
            Invoke("UnActive", 10.0f);   //10�� �� ��Ȱ��ȭ ó��
        }
    }
    public override void ReturnStateEnter()
    {
        Move(mStartPos);
    }   //���ư��� ���� ���� ��
    public override void ReturnState()
    {
        //������ġ ��ó�� �´ٸ� IdleState ��ȯ
        if(IsDistLong(mTransform.position, mStartPos, 3.0f))
        { 
            mFSM.ChangeState(FSMState.Idle); 
        }
    }       //���ư��� ���� ����
    public override void ReturnExit()
    {
        Stop();
    }       //���ư��� ���� Ż�� ��
    #endregion

    //State ���� ����� �Լ�
    #region FSMLogic
    /// <summary>
    /// targetPos : �̵��� ��ġ
    /// </summary>
    protected void Move(Vector3 targetPos)
    {
        mNAVAgent.isStopped = false;                                //agent ������ ó��
        mNAVAgent.SetDestination(targetPos);                        //agent Ÿ���� Path ã��
        mAnim.Move(1.0f);    //�����̴� ���ϸ��̼�
    }

    protected void Stop()
    {
        mAnim.Stop();
        mNAVAgent.isStopped = true;
        mNAVAgent.ResetPath();          //�� ���� ����
    }   //���� ����

    protected void Attack()
    {
        int iSkillIdx = -1;
        float fSkillDistance = -1.0f;
        float targetDist = Vector3.Distance(mTransform.position, mTargetRigid.position);
        DataManager dataMgr = GameManager.Instance.GetDataMgr();

        for (int i = 0; i < mSkill.Count; ++i)
        {
            //��Ÿ�� �ƴ��� üũ && �Ÿ��� ���� ��ų���� && �÷��̾� �����Ÿ� �ȿ� �ִ���
            float fSkillEndDist = dataMgr.GetSkillData(mSkill[i].SkillKey).EndDist;
            if (!mSkill[i].IsCoolTime &&
                fSkillEndDist > fSkillDistance &&
                fSkillEndDist > targetDist)
            {
                iSkillIdx = i;
                fSkillDistance = fSkillEndDist;
            }
        }

        //���ǿ� �����ϴ� ��ų ������ && ��ų �÷��� �� ��
        if(iSkillIdx != -1 &&
            mSkill[iSkillIdx].Play(mTransform.position, mTransform.forward))
        {
            mTransform.rotation = Quaternion.LookRotation((mTargetRigid.transform.position - mTransform.position).normalized);
            StartCoroutine(mSkill[iSkillIdx].SkillCoroutine());
            mAnim.Attack();      //���� ���ϸ��̼�
        }
    }   //���� �� 
    #endregion

    private bool IsChangeState(FSMState state)
    {
        switch (state)
        {
            case FSMState.Idle:
                { return !FindTarget(10.0f); }      //�ָ� ���� ��     
            case FSMState.Move:
                { return FindTarget(9.0f); }        //�ȿ� ���� ��
            case FSMState.Attack:
                { return FindTarget(5.0f); }        //�ȿ� ���� ��
            case FSMState.Return:
                { return !IsDistLong(mTransform.position, mStartPos, 15.0f); }      //�ָ� �ִ���
            case FSMState.Damaged:
                return meEnemyType == EEnemyType.Monster;    //���� Ÿ���� ����
            default:
                Debug.LogError("ChangeStateCheck state is Default");
                break;
        }
        return false;
    }   //State�� üũ

    /// <summary>
    /// fRange : Ÿ�� ã�� ����
    /// </summary>
    private bool FindTarget(float fRange)
    {
        Collider[] targetCollider = Physics.OverlapSphere(mTransform.position, fRange, miTargetLayerMask);

        //Ÿ���� ��� �ִٸ�
        if(targetCollider != null && targetCollider.Length > 0)
        {
            mTargetRigid = targetCollider[0].GetComponent<Rigidbody>();
            return true;
        }

        return false;
    }   //Ÿ���� ���� �� true ����

    /// <summary>
    /// aPosition : ��ġ a
    /// bPosition : ��ġ b
    /// dist : a~b ������ ����
    /// </summary>
    private bool IsDistLong(Vector3 aPosition, Vector3 bPosition, float dist)
    {
        //dist �� a~b�� ���� �� ���̺��� ũ�ٸ�
        if(dist >= Vector3.Distance(aPosition, bPosition))
        { 
            return true; 
        }

        return false;
    }   //�������� ���̺��� dist�� �� Ŭ �� true

    /// <summary>
    /// iDamage : ������
    /// dir : ���� ����
    /// </summary>
    public void Damaged(int iDamage, Vector3 dir)
    {
        //�׾��� �� ����
        if (mbIsDie)
        {
            return;
        }

        Vector3 hitPos = mTransform.position - (dir * 2.0f);    //�տ��� ǥ���� ��ġ = ���� ��ġ - �������� * 2.0f
        float randomDist = Random.Range(-0.2f, 0.2f);    //������ �߰��� ����

        //�� ��ƼŬ
        hitPos.x += randomDist;    //���� ����
        FloatingText floatingText = GameManager.Instance.GetObjPoolManager().GetObject(3003) as FloatingText;
        floatingText.Play(hitPos + mHeight, iDamage);

        hitPos.y += randomDist;    //���� ����
        Particle hitParticle = GameManager.Instance.GetObjPoolManager().GetObject(4002) as Particle;
        hitParticle.Active(hitPos + mHalfHeight, mTransform.forward);     //���ʹ� �� ��ƼŬ

        //�������� �԰� ü���� ���ٸ�
        if (mStat.Damaged(iDamage))
        {
            mFSM.ChangeState(FSMState.Dead);    //��� ���� ��ȯ
        }
        else if (meEnemyType == EEnemyType.Monster)     //���� Ÿ�� �ϰ��
        {
            mFSM.ChangeState(FSMState.Damaged);     //Hit ���� ��ȯ
        }

        if(mBarSlider != null)
        {
            mBarSlider.SliderAnim(mStat.HP);        //HPSlider
        }
    }   //������

    private IEnumerator DeadCoroutine()
    {
        //���ʹ� ���̸�ŭ ������ �̵�
        for (float fHeight = 0; fHeight < mHeight.y; fHeight += 0.1f)
        {
            Vector3 enemyPos = mTransform.position;
            enemyPos.y -= 0.1f;
            mTransform.position = enemyPos;
            yield return new WaitForSeconds(0.1f);
        }

        Invoke("UnActive", 4.0f);   //4�� �� ��Ȱ��ȭ ó��
    }

    //Ÿ�Ӷ��� �Լ�
    #region Timeline
    public void StartBossTimeline(Vector3 startPos)
    {
        GameManager gameMgr = GameManager.Instance;     //���� �޴���
        fieldSpawnPoint = null;
        mBarSlider = null;
        mfAtkDelay = 0.0f;      //���� ������ �ʱ�ȭ
        mStartPos = startPos;   //���� ��ġ ������ ����
        transform.position = startPos;    //������ġ

        mTransform.LookAt(gameMgr.GetPlayer.GetPos);    //�÷��̾� ���� �ٶ󺸱�
        gameObject.SetActive(true);     //Ȱ��ȭ

        mAnim.Attack();     //���� ���ϸ��̼� ����
    }

    public void EndBossTimeLine()
    {
        //���� HP�� Ȱ��ȭ
        mBarSlider = (UIManager.Instance.GetGUI(EGUI.Stage) as StagePanel).GetBossHPSlider;
        mBarSlider.SetSlider(mStat.HP, mStat.HP);
        mBarSlider.SetActive(true);

        mStat.ResetData();      //���� �ʱ�ȭ
        mbIsDie = false;     //���ó�� �ʱ�ȭ
        PlayFSM();    //FSM �÷���
        mAnim.Reset();          //���ϸ��̼� �ʱ�ȭ
        mFSM.ChangeState(FSMState.Attack);    //AttackState ��ȯ
        mTransform.LookAt(GameManager.Instance.GetPlayer.GetPos);    //Ÿ�� ���� ȸ��

    }
    #endregion
}

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
    private float attackDuration = 0;                //���� �ӵ�
    private float hitDelay = 0;

    readonly float attackDurationTime = 1.0f;        //���ݽð�
    readonly float hitDelayTime = 0.5f;              //������ ������ �ð�
    protected Skill[] skill = new Skill[2];          //��ų

    public override void Initialize(Monster monster)
    {
        this.monster = monster;
        anim = GetComponent<Animator>();            //���ϸ��̼�
        agent = GetComponent<NavMeshAgent>();       //AI
        agent.enabled = false;
        data = Resources.Load<EnemyData>("ScriptableObject/DragonData");    //������ ����

        for(int i = 0; i < 2; ++i)
        {
            skill[i] = Instantiate(data.SkillData[i].Skill);    //��ų����
            skill[i].gameObject.transform.SetParent(this.gameObject.transform);
            skill[i].Initialize(data.SkillData[i]);             //��ų ������ ����
        }

        enemyStateData = new EnemyStateData();      //State ������ ����
        stateMachine = new FSM(this);               //FSM State ����
        audioSource = gameObject.AddComponent<AudioSource>();
        soundData = Resources.Load<SoundClipData>("ScriptableObject/SoundData/DragonSoundData");
        this.gameObject.SetActive(false);           //�ش� ������Ʈ ��Ȱ��ȭ ó��
    }
    //FSM �ൿ���� �Լ�
    #region FSM
    public override void AttackToTarget()
    {
        //�÷��̾� �������� �ٶ� �� �ʱ�ȭ, ����ó��
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
    }   //���� ó��

    public override void MoveToTarget()
    {
        if (!targetRigid) return;
        agent.isStopped = false;                                    //AI ������Ʈ�� ������ ó��
        agent.SetDestination(targetRigid.position);
        anim.SetFloat("MoveFront", 1.0f);       //���ϸ��̼� Walk ����
    }   //������ ó��

    public override void Stop()
    {
        anim.SetFloat("MoveFront", 0.0f);       //���ϸ��̼� Idle ����
        agent.isStopped = true;
        agent.ResetPath();                      //AI ��ã�� ����
    }   //���� ó��

    public override void ReadyAttack()
    {
        attackDuration = 0;
    }   //������ ���� �ʱ�ȭ

    public override void SetDamaged()
    {
        hitDelay = data.HitDelay;
        //anim.SetTrigger("Damaged");
    }   //�������� �Ծ�����
    #endregion

    //FSM �Ÿ�, ����, �ð� üũ �Լ�
    #region FSMcheck
    public override bool SearchRangeCheck()
    {
        return TargetInRange(data.SearchRangeSquare);
    }   //���� Ž������ �ȿ� ������ true ����
    public override bool AttackRangeCheck()
    {
        return TargetInRange(data.AttackRangeSquare);
    }   //���� ���ݹ��� �ȿ� ������ true ����
    public override bool AttackDurationCheck()
    {
        //��ų�� ��� ���̶�� true
        for(int i = 0; i < skill.Length; ++i)
        { if (skill[i].PlayCheck()) { return true; } }
        
        //��ų�� ������� ���� �� ���� �ð� ����
        if (0 < attackDuration)
        {
            attackDuration -= attackDurationTime;
            return true;
        }

        return false;
    }   //���� �ӵ� �ð��� �غ� �ȵɽ� true ����, �غ�ɽ� false ����
    public override bool HitDelayCheck()
    {
        if (0 < hitDelay)
        {
            hitDelay -= hitDelayTime;
            return true;
        }
        return false;
    }   //�¾��� �� ������
    #endregion

    protected override void Attack()
    {
        attackDuration = data.AttackDelay;
        Collider[] targets = Physics.OverlapSphere(transform.position, data.AttackRange, data.TargetMask);
        if (targets != null && TargetInRange(data.AttackRangeSquare) && TargetInAngle(data.AttackAngle))
        { targets[0].GetComponent<Player>().Damaged(data.Damege); }
    }   //���� ������ ������ �÷��̾�� ������� ������.

    protected override void StopCoroutine()
    {

    }
}

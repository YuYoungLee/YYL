using UnityEngine;
using UnityEngine.AI;
//TODO FSM �����

public class Grunt : Enemy
{    
    float attackDuration = 0;                     //���� �ӵ�
    float hitDelay = 0;
    
    readonly float attackDurationTime = 1.0f;        //���ݽð�
    readonly float hitDelayTime = 0.5f;              //������ ������ �ð�
    
    public override void Initialize(Monster monster)
    {
        this.monster = monster;
        anim = GetComponent<Animator>();            //�׷�Ʈ ���ϸ��̼�
        agent = GetComponent<NavMeshAgent>();       //AI
        agent.enabled = false;
        data = Resources.Load<EnemyData>("ScriptableObject/GruntData");    //GruntData
        enemyStateData = new EnemyStateData();      //State ������ ����
        stateMachine = new FSM(this);               //FSM State ����
        audioSource = gameObject.AddComponent<AudioSource>();
        soundData = Resources.Load<SoundClipData>("ScriptableObject/SoundData/GruntSoundData");
        this.gameObject.SetActive(false);           //�ش� ������Ʈ ��Ȱ��ȭ ó��
    }
    //FSM �׷�Ʈ ������ �ൿ���� �Լ�
    #region GruntFSM
    public override void AttackToTarget()
    {
        //�÷��̾� �������� �ٶ� �� �ʱ�ȭ, ����ó��
        if (!LookRotationToTarget()) return;
        Stop();
        anim.SetTrigger("Attack");
        Invoke("Attack", 0.35f);  //0,37�� ����, 0,5�ʸ��� �ݺ�
    }   //�׷�Ʈ�� ����
    
    public override void MoveToTarget()
    {
        if(!targetRigid) return;
        agent.isStopped = false;                                    //AI ������Ʈ�� ������ ó��
        agent.SetDestination(targetRigid.position);
        anim.SetFloat("MoveFront", agent.velocity.magnitude);       //���ϸ��̼� Walk ����
    }   //�׷�Ʈ�� ������ ó��
    
    public override void Stop()
    {
        anim.SetFloat("MoveFront", 0.0f);       //���ϸ��̼� Idle ����
        agent.isStopped = true;
        agent.ResetPath();                      //AI ��ã�� ����
    }   //�׷�Ʈ�� ���� ó��
    
    public override void ReadyAttack()
    {
        attackDuration = 0;
    }   //������ ���� �ʱ�ȭ

    public override void SetDamaged()
    {
        hitDelay = data.HitDelay;
        anim.SetTrigger("Damaged");
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
        //TargetMask == enemy, ���ݹ���, ���� �ȿ� ������
        Collider[] targets = Physics.OverlapSphere(transform.position, data.AttackRange, data.TargetMask);
        if (targets != null && TargetInRange(data.AttackRangeSquare) && TargetInAngle(data.AttackAngle))
        { targets[0].GetComponent<Player>().Damaged(data.Damege); }
    }   //���� ������ ������ �÷��̾�� ������� ������.

    protected override void StopCoroutine()
    {

    }
}

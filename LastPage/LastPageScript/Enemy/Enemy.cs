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
}   //ChangeState�� ������

public abstract class Enemy : MonoBehaviour
{
    protected bool DieStatus = false;   //��� ����
    protected Monster monster;          //����Ÿ��
    protected int hp;                   //ü��
    protected float hitTimer;           //�¾����� ������ Ÿ�̸�

    protected EnemyHealthBar healthBar = null;  //������ ü�¹�

    protected Rigidbody targetRigid = null;     //�÷��̾��� rigidbody
    protected NavMeshAgent agent;               //AI �޽� ������Ʈ
    protected Animator anim;                    //���ϸ��̼�

    protected EnemyData data = null;            //ScriptableObject Data
    protected EnemyStateData enemyStateData = null;       //�׷�Ʈ�� StateData

    protected FSM stateMachine = null;          //FSM �ӽ�
    readonly float waitUpdateTime = 0.5f;       //FSM ���ð�
    protected Coroutine coroutineFSM = null;    //FSM�� �ڷ�ƾ

    protected AudioSource audioSource = null;   //����� ���
    protected SoundClipData soundData = null;   //���� ������

    #region data Init
    public abstract void Initialize(Monster monster);
    #endregion

    #region FSM Func
    //���� < �����ϴ��ʿ��� float�� �Ѱ��ֱ�
    protected IEnumerator OnUpdate(float waitSec = 0.0f)
    {
        yield return new WaitForSeconds(waitSec);
        while (true)
        {
            stateMachine.StateUpdate();
            yield return new WaitForSeconds(waitUpdateTime);
        }   //FSM ����
    }   //FSM Update
    public abstract void AttackToTarget();
    public abstract void MoveToTarget();
    public abstract void Stop();
    public abstract void ReadyAttack();                     //������ ���� �ʱ�ȭ
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
    }   //StateMachine ���� ��ȯ ȣ��
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

    //FSM bool �Լ�
    #region FSMcheck
    public abstract bool SearchRangeCheck();        //�÷��̾ Ž�� ���� �ȿ� ������ true
    public abstract bool AttackRangeCheck();        //�÷��̾ ���� ���� �ȿ� ������ true
    public abstract bool AttackDurationCheck();     //���ݼӵ� üũ
    public abstract bool HitDelayCheck();           //���� ������
    #endregion

    protected abstract void Attack();       //�÷��̾� ����
    public void Damaged(int damage)
    {
        if (DieStatus) return;

        //������ ó���Ұ�
        hp = Mathf.Max(hp - damage, 0);                         //������ ���

        //���� Ÿ���� ������ �� ü�¹� ����
        if (monster == Monster.Monster_Boss)
        { GameManager.Instance.GetUIManager().GetEnemyUI.HealthBar.SetHp(hp, data.HP); }
        else  //���� Ÿ���� ü�¹� �� ���
        { 
            healthBar.StartHitCoroutine();
            healthBar.UpdateUI(hp);           //ü�¹� �� ����
        }
        
        GameManager.Instance.CallDamageTextPool(damage, this.transform.position);   //������ �ؽ�Ʈ ǥ��

        if (hp < 1)
        {
            if (monster != Monster.Monster_Boss) { audioSource.PlayOneShot(soundData.GetAudioClip(SFXEnemySoundType.Damaged)); }
            ChangeState(EnemyState.Die);
        }
        else
        {
            ChangeState(EnemyState.Damaged);    //���� ��ȯ �������� ���� ��
        }
    }           //���� �¾����� ó��
    public void SetActive(Vector3 SpawnPos, EnemyHealthBar healthBar, Rigidbody targetRigid)
    {
        hp = data.HP;                                       //ü�� �ʱ�ȭ
        this.targetRigid = targetRigid;
        this.transform.position = SpawnPos;                 //���� ��ġ�� ����
        LookRotationToTarget();                             //�÷��̾� �������� �ٶ󺸱�

        this.gameObject.SetActive(true);                    //������Ʈ Ȱ��ȭ
        agent.enabled = true;
        stateMachine.SetState(enemyStateData.idle);         //idle State�� ����
        //ü�¹ٰ� ������, ���� ���� Ÿ���� �ƴ� ��
        if (healthBar != null && monster != Monster.Monster_Boss)
        {
            this.healthBar = healthBar;   //ü�¹�
            this.healthBar.SetData(SpawnPos, this.gameObject.transform, data.HP);
            coroutineFSM = StartCoroutine(OnUpdate());          //�ڷ�ƾ Ȱ��ȭ
        }
        else
        {
            anim.SetTrigger("Scream");
            GameManager.Instance.GetUIManager().GetEnemyUI.HealthBar.SetBossUI(hp, data.HP);
            coroutineFSM = StartCoroutine(OnUpdate(3.0f));     //3������ �� �ڷ�ƾ Ȱ��ȭ
        }
    }   //������ ������ �Լ�
    public void SetDisable()
    {
        if (monster == Monster.Monster_Boss) 
        {
            GameManager.Instance.GetTeleporter.SetActive(this.gameObject.transform.position);
            GameManager.Instance.GetUIManager().GetEnemyUI.HealthBar.SetActive(false); //���� ü�¹� ��Ȱ��ȭ
        }
        else { healthBar.ReturnPool(); }
        Stop();                             //����ó��
        StopCoroutine(OnUpdate());          //�ڷ�ƾ ����
        coroutineFSM = null;
        StopCoroutine();                    //�ڷ�ƾ ������� ����ó��
        stateMachine.DeleteState();         //State ����
        agent.enabled = false;
        this.gameObject.SetActive(false);   //��Ȱ��ȭ ó��

        GameManager.Instance.GetEnemyPool.ReturnPool(monster, this);
    }   //������Ʈ ��Ȱ��ȭ �� ����
    protected abstract void StopCoroutine();
    protected bool TargetInRange(float sqrRange)
    {
        float dir = Vector3.SqrMagnitude(targetRigid.position - transform.position);

        if (dir < sqrRange)
        { return true; }
        return false;
    }   //Ÿ���� radius ���� �ȿ� ������ true ����
    protected bool TargetInAngle(float angle)
    {
        if (!targetRigid) return false; //Ÿ���� ������ ����� �Ұ����ϹǷ� false ����
        Vector3 dir = (targetRigid.position - transform.position).normalized;
        if (Vector3.Angle(transform.forward, dir) < angle) { return true; }
        return false;
    }   //Ÿ���� angle ���� �ȿ� ������ true ����
    protected bool LookRotationToTarget()
    {
        Vector3 dir = (targetRigid.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(dir);
        return true;
    }   //�÷��̾� �������� �ٶ󺸱�
}

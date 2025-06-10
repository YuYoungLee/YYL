using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : FSMAble, IDamageable
{
    public override event System.Action ReturnPool;

    [SerializeField] private int miPlayerKey = 0;     //�÷��̾� Ű��
    private Rigidbody rigid = null;         //�÷��̾� rigidbody

    [Header("Slope")]
    private RaycastHit slopeHit;

    private readonly float mfSlopeAngle = 45.0f;
    private readonly int miGroundLayerMask = (1 << 8); //���̾� ����ũ

    [Header("CameraGeometry")]
    [SerializeField] private Geometry geometry;     //ī�޶� Ÿ�� ������Ʈ��

    [Header("Anim")]
    private PlayerAnim mAnim = null;     //�÷��̾� ���ϸ��̼�

    [Header("Stat")]
    private PlayerStat mStat = null;    //�÷��̾� ����

    [Header("Sound")]
    private AudioSource mPlayerSFX = null;

    //Todo. Move ����
    [Header("Movement")]
    private Vector2 moveDir;        //����
    private Vector2 rotateDir;      //ȸ��
    private Vector3 velocity;       //�ӷ�
    private float mfYaw;              //y�� ȸ��
    private float mfPitch;            //x�� ȸ��
    private const float maxRotateDir = 1.0f;
    private Vector2 mMouseSensivity;   //���콺 ����
    public bool isGround = true;                       //�� ���� �ִ� ��� true
    private readonly float mfGravity = 9.81f;           //�߷°� 9.81f
    private readonly int miTargetLayerMask = (1 << 7);  //���̾� ����ũ
    private readonly float mfDownForce = 5.0f;          //�Ʒ��� �������� �� (�߷¿� * �� ��)
    private float jumpForce;        //���� ��

    private Vector3 mHeight;       //�÷��̾� ���̰�
    private float speed;           //�ӵ� test ��

    private List<Skill> mSkill;     //��ų
    private Skill mAttack;    //�⺻����
    private Coroutine mAttackCoroutine;    //���� �ڷ�ƾ
    private Coroutine mInputStopMoveCoroutine;      //��ǲ ���߱�

    private bool mbIsAttack;
    private bool mbIsDie;

    public bool mbGizmoDraw = false;
    public float mfGizmoAngle = 0.0f;
    public float GizmoDist;
    public Vector3 forwardGizmo;


    [SerializeField] private PlayerPivot mPivot;     //�Ǻ�    

    private float mfRotateYDir = 1;    //Y�� ȸ�� ����

    [Header("Interaction")]
    private List<IInteractAble> mInteractOBJ;   //��ȣ�ۿ� ������Ʈ

    public int PlayerKey => miPlayerKey;    //�÷��̾� Ű��

    public Vector3 GetPos
    {
        get { return rigid.position; }
    }    //�÷��̾� ��ġ

    public PlayerStat PlayerStat => mStat;      //�÷��̾� ����

    public bool IsInteraction => mInteractOBJ.Count > 0;    //��ȣ�ۿ�

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
        Rotate();   //ȸ��
    }

    public override void Initialize()
    {
        DataManager dataMgr = GameManager.Instance.GetDataMgr();    //������ �޴��� �ӽú���

        base.Initialize();
        geometry.Initialize();      //ī�޶� Ÿ�� ������Ʈ��

        mAnim = new PlayerAnim();    //�÷��̾� �ִϸ��̼�
        mAnim.SetAnim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        mTransform = GetComponent<Transform>();

        //Data �� �ʱ�ȭ
        mbIsAttack = false;
        mbIsDie = false;
        velocity = Vector2.zero;
        moveDir = Vector2.zero;
        rotateDir = Vector2.zero;
        mfPitch = mTransform.rotation.z;
        mfYaw = mTransform.rotation.y;
        mMouseSensivity = new Vector2(100.0f, 100.0f);  //���콺 �ΰ���
        jumpForce = 10.0f;       //���� ��
        speed = 5.0f;           //���� �ӵ�

        //�÷��̾� ����
        if (mStat == null)
        {
            mStat = new PlayerStat();
        }
        mStat.Initialize(miPlayerKey);

        //����
        if (mPlayerSFX == null)
        {
            mPlayerSFX = GetComponent<AudioSource>();
        }

        //Skill
        if (mSkill == null)
        {
            mSkill = new List<Skill>();     //��ų ����Ʈ ����


            foreach (int iSkillKey in dataMgr.GetPlayerData(miPlayerKey).SkillKeyList)
            {
                Skill skill = new Skill();    //��ų���� �ӽú���
                skill.SetData(iSkillKey, miTargetLayerMask, mStat, mTransform);
                mSkill.Add(skill);
            }
        }

        //�÷��̾� �⺻����
        if (mAttack == null)
        {
            mAttack = new Skill();
            mAttack.SetData(9, miTargetLayerMask, mStat, mTransform);
        }

        //��ȣ�ۿ� ������Ʈ
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

    //�÷��̾� ��ǲ
    #region Input
    public void AttackStart(InputAction.CallbackContext context)
    {
        //�� ���� ���� �� ���� ���� �ƴҶ���
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

        //���� ������
        if (isGround)
        {
            moveDir = context.ReadValue<Vector2>(); //�����̴� ����
            mFSM.ChangeState(FSMState.Move);
        }
        else
        {
            moveDir = Vector2.zero;     //����� �� ���
        }

        velocity = rigid.transform.forward * moveDir.y + rigid.transform.right * moveDir.x; //���Ⱚ ���ϱ�
        velocity = velocity.normalized * speed;             //�ӷ� = ���� * �ӵ�;
    }   //�÷��̾� ������ ��ǲ ó��

    public void InputRotate(InputAction.CallbackContext context)
    {
        rotateDir = context.ReadValue<Vector2>();
        rotateDir *= 0.1f;

        //���� 1, -1, 0 ������ ����
        rotateDir.x = Mathf.Clamp(rotateDir.x, -maxRotateDir, maxRotateDir);
        rotateDir.y = Mathf.Clamp(rotateDir.y, -maxRotateDir, maxRotateDir);
        rotateDir.y *= mfRotateYDir;     //���� ���� �� 1, ������ �� -1�� Y�� ���ϱ�

        //ȸ�� �� �� ���� ��ȯ
        velocity = rigid.transform.forward * moveDir.y + rigid.transform.right * moveDir.x; //���Ⱚ ���ϱ�
        velocity = velocity.normalized * speed;             //�ӷ� = ���� * �ӵ�;
    }   //ȸ�� ��ǲ

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
            velocity.y = 0.0f; //���� ���� ���� y�� 0����
        }
        mFSM.ChangeState(FSMState.Idle);
    }   //�����ӹ��� �ʱ�ȭ

    /// <summary>
    /// iSkillKey : ��ų Ű��
    /// </summary>
    public void InputSkill(int iSkillKey)
    {
        //�⺻ ������ �ϰ� ���� ������, �� ���� ���� �� ����
        if (mbIsAttack || !isGround)
        {
            return;
        }

        //��ų ��� ���� ��
        for (int i = 0; i < mSkill.Count; ++i)
        {
            //Ű���� ��ġ�� ��ų�� ���� ��, ��ų ��� �� �� ���� ��
            if (iSkillKey == mSkill[i].SkillKey &&
                mSkill[i].Play(mTransform.position, mTransform.forward))
            {
                DataManager dataMgr = GameManager.Instance.GetDataMgr();
                float fStopTime = 0.0f;

                //�и� ��ų �����ð� �߰�
                switch (dataMgr.GetSkillData(iSkillKey).AttackType)
                {
                    case EAttackType.Melee:
                        fStopTime =
                            dataMgr.GetSkillData(iSkillKey).StartTime +
                            dataMgr.GetSkillData(iSkillKey).DurationCount * dataMgr.GetSkillData(iSkillKey).RepeatTime;    //�⺻ ��ų �����ð�
                        break;
                    default:
                        fStopTime = dataMgr.GetSkillData(iSkillKey).StartTime;
                        break;
                }
                //Invoke("Gizemo", fStopTime);

                mInputStopMoveCoroutine = StartCoroutine(InputStopMoveCoroutine(fStopTime));
                StartCoroutine(mSkill[i].SkillCoroutine());    //�ڷ�ƾ ����
                mAnim.Attack(mSkill[i].SkillKey);    //�ִϸ��̼� ����
            }
        }
    }   //��ų ��ǲ

    public void InputJump(InputAction.CallbackContext context)
    {
        //���� ��� ���� �� ���� ��� ����
        if (!isGround)
        {
            return;
        }
        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);    //�������� �� ����
        PlayOneShot("PlayerJump");
    }   //���� ��ǲ

    public void InputInteraction(InputAction.CallbackContext context)
    {
        if (mInteractOBJ.Count > 0)
        {
            IInteractAble interaction = mInteractOBJ[0];
            //���� �ؾ� �� Ÿ�� �� ��
            if (mInteractOBJ[0] is DropItem || mInteractOBJ[0] is SceneTeleport)
            {
                mInteractOBJ.RemoveAt(0);
            }

            interaction.Interaction();  //��ȣ�ۿ�

            //��ȣ�ۿ� ������ ������Ʈ�� ���ٸ�, NPC �� ��
            //��ȣ�ۿ� ǥ�� ����
            if (mInteractOBJ.Count == 0 || mInteractOBJ[0] is NPC)
            {
                (UIManager.Instance.GetGUI(EGUI.Player) as PlayerPanel).InteractionUIObject.SetActive(false);     //����
            }
        }
    }   //��ȣ�ۿ� ��ǲ
    #endregion

    //�÷��̾� ������ ���
    #region Update
    private void Move()
    {
        rigid.velocity = velocity;
    }   //������ ���
    private void Rotate()
    {
        mfYaw += rotateDir.x * mMouseSensivity.x * Time.deltaTime;     //���콺 ���� ����ŭ ȸ��
        mfPitch += rotateDir.y * mMouseSensivity.y * Time.deltaTime;

        mfYaw = ClampDgree(mfYaw, -360, 360);     //360 ~ -360ȸ���� ���� ������ �� ����
        mfPitch = Mathf.Clamp(mfPitch, -90, 80);     //-90 80�� �� ����

        mTransform.rotation = Quaternion.Euler(0.0f, mfYaw, 0.0f);     //�÷��̾� Yaw ȸ��
        geometry.SetRotate(mfPitch, mfYaw);     //ī�޶� Ÿ���� ������Ʈ�� ȸ��
    }   //ȸ�� ���
    private void Gravity()
    {
        RaycastHit hit;
        // SphereCast�� �� Ȯ��
        if (Physics.Raycast(mTransform.position + Vector3.up * 0.3f, Vector3.down, out hit, 0.4f, miGroundLayerMask))
        {
            velocity.y = 0.0f;    //���� �� y�� �ӵ� 0 ����
            isGround = true;
            return;
        }

        // ���� ���� �ʾ��� ��� �߷� ����
        rigid.velocity += Vector3.down * mfGravity * mfDownForce * Time.deltaTime;
        isGround = false;

    }//�߷� ����
    private void Slope()
    {
        //���� �� �ƴ� �� && ���������� ���� 45�� �ƴ� ��
        if (Physics.Raycast(mTransform.position + velocity.normalized * 0.3f, Vector3.down, out slopeHit, 0.3f, miGroundLayerMask) &&
            slopeHit.normal != Vector3.up &&
            Vector3.Angle(Vector3.up, slopeHit.normal) < mfSlopeAngle)
        {
            velocity = Vector3.ProjectOnPlane(velocity, slopeHit.normal);   //���� ���ͷ� �̵�
        }
    }   //������ �� ó��
    #endregion

    //FSMable Override �Լ���
    #region FSMstate
    public override void IdleStateEnter()
    {
        mAnim.Stop();                //���ϸ��̼� ���� ����
    }
    public override void IdleState() { }     //���̵� ���� ����
    public override void DamagedStateEnter()
    {
        PlayOneShot("PlayerDamaged");    //������ ����

        //���� ������� �� ����
        if (mInputStopMoveCoroutine != null || mAttackCoroutine != null)
        {
            return;
        }

        mInputStopMoveCoroutine = StartCoroutine(InputStopMoveCoroutine(1.0f));
        mAnim.Damaged();
        //���ϸ��̼� ����
    }
    public override void DamagedState()
    {
    }
    public override void DamagedExit()
    {
    }
    public override void DeadStateEnter()
    {
        GameManager.Instance.GetInputMgr.SetActionMap(EActionMap.NoInput);      //��ǲ ����
        mAnim.Dead();
        PlayOneShot("PlayerDead");    //��� ����
        //��Ȱ�г� ����
        (UIManager.Instance.GetGUI(EGUI.Player) as PlayerPanel).RevivalPlayer.SetActive(true);

    }       //��� ���� ����
    public override void MoveEnter()
    {
        StopLoop();
        PlayLoop("PlayerWalk");    //�����̱� ����
    }
    public override void MoveState()
    {
        mAnim.Move(moveDir.x, moveDir.y);
    }       //������ ���� ����
    public override void MoveExit()
    {
        StopLoop();
    }
    public override void AttackEnter()
    {
        velocity = Vector3.zero;    //�ӷ� 0���� ����
        mbIsAttack = true;     //���ݻ��� ��ȯ
        //�⺻���� ����
        if (mAttackCoroutine == null)
        {
            mAttackCoroutine = StartCoroutine(AttackCoroutine());
        }
    }   //���� ���� ���� ��
    public override void AttackState()
    {
    }     //���� ���� ����
    public override void AttackExit()
    {
        if (mAttackCoroutine != null)
        {
            StopCoroutine(mAttackCoroutine);
        }
        mbIsAttack = false;
        mAttackCoroutine = null;
    }       //���� ���� Ż�� ��
    public override void ReturnStateEnter() { }
    public override void ReturnState() { }
    public override void ReturnExit() { }

    #endregion

    //���� �Լ�
    #region CarculateMath
    /// <summary>
    /// dir : ���Ⱚ
    /// </summary>
    private float ClampDir(float dir)
    {
        if (dir > maxRotateDir) return maxRotateDir;            //0���� Ŭ��
        else if (dir < -maxRotateDir) return -maxRotateDir;     //0���� ������
        return dir;
    }   //���� ���� ������ 1, -1, 0 ������ ����

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
        mPlayerSFX.PlayOneShot(SoundManager.Instance.AudioData(strKey));    //���� �ѹ� ���
    }   //���� �ѹ� ���

    private void PlayLoop(string strKey)
    {
        mPlayerSFX.clip = SoundManager.Instance.AudioData(strKey);
        mPlayerSFX.loop = true;
        mPlayerSFX.Play();
    }   //���� ���� ���

    private void StopLoop()
    {
        mPlayerSFX.clip = null;
        mPlayerSFX.loop = false;
        mPlayerSFX.Stop();
    }   //���� ���� ���� ����
    #endregion

    /// <summary>
    /// iDamage : ������
    /// dir : ���� ����
    /// </summary>
    public void Damaged(int iDamage, Vector3 dir)
    {
        //��� �� ����
        if (mbIsDie)
        {
            return;
        }

        //�� ��ƼŬ
        Vector3 hitPos = mTransform.position - (dir * 2.0f);    //�տ��� ǥ���� ��ġ = �÷��̾� ��ġ - �������� * 2.0f
        float randomDist = Random.Range(-0.2f, 0.2f);    //������ �߰��� ����
        hitPos.y += randomDist;    //���� ����
        hitPos.x += randomDist;    //���� ����
        Particle hitParticle = GameManager.Instance.GetObjPoolManager().GetObject(4002) as Particle;
        hitParticle.Active(hitPos + mHeight, mTransform.forward);

        //mStat.Hp
        if (mStat.Damaged(iDamage))
        {
            mbIsDie = true;
            //��� �� ó��
            mFSM.ChangeState(FSMState.Dead);    //���ϸ��̼� ��� ó��
        }

        mFSM.ChangeState(FSMState.Damaged);

        PlayerPanel playerPanel = UIManager.Instance.GetGUI(EGUI.Player) as PlayerPanel;
        playerPanel.SliderAnim(EPlayerSlider.HP, (float)mStat.HP);
    }   //������ ������ ó��

    //Ʈ���� �Լ�
    #region Trigger
    public void OnTriggerEnter(Collider other)
    {
        //��ȣ�ۿ� ������ ������Ʈ �� ��
        if (other.tag == "InteractAble")
        {
            IInteractAble interactOBJ = other.GetComponent<IInteractAble>();    //������Ʈ �ޱ�
            //����Ʈ�� ���ԵǾ� ���� �ʴٸ�
            if (!mInteractOBJ.Contains(interactOBJ))
            {
                mInteractOBJ.Add(interactOBJ);      //����
                (UIManager.Instance.GetGUI(EGUI.Player) as PlayerPanel).InteractionUIObject.SetActive(true);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        //��ȣ�ۿ� ������Ʈ�϶�
        if (other.tag == "InteractAble")
        {
            IInteractAble interactOBJ = other.GetComponent<IInteractAble>();    //������Ʈ �ޱ�
            //����Ʈ�� �����ϰ� ������
            if (mInteractOBJ.Contains(interactOBJ))
            {
                mInteractOBJ.Remove(interactOBJ);      //����
            }

            //��ȣ�ۿ� ������ ������Ʈ�� ���� ��
            if (mInteractOBJ.Count == 0)
            {
                (UIManager.Instance.GetGUI(EGUI.Player) as PlayerPanel).InteractionUIObject.SetActive(false);     //����
            }
        }
    }
    #endregion

    //�÷��̾� ����
    #region SetPlayer
    public void SetMove(Vector3 position, float yaw)
    {
        mfYaw = yaw;
        mfPitch = -30.0f;
        mTransform.position = position;
    }   //�÷��̾� ��ǥ �̵�, �ٶ󺸱�

    public void SetLookAt(Vector3 pos)
    {
        Vector3 dir = (pos - mTransform.position).normalized;
        dir.y = 0f;
        Quaternion rot = Quaternion.LookRotation(dir);
        Vector3 rotate = rot.eulerAngles;
        mfYaw = rotate.y;
    }   //�÷��̾� ���� �ٶ󺸱�

    public void SetAxisYInverse(bool bAxisYInverseStatu)
    {
        //������ 1, ������ -1
        if (bAxisYInverseStatu)
        {
            mfRotateYDir = 1;
        }
        else
        {
            mfRotateYDir = -1;
        }
    }   //Y�� ���� ����

    public void SetMouseSencivity(float fSensivityX, float fSensivityY)
    {
        mMouseSensivity.x = fSensivityX;
        mMouseSensivity.y = fSensivityY;
    }   //���콺 �ΰ��� ����

    /// <summary>
    /// eStat : ������ ����
    /// iStat : ���� ���� ��
    /// </summary>
    public void AddStat(EStatDataType eStat, int iStat)
    {
        mStat.Add(eStat, iStat);
    }   //�÷��̾� ���� ����

    /// <summary>
    /// eStat : ������ ����
    /// iStat : �� ���� ��
    /// </summary>
    public bool SumStat(EStatDataType eType, int iStat)
    {
        return mStat.Sum(eType, iStat);
    }   //�÷��̾� ���� ����

    /// <summary>
    /// iSkillKey : ��ų Ű��
    /// iSkillLevel : ������ ��ų ����
    /// </summary>
    public void SetSkillPower(int iSkillKey, int iSkillLevel)
    {
        for (int i = 0; i < mSkill.Count; ++i)
        {
            //��ų Ű�� ������ �Ŀ� ����
            if (mSkill[i].SkillKey == iSkillKey)
            {
                mSkill[i].SetSkillPower(iSkillLevel);
                break;
            }
        }
    }   //��ų�Ŀ� ����

    /// <summary>
    /// iSkillKey : ��ų Ű��
    /// quickSlot : ������
    /// </summary>
    public void SetTargetQuickSlot(int iSkillKey, QuickSlot quickSlot)
    {
        for (int i = 0; i < mSkill.Count; ++i)
        {
            //��ų Ű�� ���ٸ� ������ ���
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
    }   //������ ����
    #endregion

    public void LoadData(SaveData saveData)
    {
        mInteractOBJ.Clear();   //��ȣ�ۿ� �ʱ�ȭ
        (UIManager.Instance.GetGUI(EGUI.Player) as PlayerPanel).InteractionUIObject.SetActive(false);
        mStat.ResetData(miPlayerKey);   //���ݸ���
        mStat.LoadData(saveData);     //������ �ҷ�����
    }   //�÷��̾� ������ �ε�

    private IEnumerator AttackCoroutine()
    {
        while (true)
        {
            //���� ���
            if (mAttack.Play(mTransform.position, mTransform.forward))
            {
                StartCoroutine(mAttack.SkillCoroutine());       //�ڷ�ƾ
                mAnim.Attack(mAttack.SkillKey);     //����
                PlayOneShot("SwordAttack");     //���� ���
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    private IEnumerator InputStopMoveCoroutine(float fTime)
    {
        mbIsAttack = true;    //���ݻ��� ��ȯ
        velocity = Vector3.zero;    //�ӷ� 0���� ����
        yield return new WaitForSeconds(fTime);     //�ð�����
        mbIsAttack = false;     //���ݻ��� ����

        mInputStopMoveCoroutine = null;
    }   //fTime ��ŭ �÷��̾� ���� ��ǲ ���߱�

    public void ResetPlayer()
    {
        mInteractOBJ.Clear();   //��ȣ�ۿ� ����
        (UIManager.Instance.GetGUI(EGUI.Player) as PlayerPanel).InteractionUIObject.SetActive(false);   //��ȣ�ۿ� ǥ�� ����

        if (mAttackCoroutine != null)
        {
            StopCoroutine(mAttackCoroutine);    //����
            mAttackCoroutine = null;
        }

        if (mInputStopMoveCoroutine != null)
        {
            StopCoroutine(mInputStopMoveCoroutine);     //�����ð�
            mInputStopMoveCoroutine = null;
        }

        for (int i = 0; i < mSkill.Count; ++i)
        {
            mSkill[i].StopParticle();
        }

        isGround = true;
        mbIsAttack = false;

    }   //�÷��̾� ������ ����

    public void RevivalPlayer()
    {
        ResetPlayer();    //����
        mFSM.ChangeState(FSMState.Idle);    //Idle
        //ü��
        AddStat(EStatDataType.HP, mStat.MaxHP);
        AddStat(EStatDataType.MP, mStat.MaxMP);
        mAnim.Reset();
        mbIsDie = false;
    }   //�÷��̾� ��Ȱ ��
}
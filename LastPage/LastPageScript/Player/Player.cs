using UnityEngine.InputSystem;
using UnityEngine;
public enum PLAYER_CLASS
{
    WARRIOR,
}

public abstract class Player : IObserver
{
    protected bool dieStatus = false;                            //사망시 
    protected Transform playerTrans = null;                      //플레이어 트렌스폼 카메라만 사용
    protected Geometry targetCamera = null;                      //카메라 타겟에 사용할 오브젝트
    protected Rigidbody rigid = null;                            //플레이어 리지드바디
    protected Animator anim = null;                              //플레이어 에니메이션
    protected RaycastHit slopeHit;                               //바닥 경사 각도계산 용도
    
    protected WarriorData data = null;                           //플레이어의 데이터
    protected SoundClipData soundData = null;                    //플레이어 사운드 데이터
    protected AudioSource audioSource = null;                    //사운드 재생
    protected Skill[] skill = new Skill[5];                      //플레이어 스킬
    //call
    public delegate UIManager CallUIManager();
    public CallUIManager uiManager;                              //플레이어 UI 호출
    public delegate QuestManager CallQuestManager();
    public CallQuestManager questManager;                        //플레이어 UI 호출
    private InteractionObject interactionObject = null;          //상호 작용 오브젝트

    #region PlayerStat
    [Header("Move")]
    private float rotateYaw = 0.0f;     //y 축 회전
    private float rotatePitch = 0.0f;   //x 축 회전
    private float playerHeight = 1.0f;
    protected bool interactionStatus = false;
    private bool isGround = true;
    protected float rotateSpeed = 5.0f;                      //플레이어 곱할 속도
    protected Vector3 velocity = Vector3.zero;         //플레이어 속도
    protected Vector3 forward = Vector3.zero;          //플레이어 정면
    protected Vector3 right = Vector3.zero;            //플레이어 좌우
    protected Vector2 direction = Vector2.zero;        //플레이어 이동

    [Header("PlayerStats")]
    protected PlayerStat stat = new PlayerStat();
    #endregion
    protected void UpdatePlayer()
    {
        if (dieStatus || interactionStatus) return;
        Move();
        Rotate();
    }   //플레이어의 공통된 움직임 함수 작동

    //플레이어 데이터 설정
    #region InitData
    public abstract void Initialize();

    protected void SetPlayerData()
    {
        stat.SetData(data);
        stat.AddMoney(1000);  
        uiManager().GetPlayerUI.SetMoneyText(stat.Money);
        uiManager().GetPlayerUI.GetActionPanel.SetPlayerUIStat(stat);
    }   //플레이어 스텟 적용

    protected void SetSkill()
    {
        for (int i = 0; i < data.SkillDataSize; ++i)
        {
            skill[i] = Instantiate(data.SkillData[i].Skill);    //플레이어 스킬소유
            skill[i].gameObject.transform.SetParent(playerTrans);
            skill[i].Initialize(data.SkillData[i]);             //스킬 데이터 삽입
        }
    }   //스킬 설정

    public void Save()
    {
        GameManager.Instance.SaveLoadManager.GetPlayerData().Save(stat);
    }

    public void Load(PlayerData data)
    {
        stat.Load(data);
        uiManager().GetPlayerUI.SetMoneyText(stat.Money);
        uiManager().GetPlayerUI.GetActionPanel.SetPlayerUIStat(stat);
        dieStatus = false;
        anim.Play("Idle_Battle_SwordAndShield", 0);
    }
    #endregion

    //플레이어 인풋엑션 : 플레이어 키를 눌렀을 때 함수 호출
    #region InputAction
    public void InputMove(InputAction.CallbackContext context)
    {
        //애니메이션 설정
        direction = context.ReadValue<Vector2>();
        anim.SetBool("SetMove", true);
        anim.SetFloat("MoveFB", direction.y);
        anim.SetFloat("MoveRL", direction.x);
        audioSource.PlayOneShot(soundData.GetAudioClip(SFXPlayerSoundType.Move), GameManager.Instance.GetSoundManager.EffectVolume);
    }   //플레이어 움직임
    public void InputStopMove(InputAction.CallbackContext context)
    {
        anim.SetBool("SetMove", false);
        direction = Vector2.zero;
    }   //플레이어 멈출때 초기화
    public abstract void InputM1(InputAction.CallbackContext context);
    public abstract void InputStopM1(InputAction.CallbackContext context);
    public abstract void InputM2(InputAction.CallbackContext context);
    public abstract void InputStopM2(InputAction.CallbackContext context);

    public void InputJumpSpace(InputAction.CallbackContext context)
    {
        //땅에 있을 때
        if (isGround) 
        {
            isGround = false;
            rigid.AddForce(Vector3.up * 50.0f, ForceMode.Impulse);
            audioSource.PlayOneShot(soundData.GetAudioClip(SFXPlayerSoundType.Jump), GameManager.Instance.GetSoundManager.EffectVolume);
        }
    }   //점프 힘 적용
    public void InputInteraction(InputAction.CallbackContext context)
    {
        if (interactionObject != null)//상호작용 오브젝트 일 때
        {
            interactionObject.InteractionPlayer();            //오브젝트 상호작용
            uiManager().GetPlayerUI.ActiveItemText(false);    //UI 텍스트 출력
        }
    }   //플레이어 상호작용
    public void InputActiveInventoryPanel(InputAction.CallbackContext context)
    {
        GameManager.Instance.GetQuestManager().UpdateQuest();       //구독한 퀘스트 업데이트
        uiManager().GetPlayerUI.PlayerPanel.SetActive(true);
        interactionStatus = true;
    }    //인벤토리 활성화
    public void InputDisableInventoryPanel(InputAction.CallbackContext context)
    {
        uiManager().GetPlayerUI.PlayerPanel.SetActive(false);
        interactionStatus = false;
    }   //인벤토리 비활성화
    public abstract void SkillQ(InputAction.CallbackContext context);
    public abstract void SkillE(InputAction.CallbackContext context);
    public abstract void SkillR(InputAction.CallbackContext context);
    public void GameOptionKey(InputAction.CallbackContext context)
    {
        GameManager.Instance.GetUIManager().GetPlayerUI.GetgameOptionPanel.SetActive(!GameManager.Instance.GetUIManager().GetPlayerUI.GetgameOptionPanel.gameObject.activeSelf);
        GameManager.Instance.SetInputDisable();
    }   //인게임 옵션창
    #endregion

    //플레이어 로직 : 움직임 회전 공격 연산
    #region Rogic
    protected bool PlaySkill(SkillKey key)
    {
        //같은스킬 쿨타임 일 시 false
        if (interactionStatus || skill[(int)key].PlayCheck()) return false;

        int damage = stat.Damage + (int)(Random.Range(1.0f, stat.CriticalRate) * stat.CriticalDamage);
        skill[(int)key].Play(playerTrans.position, damage, playerTrans.forward, playerTrans.rotation);
        return true;
    }
    private void Move()
    {
        forward = playerTrans.forward * direction.y;    //위아래
        right = playerTrans.right * direction.x;        //좌우
        velocity = forward + right;                         //x, y 축 이동
        velocity = velocity.normalized;

        if(!isGround)  //점프 상태일때
        {
            velocity += Vector3.down * 0.5f;
        }
        else if(SlopeCheck())
        {
            velocity = Vector3.ProjectOnPlane(velocity, slopeHit.normal);
        }

        rigid.velocity = velocity * stat.MoveSpeed;   //속도 * 이동속도 * 달리기 속도
    }   //플레이어 움직임
    private void Rotate()
    {
        rotateYaw += Input.GetAxis("Mouse X") * 1.3f;
        rotatePitch -= Input.GetAxis("Mouse Y");
        rotateYaw = ClampAngle(rotateYaw, -360, 360);
        rotatePitch = ClampAngle(rotatePitch, -20, 70);

        targetCamera.SetRotate(rotatePitch, rotateYaw);
        playerTrans.rotation = Quaternion.Euler(0.0f, rotateYaw, 0.0f);           //플레이어 y축 회전
    } //플레이어 Y축 회전, X축 회전 지오메트리 전달
    private bool SlopeCheck()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight /2 + 0.5f) && slopeHit.normal != Vector3.up)
        {
            return true;
        }
        return false;
    }
    private float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }   //각도 값 제한
    public void SetYaw(Vector3 rotate)
    {
        rotateYaw = rotate.y;
    }
    #endregion

    //플레이어 계산 식 : protected
    #region carculate
    public abstract void Damaged(int enemyDamege);    //플레이어가 맞았을 때 호출

    public void AddExe(int exe)
    {
        stat.AddExe(exe);
        uiManager().GetPlayerUI.GetActionPanel.SetPlayerUIStat(stat);
        stat.AddMoney(100);
        uiManager().GetPlayerUI.SetMoneyText(stat.Money);
    }   //플레이어 경험치 획득
    #endregion

    //플레이어 상호작용
    #region Interaction
    public void SetGeometry() { GameManager.Instance.GetCameraController.SetTarget(targetCamera.transform); }
    public void UseItem(Item item, int count)
    {
        if (item.Type == ItemType.Artifact) audioSource.PlayOneShot(soundData.GetAudioClip(SFXPlayerSoundType.GetItem));
        stat.UseItem(item, count);
        uiManager().GetPlayerUI.GetActionPanel.SetPlayerUIStat(stat);
    }   //아이템 타입에 맞게 스텟 추가
    public void BuyItem(int price)
    {
        stat.Buy(price);
        uiManager().GetPlayerUI.SetMoneyText(stat.Money);
    }
    public bool BuyCheck(int price)
    {
        return stat.Money - price > 0 ? true : false;
    }   //구매가 가능한지 체크
    public void SetJumpStatus()
    {
        isGround = false;
    }

    public void AddQuest(int key)
    {
        if(GameManager.Instance.GetQuestManager().AddQuest(key))    //키가 존제 할 경우 퀘스트 생성
        {
            GameManager.Instance.GetQuestManager().AccetpQuest[key].Add(this);      //퀘스트 구독
        }
        
        //수락 시 옵저버 퀘스트 전송
        //퀘스트 패널 UI key 전송
        GameManager.Instance.GetUIManager().GetPlayerUI.GetPlayerPanel.GetAcceptQuest.AddQuestSlot(key);
    }   //퀘스트 구독

    public void RemoveQuest(int key)
    {
        GameManager.Instance.GetQuestManager().AccetpQuest[key].Remove(this);       //구독 취소
        AddExe(GameManager.Instance.GetQuestManager().QuestDB.GetReward(key));      //플레이어 보상
    }   //구독 취소 및 보상

    public override void UpdateObserver(int questKey)
    {
        GameManager.Instance.GetUIManager().GetPlayerUI.GetPlayerPanel.GetAcceptQuest.UpdateCurrentQuestText(questKey);     //퀘스트 패널 텍스트 업데이트
    }   //옵저버 업데이트

    public void InteractionType(QuestAddValueType type)
    {
        GameManager.Instance.GetQuestManager().AddCount(type);    //타입에 맞을 때 카운트 증가
    }   //상호작용 타입에 따른 값 변경
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        //상호작용 오브젝트
        if(other.tag.Equals("Interaction"))
        {
            interactionObject = other.GetComponent<InteractionObject>();
            uiManager().GetPlayerUI.ActiveItemText(interactionObject.InteractionStatus);
        }
    }   //트리거 들어 왔을때

    private void OnTriggerExit(Collider other)
    {
        //상호작용 비우는 작업
        if (other.tag.Equals("Interaction"))
        {
            uiManager().GetPlayerUI.ActiveItemText(false);
            interactionObject = null;
        }
    }   //트리거 나갈 때

    private void OnCollisionStay(Collision collision)
    {
        //땅에 닿아 있을 때
        if (collision.gameObject.tag.Equals("Ground"))
            isGround = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        //땅에 없을 때
        if (collision.gameObject.tag.Equals("Ground"))
            isGround = false;
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum SceneLoad
{
    MAIN,
    STAGE1,
    STAGE2,
    STAGE3,
    RETRY,
    Default,
}

public class GameManager : MonoBehaviour
{
    static GameManager instance = null;
    [Header("Input")]
    [SerializeField] PlayerInput input;

    [Header("Player")]
    private Player player = null;                   //플레이어 스크립트

    [Header("Camera")]
    private ThirdPointCameraController cameraController = null;

    private UIManager uiManager = null;                    //UIManager
    private DamageTextPool damageTextPool = null;          //데미지 텍스트풀
    [SerializeField] EnemyPool enemyPool;                  //오브젝트 풀
    private SpawnerManager spawnerManager = null;          //아이템 스포너 관리
    private ItemManager itemManager = null;                //아이템 매니저
    private Curve curve = new Curve();                     //커브 계산
    private SceneLoad currentScene;                        //최근변경된 씬 enum값

    [Header("Quest")]
    private QuestManager questManager = null;
    [Header("Audio")]
    private SoundManager soundManager = null;

    [Header("Teleporter")]
    [SerializeField] Teleporter teleporter = null;

    [Header("SaveLoad")]
    [SerializeField] SaveLoadManager saveLoadManager = null;

    [Header("Loading")]
    [SerializeField] LoadingSceneManager loadingSceneManager = null;
    #region Call
    public void CallDamageTextPool(int damage, Vector3 StartPos)
    {
        damageTextPool.PlayDamageText(damage, StartPos);
    }
    //호출부분
    public Vector3[] GetPath(Vector3 start, Vector3 end, float height)
    {
        curve.CalculateLaunch(start, end, height);
        return curve.GetPath();
    }   //포물선 계산 추후 이벤트
    public Player GetPlayer() { return player; }
    public UIManager GetUIManager() { return uiManager; }             //UI매니저 호출
    public SoundManager GetSoundManager => soundManager;    //사운드 메니저 호출
    public QuestManager GetQuestManager() { return questManager; }   //퀘스트 매니저 호출
    public ItemManager GetItemManager => itemManager;       //아이템 매니저 호출
    public EnemyPool GetEnemyPool => enemyPool;
    public SpawnerManager GetspawnerManager => spawnerManager;
    public Teleporter GetTeleporter => teleporter;
    public SaveLoadManager SaveLoadManager => saveLoadManager;
    public ThirdPointCameraController GetCameraController => cameraController;
    #endregion
    #region Init()
    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                DontDestroyOnLoad(instance);
            }
            return instance;
        }
    }   //싱글톤 처리

    private void Start()
    {
        //인스턴스와 비교
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Intialize();
    }

    private void Intialize()
    {
        cameraController = GetComponentInChildren<ThirdPointCameraController>();
        cameraController.Initialize();
        uiManager = GetComponentInChildren<UIManager>();

        //퀘스트 메니저
        questManager = GetComponentInChildren<QuestManager>();
        soundManager = GetComponentInChildren<SoundManager>();

        soundManager.BGMPlayLoop(SFXBGMType.MainMenu);

        damageTextPool = GetComponentInChildren<DamageTextPool>();      //데미지 텍스트 풀
        damageTextPool.Initialize();

        itemManager = GetComponentInChildren<ItemManager>();
        itemManager.Initialize();

        questManager.Initialize();
        SceneManager.sceneLoaded += OnSceneLoaded;
        uiManager.GetPlayerUI.SetActive(false);
        uiManager.GetEnemyUI.SetActive(false);
        FindObjectOfType<MenuUI>().GetSaveLoadPanel.Initialize();

        currentScene = SceneLoad.MAIN;
    }
    public void SetInput()
    {
        input.SwitchCurrentActionMap("Player");
        input.actions["Move"].performed += player.InputMove;            //WASD 키 누를 시 실행
        input.actions["Move"].canceled += player.InputStopMove;         //WASD 키 땔 시 실행
        input.actions["AttackM1"].performed += player.InputM1;          //마우스 좌클릭시 실행
        input.actions["AttackM1"].canceled += player.InputStopM1;       //마우스 좌클릭시 땔때 실행
        input.actions["AttackM2"].performed += player.InputM2;          //마우스 우클릭시 실행
        input.actions["AttackM2"].canceled += player.InputStopM2;       //마우스 우클릭 땔때 실행
        input.actions["Interaction"].started += player.InputInteraction;//플레이어 상호작용 키
        input.actions["ArtifactInventory"].performed += player.InputActiveInventoryPanel;    //Tap 키 누를시 실행
        input.actions["ArtifactInventory"].canceled += player.InputDisableInventoryPanel;    //Tap 키 땔시 실행
        input.actions["Jump"].started += player.InputJumpSpace;         //스페이스키 누를때 점프
        input.actions["SkillQ"].started += player.SkillQ;
        input.actions["SkillE"].started += player.SkillE;
        input.actions["SkillR"].started += player.SkillR;
        input.actions["Option"].started += player.GameOptionKey;
    }       //플레이어의 인풋 설정
    public void DeleteInput()
    {
        if (!input.inputIsActive) return;
        input.actions["Move"].performed -= player.InputMove;            //WASD 키 누를 시 실행
        input.actions["Move"].canceled -= player.InputStopMove;         //WASD 키 땔 시 실행
        input.actions["AttackM1"].performed -= player.InputM1;            //마우스 좌클릭시 실행
        input.actions["AttackM1"].canceled -= player.InputStopM1;       //마우스 좌클릭시 땔때 실행
        input.actions["AttackM2"].performed -= player.InputM2;          //마우스 우클릭시 실행
        input.actions["AttackM2"].canceled -= player.InputStopM2;       //마우스 우클릭 땔시 실행
        input.actions["Interaction"].started -= player.InputInteraction;//플레이어 상호작용 키
        input.actions["ArtifactInventory"].performed -= player.InputActiveInventoryPanel;    //Tap 키 누를시 실행
        input.actions["ArtifactInventory"].canceled -= player.InputDisableInventoryPanel;    //Tap 키 땔시 실행
        input.actions["Jump"].started -= player.InputJumpSpace;         //스페이스키 누를때 점프
        input.actions["SkillQ"].started -= player.SkillQ;
        input.actions["SkillE"].started -= player.SkillE;
        input.actions["SkillR"].started -= player.SkillR;
        input.actions["Option"].started -= player.GameOptionKey;
        input.currentActionMap.Disable();
    }    //인풋 설정 제거
    public void SetInputEnable() { input.currentActionMap.Enable(); }   //인풋 활성화
    public void SetInputDisable() { input.currentActionMap.Disable(); } //인풋 비활성화
    #endregion

    public void ResetMap(bool NextIsLobby = false)
    {
        if (enemyPool != null) { enemyPool.AllReturnActiveMonster(); }      //활성화된 몬스터 제거
        if (spawnerManager != null)
        {
            spawnerManager.ResetSpawner();          //스포너 초기화
            spawnerManager = null;
        }

        if (NextIsLobby)
        {
            uiManager.GetPlayerUI.GetPlayerPanel.GetAcceptQuest.ClearSlot();    //퀘스트 패널 초기화
            questManager.AccetpQuest.Clear();
            uiManager.GetPlayerUI.GetPlayerPanel.GetInventory.ClearSlot();  //인벤토리 지우기
            uiManager.GetPlayerUI.SetActive(false);
            uiManager.GetEnemyUI.SetActive(false);
        }
        else
        {
            uiManager.GetPlayerUI.SetActive(true);
            uiManager.GetEnemyUI.SetActive(true);
        }
    }   //다음 씬으로 가기전 초기화 작업
    public void StartGame()
    {
        GameObject worrior = Instantiate(Resources.Load<GameObject>("Prefabs/Player/Warrior"));
        worrior.transform.SetParent(this.gameObject.transform, false);
        player = worrior.GetComponent<Warrior>();
        player.uiManager += GetUIManager;           //UI 호출 델리게이트
        player.questManager += GetQuestManager;
        player.Initialize();
        SetInput();
        saveLoadManager.InitData();     //데이터 삽입
        ChangeScene((SceneLoad)Random.Range(1, (int)SceneLoad.RETRY));
    }   //게임 시작

    /// <summary>
    /// SceneLoad : 변경할 씬 타입
    /// </summary>
    public void ChangeScene(SceneLoad sceneType)
    {
        if (sceneType != SceneLoad.RETRY)
        {
            currentScene = sceneType;
        }
        else
        {
            saveLoadManager.InitData();
        }

        switch (currentScene)
        {
            case SceneLoad.MAIN:
                ResetMap(true);
                soundManager.BGMPlayLoop(SFXBGMType.MainMenu);
                break;
            case SceneLoad.STAGE1:
                ResetMap();
                soundManager.BGMPlayLoop(SFXBGMType.Stage1);
                SaveData();
                break;
            case SceneLoad.STAGE2:
                ResetMap();
                soundManager.BGMPlayLoop(SFXBGMType.Stage2);
                SaveData();
                break;
            case SceneLoad.STAGE3:
                ResetMap();
                soundManager.BGMPlayLoop(SFXBGMType.Stage3);
                SaveData();
                break;
        }
        loadingSceneManager.gameObject.SetActive(true);
        loadingSceneManager.StartCoroutine(currentScene);
    }   //씬 변경 및 초기화
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        loadingSceneManager.StopLoadingCoroutine();
        if (scene.name == "Main")
        {
            DeleteInput();
            if (player != null) { Destroy(player.gameObject); }
            player = null;
            spawnerManager = null;
            FindObjectOfType<MenuUI>().GetSaveLoadPanel.Initialize();
        }
        else
        {
            uiManager.GetPlayerUI.SetActive(true);
            uiManager.GetEnemyUI.SetActive(true);
            enemyPool = FindObjectOfType<EnemyPool>();
            enemyPool.Initialize();
            spawnerManager = FindObjectOfType<SpawnerManager>();            //필드의 스포너 매니저
            spawnerManager.SetSpawner(Random.Range(3, 6));                  //3~5마리 랜덤 스폰
        }
    }   //씬 로드 이후 실행
    private void SaveData()
    {
        player.Save();
        uiManager.GetPlayerUI.PlayerPanel.GetInventory.Save();
        questManager.Save();
        SaveLoadManager.SaveFile();
    }
}
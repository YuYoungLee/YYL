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
    private Player player = null;                   //�÷��̾� ��ũ��Ʈ

    [Header("Camera")]
    private ThirdPointCameraController cameraController = null;

    private UIManager uiManager = null;                    //UIManager
    private DamageTextPool damageTextPool = null;          //������ �ؽ�ƮǮ
    [SerializeField] EnemyPool enemyPool;                  //������Ʈ Ǯ
    private SpawnerManager spawnerManager = null;          //������ ������ ����
    private ItemManager itemManager = null;                //������ �Ŵ���
    private Curve curve = new Curve();                     //Ŀ�� ���
    private SceneLoad currentScene;                        //�ֱٺ���� �� enum��

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
    //ȣ��κ�
    public Vector3[] GetPath(Vector3 start, Vector3 end, float height)
    {
        curve.CalculateLaunch(start, end, height);
        return curve.GetPath();
    }   //������ ��� ���� �̺�Ʈ
    public Player GetPlayer() { return player; }
    public UIManager GetUIManager() { return uiManager; }             //UI�Ŵ��� ȣ��
    public SoundManager GetSoundManager => soundManager;    //���� �޴��� ȣ��
    public QuestManager GetQuestManager() { return questManager; }   //����Ʈ �Ŵ��� ȣ��
    public ItemManager GetItemManager => itemManager;       //������ �Ŵ��� ȣ��
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
    }   //�̱��� ó��

    private void Start()
    {
        //�ν��Ͻ��� ��
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

        //����Ʈ �޴���
        questManager = GetComponentInChildren<QuestManager>();
        soundManager = GetComponentInChildren<SoundManager>();

        soundManager.BGMPlayLoop(SFXBGMType.MainMenu);

        damageTextPool = GetComponentInChildren<DamageTextPool>();      //������ �ؽ�Ʈ Ǯ
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
        input.actions["Move"].performed += player.InputMove;            //WASD Ű ���� �� ����
        input.actions["Move"].canceled += player.InputStopMove;         //WASD Ű �� �� ����
        input.actions["AttackM1"].performed += player.InputM1;          //���콺 ��Ŭ���� ����
        input.actions["AttackM1"].canceled += player.InputStopM1;       //���콺 ��Ŭ���� ���� ����
        input.actions["AttackM2"].performed += player.InputM2;          //���콺 ��Ŭ���� ����
        input.actions["AttackM2"].canceled += player.InputStopM2;       //���콺 ��Ŭ�� ���� ����
        input.actions["Interaction"].started += player.InputInteraction;//�÷��̾� ��ȣ�ۿ� Ű
        input.actions["ArtifactInventory"].performed += player.InputActiveInventoryPanel;    //Tap Ű ������ ����
        input.actions["ArtifactInventory"].canceled += player.InputDisableInventoryPanel;    //Tap Ű ���� ����
        input.actions["Jump"].started += player.InputJumpSpace;         //�����̽�Ű ������ ����
        input.actions["SkillQ"].started += player.SkillQ;
        input.actions["SkillE"].started += player.SkillE;
        input.actions["SkillR"].started += player.SkillR;
        input.actions["Option"].started += player.GameOptionKey;
    }       //�÷��̾��� ��ǲ ����
    public void DeleteInput()
    {
        if (!input.inputIsActive) return;
        input.actions["Move"].performed -= player.InputMove;            //WASD Ű ���� �� ����
        input.actions["Move"].canceled -= player.InputStopMove;         //WASD Ű �� �� ����
        input.actions["AttackM1"].performed -= player.InputM1;            //���콺 ��Ŭ���� ����
        input.actions["AttackM1"].canceled -= player.InputStopM1;       //���콺 ��Ŭ���� ���� ����
        input.actions["AttackM2"].performed -= player.InputM2;          //���콺 ��Ŭ���� ����
        input.actions["AttackM2"].canceled -= player.InputStopM2;       //���콺 ��Ŭ�� ���� ����
        input.actions["Interaction"].started -= player.InputInteraction;//�÷��̾� ��ȣ�ۿ� Ű
        input.actions["ArtifactInventory"].performed -= player.InputActiveInventoryPanel;    //Tap Ű ������ ����
        input.actions["ArtifactInventory"].canceled -= player.InputDisableInventoryPanel;    //Tap Ű ���� ����
        input.actions["Jump"].started -= player.InputJumpSpace;         //�����̽�Ű ������ ����
        input.actions["SkillQ"].started -= player.SkillQ;
        input.actions["SkillE"].started -= player.SkillE;
        input.actions["SkillR"].started -= player.SkillR;
        input.actions["Option"].started -= player.GameOptionKey;
        input.currentActionMap.Disable();
    }    //��ǲ ���� ����
    public void SetInputEnable() { input.currentActionMap.Enable(); }   //��ǲ Ȱ��ȭ
    public void SetInputDisable() { input.currentActionMap.Disable(); } //��ǲ ��Ȱ��ȭ
    #endregion

    public void ResetMap(bool NextIsLobby = false)
    {
        if (enemyPool != null) { enemyPool.AllReturnActiveMonster(); }      //Ȱ��ȭ�� ���� ����
        if (spawnerManager != null)
        {
            spawnerManager.ResetSpawner();          //������ �ʱ�ȭ
            spawnerManager = null;
        }

        if (NextIsLobby)
        {
            uiManager.GetPlayerUI.GetPlayerPanel.GetAcceptQuest.ClearSlot();    //����Ʈ �г� �ʱ�ȭ
            questManager.AccetpQuest.Clear();
            uiManager.GetPlayerUI.GetPlayerPanel.GetInventory.ClearSlot();  //�κ��丮 �����
            uiManager.GetPlayerUI.SetActive(false);
            uiManager.GetEnemyUI.SetActive(false);
        }
        else
        {
            uiManager.GetPlayerUI.SetActive(true);
            uiManager.GetEnemyUI.SetActive(true);
        }
    }   //���� ������ ������ �ʱ�ȭ �۾�
    public void StartGame()
    {
        GameObject worrior = Instantiate(Resources.Load<GameObject>("Prefabs/Player/Warrior"));
        worrior.transform.SetParent(this.gameObject.transform, false);
        player = worrior.GetComponent<Warrior>();
        player.uiManager += GetUIManager;           //UI ȣ�� ��������Ʈ
        player.questManager += GetQuestManager;
        player.Initialize();
        SetInput();
        saveLoadManager.InitData();     //������ ����
        ChangeScene((SceneLoad)Random.Range(1, (int)SceneLoad.RETRY));
    }   //���� ����

    /// <summary>
    /// SceneLoad : ������ �� Ÿ��
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
    }   //�� ���� �� �ʱ�ȭ
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
            spawnerManager = FindObjectOfType<SpawnerManager>();            //�ʵ��� ������ �Ŵ���
            spawnerManager.SetSpawner(Random.Range(3, 6));                  //3~5���� ���� ����
        }
    }   //�� �ε� ���� ����
    private void SaveData()
    {
        player.Save();
        uiManager.GetPlayerUI.PlayerPanel.GetInventory.Save();
        questManager.Save();
        SaveLoadManager.SaveFile();
    }
}
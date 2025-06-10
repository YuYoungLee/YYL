using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Monster
{
    Monster_Grunt,
    Monster_SpiderKing,
    Monster_Size,
    Monster_Boss,        //���� ���� �϶� ó��
}   //���� ����

public class EnemyPool : MonoBehaviour
{
    private Dictionary<Monster, Queue<Enemy>> obejctPool = new Dictionary<Monster, Queue<Enemy>>(); //������ ������Ʈ Ǯ
    private Queue<EnemyHealthBar> enemyHealthBarPool = new Queue<EnemyHealthBar>();     //������ ü�¹� Ǯ
    private List<Enemy> activeEnemy = new List<Enemy>();
    [SerializeField] GameObject canvas;                             //uicanvas

    private readonly int monsterCreateMax = 10;           //���� ������ ������ ����
    #region Init
    public void Initialize()
    {
        if (enemyHealthBarPool.Count > 0) return;
        for (int i = 0; i < (int)Monster.Monster_Size; i++)
        {
            obejctPool[(Monster)i] = new Queue<Enemy>();        //�ش� Ű�� Queue ����
            AddPool((Monster)i, LoadPrefab((Monster)i));        //���� Ǯ�� �߰�
        }

        CreateHealthBar(10);
        InitBossMonster();
    }   //���� ���� ����ŭ ������Ʈ Ǯ ü���
    private void InitBossMonster()
    {
        obejctPool[Monster.Monster_Boss] = new Queue<Enemy>();
        Enemy boss = Resources.Load<Dragon>("Prefabs/Enemy/GreenDragon");
        boss = Instantiate(boss);
        boss.transform.SetParent(this.gameObject.transform, false);
        boss.Initialize(Monster.Monster_Boss);
        obejctPool[Monster.Monster_Boss].Enqueue(boss);
    }   //���� ���� ������ ����
    private Enemy LoadPrefab(Monster monsterType)
    {
        Enemy monster = null;
        switch(monsterType)
        {
            case Monster.Monster_Grunt:
                monster = Resources.Load<Grunt>("Prefabs/Enemy/Grunt");
                break;
            case Monster.Monster_SpiderKing:
                monster = Resources.Load<Spider>("Prefabs/Enemy/Spider King");
                break;
        }
        //���Ͱ� null�� ��� ���� �߻�
        if (monster == null) { Debug.LogError("EnemyPool.AddMonster() : Enemy monster is null"); }
        return monster;
    }   //�ش� ���Ϳ� �´� ������ �ε� ����
    private void AddPool(Monster monsterType, Enemy monster)
    {
        Enemy prefab = null;
        for(int i = 0; i < monsterCreateMax; ++i)
        {
            prefab = Instantiate(monster);                  //������ ����
            prefab.transform.SetParent(this.gameObject.transform);     //�θ� ����
            prefab.Initialize(monsterType);    //�ʱ�ȭ ����
            obejctPool[monsterType].Enqueue(prefab);        //Ǯ�� ����
            prefab = null;
        }
    }   //���� ���� �� Ǯ�� ����
    #endregion

    public void SpawnMonster(Monster monsterType, Vector3 pos)
    {
        if(obejctPool[monsterType].Count == 0)
        {
            AddPool(monsterType, LoadPrefab(monsterType));        //���� Ǯ�� �߰�
        }
        Enemy enemy = obejctPool[monsterType].Dequeue();
        activeEnemy.Add(enemy);
        EnemyHealthBar healthBar = null;
        if (monsterType != Monster.Monster_Boss) healthBar = GetHealthBar();

        enemy.SetActive(GetSpawnPosition(pos), healthBar, GameManager.Instance.GetPlayer().GetComponent<Rigidbody>());    //��ȯ�� ��ġ, ü�¹� ����
    }   //���� ����

    /// <summary>
    /// monsterType : ���� Ÿ��, ���� ���� �� �� Monster.Monster_Boss ����
    /// monster : ���� �ڱ��ڽ� this
    /// </summary>
    public void ReturnPool(Monster monsterType, Enemy monster)
    {
        activeEnemy.Remove(monster);                //Ȱ��ȭ ���� ��󿡼� ����
        obejctPool[monsterType].Enqueue(monster);   //������Ʈ Ǯ�� ����
    }   //Ǯ�� �ٽ� ����

    private void CreateHealthBar(int count)
    {
        EnemyHealthBar hpBar = Resources.Load<EnemyHealthBar>("Prefabs/UI/EnemyUI/EnemyHealthBar");
        for (int i = 0; i < count; ++i)
        {
            EnemyHealthBar createHpBar = Instantiate(hpBar);
            createHpBar.transform.SetParent(canvas.transform, false);
            createHpBar.Initialize();
            createHpBar.gameObject.SetActive(false);
            enemyHealthBarPool.Enqueue(createHpBar);
            createHpBar.returnPool += () => { ReturnHelathBarPool(createHpBar); };
        }
    }   //ü�¹� ���� ��ŭ ����

    public EnemyHealthBar GetHealthBar()
    {
        //������ 5���� ����
        if(enemyHealthBarPool.Count < 1)
        {
            CreateHealthBar(5);
        }
        return enemyHealthBarPool.Dequeue();
    }

    public void ReturnHelathBarPool(EnemyHealthBar healthBar)
    {
        healthBar.StopHitCoroutine();
        enemyHealthBarPool.Enqueue(healthBar);
    }   //ü�¹� Ǯ�� ���ư���

    Vector3 GetSpawnPosition(Vector3 pos)
    {
        //���� �ݰ� ���̷�
        Vector2 randomPos = Random.insideUnitCircle.normalized * Random.Range(4, 8);
        NavMesh.SamplePosition(pos + new Vector3(randomPos.x, 0, randomPos.y), 
            out NavMeshHit nav, 100f, NavMesh.AllAreas);
        return nav.position;
    }   //������ ������ ��ġ�� ��ȯ

    public void AllReturnActiveMonster()
    {
        while(activeEnemy.Count > 0)
        {
            activeEnemy[0].SetDisable();
        }
    }
}

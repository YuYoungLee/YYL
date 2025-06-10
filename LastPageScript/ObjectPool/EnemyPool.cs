using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Monster
{
    Monster_Grunt,
    Monster_SpiderKing,
    Monster_Size,
    Monster_Boss,        //보스 몬스터 일때 처리
}   //몬스터 종류

public class EnemyPool : MonoBehaviour
{
    private Dictionary<Monster, Queue<Enemy>> obejctPool = new Dictionary<Monster, Queue<Enemy>>(); //몬스터의 오브젝트 풀
    private Queue<EnemyHealthBar> enemyHealthBarPool = new Queue<EnemyHealthBar>();     //몬스터의 체력바 풀
    private List<Enemy> activeEnemy = new List<Enemy>();
    [SerializeField] GameObject canvas;                             //uicanvas

    private readonly int monsterCreateMax = 10;           //몬스터 종류당 생성할 갯수
    #region Init
    public void Initialize()
    {
        if (enemyHealthBarPool.Count > 0) return;
        for (int i = 0; i < (int)Monster.Monster_Size; i++)
        {
            obejctPool[(Monster)i] = new Queue<Enemy>();        //해당 키의 Queue 생성
            AddPool((Monster)i, LoadPrefab((Monster)i));        //몬스터 풀에 추가
        }

        CreateHealthBar(10);
        InitBossMonster();
    }   //몬스터 종류 수만큼 오브젝트 풀 체우기
    private void InitBossMonster()
    {
        obejctPool[Monster.Monster_Boss] = new Queue<Enemy>();
        Enemy boss = Resources.Load<Dragon>("Prefabs/Enemy/GreenDragon");
        boss = Instantiate(boss);
        boss.transform.SetParent(this.gameObject.transform, false);
        boss.Initialize(Monster.Monster_Boss);
        obejctPool[Monster.Monster_Boss].Enqueue(boss);
    }   //보스 몬스터 데이터 삽입
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
        //몬스터가 null일 경우 오류 발생
        if (monster == null) { Debug.LogError("EnemyPool.AddMonster() : Enemy monster is null"); }
        return monster;
    }   //해당 몬스터에 맞는 프리펩 로드 리턴
    private void AddPool(Monster monsterType, Enemy monster)
    {
        Enemy prefab = null;
        for(int i = 0; i < monsterCreateMax; ++i)
        {
            prefab = Instantiate(monster);                  //프리펩 복제
            prefab.transform.SetParent(this.gameObject.transform);     //부모 설정
            prefab.Initialize(monsterType);    //초기화 설정
            obejctPool[monsterType].Enqueue(prefab);        //풀에 삽입
            prefab = null;
        }
    }   //몬스터 생성 후 풀에 삽입
    #endregion

    public void SpawnMonster(Monster monsterType, Vector3 pos)
    {
        if(obejctPool[monsterType].Count == 0)
        {
            AddPool(monsterType, LoadPrefab(monsterType));        //몬스터 풀에 추가
        }
        Enemy enemy = obejctPool[monsterType].Dequeue();
        activeEnemy.Add(enemy);
        EnemyHealthBar healthBar = null;
        if (monsterType != Monster.Monster_Boss) healthBar = GetHealthBar();

        enemy.SetActive(GetSpawnPosition(pos), healthBar, GameManager.Instance.GetPlayer().GetComponent<Rigidbody>());    //소환할 위치, 체력바 소유
    }   //몬스터 스폰

    /// <summary>
    /// monsterType : 몬스터 타입, 보스 몬스터 일 때 Monster.Monster_Boss 지정
    /// monster : 몬스터 자기자신 this
    /// </summary>
    public void ReturnPool(Monster monsterType, Enemy monster)
    {
        activeEnemy.Remove(monster);                //활성화 관리 대상에서 제거
        obejctPool[monsterType].Enqueue(monster);   //오브젝트 풀로 삽입
    }   //풀로 다시 삽입

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
    }   //체력바 갯수 만큼 생성

    public EnemyHealthBar GetHealthBar()
    {
        //없을때 5개씩 생성
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
    }   //체력바 풀로 돌아가기

    Vector3 GetSpawnPosition(Vector3 pos)
    {
        //원의 반경 사이로
        Vector2 randomPos = Random.insideUnitCircle.normalized * Random.Range(4, 8);
        NavMesh.SamplePosition(pos + new Vector3(randomPos.x, 0, randomPos.y), 
            out NavMeshHit nav, 100f, NavMesh.AllAreas);
        return nav.position;
    }   //스폰할 지역의 위치를 반환

    public void AllReturnActiveMonster()
    {
        while(activeEnemy.Count > 0)
        {
            activeEnemy[0].SetDisable();
        }
    }
}

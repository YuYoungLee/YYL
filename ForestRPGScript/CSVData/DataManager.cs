using System;
using System.Collections.Generic;

public enum DataType
{
    None,
    Skill,      //스킬타입의 데이터
    Item,       //아이템 타입의 데이터
}   //데이터 타입

public class DataManager
{
    private Dictionary<int, SkillData> mSkillData;          //스킬 데이터
    private Dictionary<int, ItemData> mItemData;            //아이템 데이터
    private List<PlayerData> mPlayerData;                   //플레이어 데이터
    private List<IState> mStateData;                        //상태 데이터
    private Dictionary<int, EnemyData> mEnemyData;          //에너미 몬스터 데이터
    private Dictionary<int, QuestData> mQuestData;          //퀘스트 데이터
    private Dictionary<int, NPCData> mNPCData;              //NPC 데이터

    private Dictionary<int, Table<Tuple<int, int>>> mReinforceTableData;    //강화 테이블 데이터
    private Dictionary<int, Table<Tuple<int, int>>> mRewardItemTableData;     //보상 아이템 테이블
    private Dictionary<int, Table<Tuple<int, int>>> mEnemyTableData;    //몬스터 데이터
    private Dictionary<int, Table<Tuple<int>>> mDropTableData;    //드랍 아이템 데이터
    private Dictionary<int, Table<Tuple<int>>> mShopTableData;    //상점 데이터
    private Dictionary<int, Table<string>> mTextTableData;      //텍스트 데이터 ex.스토리

    public SkillData GetSkillData(int iKey) => mSkillData[iKey];       //스킬 데이터
    public ItemData GetItemData(int iKey) => mItemData[iKey];          //아이템 데이터
    public PlayerData GetPlayerData(int iKey) => mPlayerData[iKey];    //플레이어 데이터
    public IState GetState(FSMState eState) => mStateData[(int)eState]; //State
    public EnemyData GetEnemyData(int iKey) => mEnemyData[iKey];        //에너미 데이터
    public QuestData GetQuestData(int iKey) => mQuestData[iKey];        //퀘스트 데이터
    public Dictionary<int, QuestData> GetQuest => mQuestData;       //퀘스트
    public NPCData GetNPCData(int iKey) => mNPCData[iKey];      //NPC 데이터

    public Table<Tuple<int, int>> GetReinforceTableData(int iKey) => mReinforceTableData[iKey];
    public Table<Tuple<int, int>> GetRewardTableData(int iKey) => mRewardItemTableData[iKey];
    public Table<Tuple<int, int>> GetEnemyTableData(int iKey) => mEnemyTableData[iKey];
    public Table<Tuple<int>> GetDropTableData(int iKey) => mDropTableData[iKey];
    public Table<Tuple<int>> GetShopTableData(int iKey) => mShopTableData[iKey];

  
    //public Table<int> GetTableData(int iKey) => mKeyTableData[iKey];    //키테이블 데이터
    //public Table<Tuple<int, int>> GetKeyCountData(int iKey) => mKeyCountTableData[iKey];    //키 카운트 데이터
    public Table<string> GetTextTableData(int iKey) => mTextTableData[iKey];    //스트링 테이블 데이터

    public void Initialize()
    {
        //null 이라면 생성
        //스킬의 데이터가 null 일 때
        if(mSkillData == null)
        {
            mSkillData = new Dictionary<int, SkillData>();
            SetSkillData(CSVReader.Instance.LoadCSV("CSVData/SkillData"));
        }
        
        //아이템 데이터가 null 일 때
        if(mItemData == null)
        {
            mItemData = new Dictionary<int, ItemData>();
            SetItemData(CSVReader.Instance.LoadCSV("CSVData/ItemData"));
        }

        //플레이어의 데이터가 null 일 때
        if(mPlayerData == null)
        {
            mPlayerData = new List<PlayerData> { };
            SetPlayerData(CSVReader.Instance.LoadCSV("CSVData/PlayerData"));
        }

        //적들의 데이터가 null 일 때
        if(mEnemyData == null)
        {
            mEnemyData = new Dictionary<int, EnemyData>();
            SetEnemyData(CSVReader.Instance.LoadCSV("CSVData/EnemyData"));
        }

        //FSM state 데이터가 null 일 때
        if(mStateData == null)
        {
            mStateData = new List<IState>
            {
            new IdleState(),
            new MoveState(),
            new AttackState(),
            new ReturnState(),
            new DamagedState(),
            new DeadState(),
            };
        }

        //텍스트 테이블 데이터가 null 일 때
        if(mTextTableData == null)
        {
            mTextTableData = new Dictionary<int, Table<string>>();
            SetStringTableData(CSVReader.Instance.LoadCSV("CSVData/StringTableData"));
        }

        //퀘스트 데이터가 null 일 때
        if(mQuestData == null)
        {
            mQuestData = new Dictionary<int, QuestData>();
            SetQuestData(CSVReader.Instance.LoadCSV("CSVData/QuestData"));
        }

        //npc 데이터가 null 일 때
        if(mNPCData == null)
        {
            mNPCData = new Dictionary<int, NPCData>();
            SetNPCData(CSVReader.Instance.LoadCSV("CSVData/NPCData"));
        }

        if(mReinforceTableData == null)
        {
            mReinforceTableData = new Dictionary<int, Table<Tuple<int, int>>>();
            SetTableKeyCount(CSVReader.Instance.LoadCSV("CSVData/ReinforceTableData"),
                mReinforceTableData);
        }

        //보상 아이템 테이블 데이터
        if(mRewardItemTableData == null)
        {
            mRewardItemTableData = new Dictionary<int, Table<Tuple<int, int>>>();
            SetTableKeyCount(CSVReader.Instance.LoadCSV("CSVData/RewardTableData"),
                mRewardItemTableData);
        }

        //상점 테이블 데이터
        if(mShopTableData == null)
        {
            mShopTableData = new Dictionary<int, Table<Tuple<int>>>();
            SetTableKey(CSVReader.Instance.LoadCSV("CSVData/ShopTableData"),
                mShopTableData);
        }

        //에너미 테이블 데이터
        if(mEnemyTableData == null)
        {
            mEnemyTableData = new Dictionary<int, Table<Tuple<int, int>>>();
            SetTableKeyCount(CSVReader.Instance.LoadCSV("CSVData/EnemyTableData"),
                mEnemyTableData);
        }

        //드랍 테이블 데이터
        if(mDropTableData == null)
        {
            mDropTableData = new Dictionary<int, Table<Tuple<int>>>();
            SetTableKey(CSVReader.Instance.LoadCSV("CSVData/DropTableData"),
                mDropTableData);
        }
    }

    /// <summary>
    /// data : csv 파일 경로
    /// </summary>
    private void SetSkillData(string[] data)
    {
        for (int i = 1; i < data.Length; ++i)
        {
            mSkillData[i] = new SkillData();    //스킬 데이터 생성
            mSkillData[i].SetData(CSVReader.Instance.GetLineSlice(data[i]));    //데이터 삽입
        }
    }   //스킬 데이터 삽입

    /// <summary>
    /// data : csv 파일 경로
    /// </summary>
    private void SetItemData(string[] data)
    {
        for (int i = 1; i < data.Length; ++i)
        {
            ItemData item = new ItemData();     //아이템 데이터 생성
            item.SetData(CSVReader.Instance.GetLineSlice(data[i]));    //데이터 삽입;
            mItemData[i] = item;
        }
    }   //아이템 데이터 삽입

    private void SetPlayerData(string[] data)
    {
        PlayerData player;
        for (int i = 1; i < data.Length; ++i)
        {
            player = new PlayerData();    //스킬 데이터 생성
            player.SetData(CSVReader.Instance.GetLineSlice(data[i]));    //데이터 삽입;
            mPlayerData.Add(player);
        }
    }   //플레이어 데이터 삽입

    private void SetEnemyData(string[] data)
    {
        EnemyData enemy;    //에너미 데이터 생성
        for (int i = 1; i < data.Length; ++i)
        {
            enemy = new EnemyData();
            string[] sliceEnemyData = CSVReader.Instance.GetLineSlice(data[i]);      //슬라이스 된 에너미의 데이터
            enemy.SetData(sliceEnemyData);    //데이터 삽입
            mEnemyData[int.Parse(sliceEnemyData[0])] = enemy;
        }
    }   //에너미 데이터 삽입

    private void SetStringTableData(string[] data)
    {
        Table<string> table;     //테이블 데이터 생성
        for (int i = 1; i < data.Length; ++i)
        {
            table = new Table<string>();
            string[] sliceTableData = CSVReader.Instance.GetListSlice(data[i]);     //리스트 슬라이스

            foreach (string strKeyCount in sliceTableData)
            {
                table.Add(strKeyCount);     //데이터 삽입
            }

            mTextTableData[i] = table;
        }
    }   //스트링 테이블 데이터 삽입

    private void SetQuestData(string[] data)
    {
        QuestData questData;
        for (int i = 1; i < data.Length; ++i)
        {
            questData = new QuestData();
            string[] sliceQuestData = CSVReader.Instance.GetLineSlice(data[i]);     //리스트 슬라이스
            questData.SetData(sliceQuestData);      //슬라이스 한 데이터 설정
            mQuestData[int.Parse(sliceQuestData[0])] = questData;    //퀘스트 데이터 삽입
        }
    }   //퀘스트 데이터 삽입

    private void SetNPCData(string[] data)
    {
        NPCData npcData;
        for (int i = 1; i < data.Length; ++i)
        {
            npcData = new NPCData();
            string[] sliceNPCData = CSVReader.Instance.GetLineSlice(data[i]);     //리스트 슬라이스
            npcData.SetData(sliceNPCData);      //슬라이스 한 데이터 설정
            mNPCData[int.Parse(sliceNPCData[0])] = npcData;    //NPC 데이터 삽입
        }
    }   //NPC 데이터 삽입

    /// <summary>
    /// type : 데이터 타입 종류
    /// key : 체크할 키값
    /// </summary>
    public bool KeyCheck(DataType type, int key)
    {
        switch (type)
        {
            case DataType.Skill:
                return mSkillData.ContainsKey(key);
            case DataType.Item:
                return mItemData.ContainsKey(key);
        }
        return false;
    }   //키에 포함 된다면 true 리턴

    private void SetTableKey(string[] data, Dictionary<int, Table<Tuple<int>>> KeyTableData)
    {
        for (int i = 1; i < data.Length; ++i)
        {
            // /단위로 슬라이스, 키 데이터 삽입
            KeyTableData.Add(i, new Table<Tuple<int>>());
            string[] sliceListKeyTableData = CSVReader.Instance.GetListSlice(data[i]);
            for(int j = 0; j < sliceListKeyTableData.Length; ++j)
            {
                KeyTableData[i].Add(Tuple.Create(int.Parse(sliceListKeyTableData[j])));
            }
        }
    }   //키값만 가지는 테이블 데이터 삽입

    private void SetTableKeyCount(string[] data, Dictionary<int, Table<Tuple<int, int>>> KeyCountTableData)
    {
        for (int i = 1; i < data.Length; ++i)
        {
            // / 단위로 슬라이스, 키 카운트 데이터 삽입
            KeyCountTableData.Add(i, new Table<Tuple<int, int>>());
            string[] sliceListKeyCountTableData = CSVReader.Instance.GetListSlice(data[i]);
            for(int j = 0; j < sliceListKeyCountTableData.Length; j+=2)
            {
                KeyCountTableData[i].Add(Tuple.Create(
                int.Parse(sliceListKeyCountTableData[j]),
                int.Parse(sliceListKeyCountTableData[j + 1])));
            }
        }
    }   //키, 갯수값을 가지는 테이블 데이터 삽입
}

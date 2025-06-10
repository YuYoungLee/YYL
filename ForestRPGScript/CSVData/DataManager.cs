using System;
using System.Collections.Generic;

public enum DataType
{
    None,
    Skill,      //��ųŸ���� ������
    Item,       //������ Ÿ���� ������
}   //������ Ÿ��

public class DataManager
{
    private Dictionary<int, SkillData> mSkillData;          //��ų ������
    private Dictionary<int, ItemData> mItemData;            //������ ������
    private List<PlayerData> mPlayerData;                   //�÷��̾� ������
    private List<IState> mStateData;                        //���� ������
    private Dictionary<int, EnemyData> mEnemyData;          //���ʹ� ���� ������
    private Dictionary<int, QuestData> mQuestData;          //����Ʈ ������
    private Dictionary<int, NPCData> mNPCData;              //NPC ������

    private Dictionary<int, Table<Tuple<int, int>>> mReinforceTableData;    //��ȭ ���̺� ������
    private Dictionary<int, Table<Tuple<int, int>>> mRewardItemTableData;     //���� ������ ���̺�
    private Dictionary<int, Table<Tuple<int, int>>> mEnemyTableData;    //���� ������
    private Dictionary<int, Table<Tuple<int>>> mDropTableData;    //��� ������ ������
    private Dictionary<int, Table<Tuple<int>>> mShopTableData;    //���� ������
    private Dictionary<int, Table<string>> mTextTableData;      //�ؽ�Ʈ ������ ex.���丮

    public SkillData GetSkillData(int iKey) => mSkillData[iKey];       //��ų ������
    public ItemData GetItemData(int iKey) => mItemData[iKey];          //������ ������
    public PlayerData GetPlayerData(int iKey) => mPlayerData[iKey];    //�÷��̾� ������
    public IState GetState(FSMState eState) => mStateData[(int)eState]; //State
    public EnemyData GetEnemyData(int iKey) => mEnemyData[iKey];        //���ʹ� ������
    public QuestData GetQuestData(int iKey) => mQuestData[iKey];        //����Ʈ ������
    public Dictionary<int, QuestData> GetQuest => mQuestData;       //����Ʈ
    public NPCData GetNPCData(int iKey) => mNPCData[iKey];      //NPC ������

    public Table<Tuple<int, int>> GetReinforceTableData(int iKey) => mReinforceTableData[iKey];
    public Table<Tuple<int, int>> GetRewardTableData(int iKey) => mRewardItemTableData[iKey];
    public Table<Tuple<int, int>> GetEnemyTableData(int iKey) => mEnemyTableData[iKey];
    public Table<Tuple<int>> GetDropTableData(int iKey) => mDropTableData[iKey];
    public Table<Tuple<int>> GetShopTableData(int iKey) => mShopTableData[iKey];

  
    //public Table<int> GetTableData(int iKey) => mKeyTableData[iKey];    //Ű���̺� ������
    //public Table<Tuple<int, int>> GetKeyCountData(int iKey) => mKeyCountTableData[iKey];    //Ű ī��Ʈ ������
    public Table<string> GetTextTableData(int iKey) => mTextTableData[iKey];    //��Ʈ�� ���̺� ������

    public void Initialize()
    {
        //null �̶�� ����
        //��ų�� �����Ͱ� null �� ��
        if(mSkillData == null)
        {
            mSkillData = new Dictionary<int, SkillData>();
            SetSkillData(CSVReader.Instance.LoadCSV("CSVData/SkillData"));
        }
        
        //������ �����Ͱ� null �� ��
        if(mItemData == null)
        {
            mItemData = new Dictionary<int, ItemData>();
            SetItemData(CSVReader.Instance.LoadCSV("CSVData/ItemData"));
        }

        //�÷��̾��� �����Ͱ� null �� ��
        if(mPlayerData == null)
        {
            mPlayerData = new List<PlayerData> { };
            SetPlayerData(CSVReader.Instance.LoadCSV("CSVData/PlayerData"));
        }

        //������ �����Ͱ� null �� ��
        if(mEnemyData == null)
        {
            mEnemyData = new Dictionary<int, EnemyData>();
            SetEnemyData(CSVReader.Instance.LoadCSV("CSVData/EnemyData"));
        }

        //FSM state �����Ͱ� null �� ��
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

        //�ؽ�Ʈ ���̺� �����Ͱ� null �� ��
        if(mTextTableData == null)
        {
            mTextTableData = new Dictionary<int, Table<string>>();
            SetStringTableData(CSVReader.Instance.LoadCSV("CSVData/StringTableData"));
        }

        //����Ʈ �����Ͱ� null �� ��
        if(mQuestData == null)
        {
            mQuestData = new Dictionary<int, QuestData>();
            SetQuestData(CSVReader.Instance.LoadCSV("CSVData/QuestData"));
        }

        //npc �����Ͱ� null �� ��
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

        //���� ������ ���̺� ������
        if(mRewardItemTableData == null)
        {
            mRewardItemTableData = new Dictionary<int, Table<Tuple<int, int>>>();
            SetTableKeyCount(CSVReader.Instance.LoadCSV("CSVData/RewardTableData"),
                mRewardItemTableData);
        }

        //���� ���̺� ������
        if(mShopTableData == null)
        {
            mShopTableData = new Dictionary<int, Table<Tuple<int>>>();
            SetTableKey(CSVReader.Instance.LoadCSV("CSVData/ShopTableData"),
                mShopTableData);
        }

        //���ʹ� ���̺� ������
        if(mEnemyTableData == null)
        {
            mEnemyTableData = new Dictionary<int, Table<Tuple<int, int>>>();
            SetTableKeyCount(CSVReader.Instance.LoadCSV("CSVData/EnemyTableData"),
                mEnemyTableData);
        }

        //��� ���̺� ������
        if(mDropTableData == null)
        {
            mDropTableData = new Dictionary<int, Table<Tuple<int>>>();
            SetTableKey(CSVReader.Instance.LoadCSV("CSVData/DropTableData"),
                mDropTableData);
        }
    }

    /// <summary>
    /// data : csv ���� ���
    /// </summary>
    private void SetSkillData(string[] data)
    {
        for (int i = 1; i < data.Length; ++i)
        {
            mSkillData[i] = new SkillData();    //��ų ������ ����
            mSkillData[i].SetData(CSVReader.Instance.GetLineSlice(data[i]));    //������ ����
        }
    }   //��ų ������ ����

    /// <summary>
    /// data : csv ���� ���
    /// </summary>
    private void SetItemData(string[] data)
    {
        for (int i = 1; i < data.Length; ++i)
        {
            ItemData item = new ItemData();     //������ ������ ����
            item.SetData(CSVReader.Instance.GetLineSlice(data[i]));    //������ ����;
            mItemData[i] = item;
        }
    }   //������ ������ ����

    private void SetPlayerData(string[] data)
    {
        PlayerData player;
        for (int i = 1; i < data.Length; ++i)
        {
            player = new PlayerData();    //��ų ������ ����
            player.SetData(CSVReader.Instance.GetLineSlice(data[i]));    //������ ����;
            mPlayerData.Add(player);
        }
    }   //�÷��̾� ������ ����

    private void SetEnemyData(string[] data)
    {
        EnemyData enemy;    //���ʹ� ������ ����
        for (int i = 1; i < data.Length; ++i)
        {
            enemy = new EnemyData();
            string[] sliceEnemyData = CSVReader.Instance.GetLineSlice(data[i]);      //�����̽� �� ���ʹ��� ������
            enemy.SetData(sliceEnemyData);    //������ ����
            mEnemyData[int.Parse(sliceEnemyData[0])] = enemy;
        }
    }   //���ʹ� ������ ����

    private void SetStringTableData(string[] data)
    {
        Table<string> table;     //���̺� ������ ����
        for (int i = 1; i < data.Length; ++i)
        {
            table = new Table<string>();
            string[] sliceTableData = CSVReader.Instance.GetListSlice(data[i]);     //����Ʈ �����̽�

            foreach (string strKeyCount in sliceTableData)
            {
                table.Add(strKeyCount);     //������ ����
            }

            mTextTableData[i] = table;
        }
    }   //��Ʈ�� ���̺� ������ ����

    private void SetQuestData(string[] data)
    {
        QuestData questData;
        for (int i = 1; i < data.Length; ++i)
        {
            questData = new QuestData();
            string[] sliceQuestData = CSVReader.Instance.GetLineSlice(data[i]);     //����Ʈ �����̽�
            questData.SetData(sliceQuestData);      //�����̽� �� ������ ����
            mQuestData[int.Parse(sliceQuestData[0])] = questData;    //����Ʈ ������ ����
        }
    }   //����Ʈ ������ ����

    private void SetNPCData(string[] data)
    {
        NPCData npcData;
        for (int i = 1; i < data.Length; ++i)
        {
            npcData = new NPCData();
            string[] sliceNPCData = CSVReader.Instance.GetLineSlice(data[i]);     //����Ʈ �����̽�
            npcData.SetData(sliceNPCData);      //�����̽� �� ������ ����
            mNPCData[int.Parse(sliceNPCData[0])] = npcData;    //NPC ������ ����
        }
    }   //NPC ������ ����

    /// <summary>
    /// type : ������ Ÿ�� ����
    /// key : üũ�� Ű��
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
    }   //Ű�� ���� �ȴٸ� true ����

    private void SetTableKey(string[] data, Dictionary<int, Table<Tuple<int>>> KeyTableData)
    {
        for (int i = 1; i < data.Length; ++i)
        {
            // /������ �����̽�, Ű ������ ����
            KeyTableData.Add(i, new Table<Tuple<int>>());
            string[] sliceListKeyTableData = CSVReader.Instance.GetListSlice(data[i]);
            for(int j = 0; j < sliceListKeyTableData.Length; ++j)
            {
                KeyTableData[i].Add(Tuple.Create(int.Parse(sliceListKeyTableData[j])));
            }
        }
    }   //Ű���� ������ ���̺� ������ ����

    private void SetTableKeyCount(string[] data, Dictionary<int, Table<Tuple<int, int>>> KeyCountTableData)
    {
        for (int i = 1; i < data.Length; ++i)
        {
            // / ������ �����̽�, Ű ī��Ʈ ������ ����
            KeyCountTableData.Add(i, new Table<Tuple<int, int>>());
            string[] sliceListKeyCountTableData = CSVReader.Instance.GetListSlice(data[i]);
            for(int j = 0; j < sliceListKeyCountTableData.Length; j+=2)
            {
                KeyCountTableData[i].Add(Tuple.Create(
                int.Parse(sliceListKeyCountTableData[j]),
                int.Parse(sliceListKeyCountTableData[j + 1])));
            }
        }
    }   //Ű, �������� ������ ���̺� ������ ����
}

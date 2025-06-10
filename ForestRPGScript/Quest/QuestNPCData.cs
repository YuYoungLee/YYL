public class QuestNPCData
{
    private int miNPCKey;   //NPC Ű��
    private int miQuestKey;     //����Ʈ Ű��
    private EQuestState meQuestState;   //����Ʈ ����

    public int GetNPCKey => miNPCKey;
    public int GetQuestKey => miQuestKey;
    public EQuestState GetQuestState => meQuestState;

    public QuestNPCData(int iNPCKey)
    {
        miNPCKey = iNPCKey;
        ResetData();    //������ �ʱ�ȭ
    }

    /// <summary>
    /// eQuestState : ����Ʈ ���� ����
    /// </summary>
    public void SetQuestState(EQuestState eQuestState)
    {
        GameManager gameMgr = GameManager.Instance;

        meQuestState = eQuestState;     //����Ʈ ���� ����

        //����Ʈ ������
        if (meQuestState == EQuestState.None)
        {
            foreach (QuestData questData in gameMgr.GetDataMgr().GetQuest.Values)
            {
                //���� ����Ʈ Ű ��
                if (miQuestKey == questData.GetPreviousQuestKey)
                {
                    miQuestKey = questData.GetQuestKey;
                    meQuestState = EQuestState.Start;   //���� ����
                    break;
                }
            }
        }

        SetNPCState();      //NPC ���� ����
    }   //����Ʈ State ����

    public void SetNPCState()
    {
        GameManager gameMgr = GameManager.Instance;

        //�ʵ� NPC�����Դٸ�
        FieldNPC fieldNPC = null;
        if (gameMgr.GetFieldNpc(out fieldNPC))
        {
            fieldNPC.GetQuestNPC(miNPCKey).NPCState(meQuestState);     //����Ʈ NPC ���� ��ȯ
        }
    }   //����Ʈ NPCState ���� ����

    public void LoadData(QuestNPCDataFromJson questNPCLoadData)
    {
        miQuestKey = questNPCLoadData.miQuestKey;    //����Ʈ Ű��
        meQuestState = questNPCLoadData.meQuestState;     //����Ʈ NPC ����

        SetNPCState();    //NPC ���� ����
    }   //����Ʈ NPC �ε�

    public void ResetData()
    {
        GameManager gameMgr = GameManager.Instance;

        miQuestKey = gameMgr.GetDataMgr().GetNPCData(miNPCKey).InitKey;
        int iPreviousKey = gameMgr.GetDataMgr().GetQuestData(miQuestKey).GetPreviousQuestKey;     //���� ����Ʈ Ű��

        //���� ����Ʈ�� -1 �� ��
        if (iPreviousKey == -1)
        {
            meQuestState = EQuestState.Start;     //����Ʈ ���� �� �� ����
        }
        else
        {
            meQuestState = EQuestState.None;    //����Ʈ ���� �ƴ� �� �ʱ⼳��
        }
    }   //������ �ʱ�ȭ
}

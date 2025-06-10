public class QuestNPCData
{
    private int miNPCKey;   //NPC 키값
    private int miQuestKey;     //퀘스트 키값
    private EQuestState meQuestState;   //퀘스트 상태

    public int GetNPCKey => miNPCKey;
    public int GetQuestKey => miQuestKey;
    public EQuestState GetQuestState => meQuestState;

    public QuestNPCData(int iNPCKey)
    {
        miNPCKey = iNPCKey;
        ResetData();    //데이터 초기화
    }

    /// <summary>
    /// eQuestState : 퀘스트 상태 설정
    /// </summary>
    public void SetQuestState(EQuestState eQuestState)
    {
        GameManager gameMgr = GameManager.Instance;

        meQuestState = eQuestState;     //퀘스트 상태 변경

        //퀘스트 끝날때
        if (meQuestState == EQuestState.None)
        {
            foreach (QuestData questData in gameMgr.GetDataMgr().GetQuest.Values)
            {
                //이전 퀘스트 키 비교
                if (miQuestKey == questData.GetPreviousQuestKey)
                {
                    miQuestKey = questData.GetQuestKey;
                    meQuestState = EQuestState.Start;   //시작 변경
                    break;
                }
            }
        }

        SetNPCState();      //NPC 상태 변경
    }   //퀘스트 State 설정

    public void SetNPCState()
    {
        GameManager gameMgr = GameManager.Instance;

        //필드 NPC가져왔다면
        FieldNPC fieldNPC = null;
        if (gameMgr.GetFieldNpc(out fieldNPC))
        {
            fieldNPC.GetQuestNPC(miNPCKey).NPCState(meQuestState);     //퀘스트 NPC 상태 전환
        }
    }   //퀘스트 NPCState 상태 설정

    public void LoadData(QuestNPCDataFromJson questNPCLoadData)
    {
        miQuestKey = questNPCLoadData.miQuestKey;    //퀘스트 키값
        meQuestState = questNPCLoadData.meQuestState;     //퀘스트 NPC 상태

        SetNPCState();    //NPC 상태 변경
    }   //퀘스트 NPC 로드

    public void ResetData()
    {
        GameManager gameMgr = GameManager.Instance;

        miQuestKey = gameMgr.GetDataMgr().GetNPCData(miNPCKey).InitKey;
        int iPreviousKey = gameMgr.GetDataMgr().GetQuestData(miQuestKey).GetPreviousQuestKey;     //이전 퀘스트 키값

        //이전 퀘스트가 -1 일 때
        if (iPreviousKey == -1)
        {
            meQuestState = EQuestState.Start;     //퀘스트 시작 일 때 설정
        }
        else
        {
            meQuestState = EQuestState.None;    //퀘스트 시작 아닐 때 초기설정
        }
    }   //데이터 초기화
}

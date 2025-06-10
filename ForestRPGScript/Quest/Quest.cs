using System;
using System.Collections.Generic;

public enum EQuestState
{
    Start,      //수락
    Progress,   //진행중
    Clear,      //조건 클리어 상태
    None,       //진행 끝났을 때
}

public class Quest
{
    private EQuestState meQuestState;     //퀘스트 상태
    private int miQuestKey = 0;           //퀘스트 키값

    private int miInteractionCount = 0;   //상호작용 카운트
    private int miKillCount = 0;          //처치 카운트 값
    private int miItemCount = 0;          //아이템 카운트 값

    public delegate DataManager DataMgr();
    public DataMgr GetDataMgr;      //데이터 메니저 델리게이트
    public delegate QuestManager QuestMgr();
    public QuestMgr GetQuestMgr;    //퀘스트 메니저 델리게이트

    public EQuestState QuestState => meQuestState;
    public int QuestKey => miQuestKey;
    public int InteractionCount => miInteractionCount;
    public int KillCount => miKillCount;
    public int ItemCount => miItemCount;

    /// <summary>
    /// iQuestKey : 퀘스트 키값
    /// </summary>
    public Quest(QuestData questData)
    {
        if (GetDataMgr == null)
        {
            GetDataMgr = GameManager.Instance.GetDataMgr;
        }

        if (GetQuestMgr == null)
        {
            GetQuestMgr = GameManager.Instance.GetQuestMgr;
        }

        meQuestState = EQuestState.Start;
        miQuestKey = questData.GetQuestKey;
        miInteractionCount = 0;
        miKillCount = 0;
        miItemCount = 0;
    }   //생성자 초기화

        /// <summary>
        /// iQuestKey : 퀘스트 키값
        /// </summary>
    public void SetData(int iQuestKey)
    {
        if(GetDataMgr == null)
        {
            GetDataMgr = GameManager.Instance.GetDataMgr;
        }

        if(GetQuestMgr == null)
        {
            GetQuestMgr = GameManager.Instance.GetQuestMgr;
        }

        //데이터 초기화
        meQuestState = EQuestState.Start;  //시작 상태
        miQuestKey = iQuestKey;
        miKillCount = 0;
        miInteractionCount = 0;
        miItemCount = 0;
    }

    /// <summary>
    /// eTargetType : 타겟의 타입
    /// iKey : 타겟의 키값
    /// iCount : 증가 갯수
    /// </summary>
    public void AddTargetCount(EQuestTargetType eTargetType, int iKey, int iCount)
    {
        //진행 중이 아니라면 리턴
        if(meQuestState != EQuestState.Progress)
        {
            return;
        }

        //퀘스트 타겟 데이터 순회
        foreach (Tuple<EQuestTargetType, int, int> tuple in GetDataMgr().GetQuestData(miQuestKey).GetTargetData)
        {
            //타겟의 키값이 같을 때
            if (tuple.Item2 == iKey)
            {
                //목표 값 까지만 값 증가
                switch (eTargetType)
                {
                    case EQuestTargetType.Kill:
                        miKillCount = Math.Min(miKillCount + iCount, tuple.Item3);
                        break;
                    case EQuestTargetType.InteractionNPC:
                        miInteractionCount = Math.Min(miInteractionCount + iCount, tuple.Item3);
                        break;
                    case EQuestTargetType.Item:
                        miItemCount = Math.Min(miItemCount + iCount, tuple.Item3);
                        break;
                }
            }
        }
    }

    public bool IsClear()
    {
        List<Tuple<EQuestTargetType, int, int>> questData = GetDataMgr().GetQuestData(miQuestKey).GetTargetData;
        int iClearCount = 0;    //클리어 한 갯수
        int iMaxTargetCount = questData.Count;    //총 목표갯수
        //퀘스트 타겟 데이터 순회
        foreach (Tuple<EQuestTargetType, int, int> tuple in questData)
        {
            //목표 값 타겟값 비교
            switch (tuple.Item1)
            {
                case EQuestTargetType.Kill:
                    if(miKillCount >= tuple.Item3)
                    {
                        ++iClearCount;
                    }
                    break;
                case EQuestTargetType.InteractionNPC:
                    if (miInteractionCount >= tuple.Item3)
                    {
                        ++iClearCount;
                    }
                    break;
                case EQuestTargetType.Item:
                    if (miItemCount >= tuple.Item3)
                    {
                        ++iClearCount;
                    }
                    break;
            }
        }

        return iClearCount == iMaxTargetCount;
    }   //퀘스트 클리어 일 때 true 리턴

    /// <summary>
    /// eQuestState : 설정할 퀘스트 상태
    /// </summary>
    public void SetQuestState(EQuestState eQuestState)
    {
        meQuestState = eQuestState;    //퀘스트 상태 변경
        UIManager uiMgr = UIManager.Instance;    //ui 메니저
        GameManager gameMgr = GameManager.Instance;
        gameMgr.GetQuestMgr().SetNPCQuestState(meQuestState, miQuestKey);       //퀘스트 상태 변경
        //퀘스트 상태 별 작동해야 할 기능
        switch (meQuestState)
        {
            case EQuestState.None:
                //클리어 퀘스트 표시
                (uiMgr.GetGUI(EGUI.Player) as PlayerPanel).GetClearQuest.PlayImageAnim();

                //아이템 보상
                int iRewardTableKey = GetDataMgr().GetQuestData(miQuestKey).GetRewardTableKey;
                EventManager.Instance.AddValueEvent(EEventMessage.AddTableItem, iRewardTableKey);
                gameMgr.GetPlayer.AddStat(EStatDataType.EXE, GetDataMgr().GetQuestData(miQuestKey).GetRewardExe);   //플레이어 경험치
                
                (UIManager.Instance.GetGUI(EGUI.Quest) as QuestPanel).RemoveQuestSlot(miQuestKey);      //UI퀘스트 슬롯 제거

                //다른 NPC 상태 변경

                GetQuestMgr().RemoveQuest(miQuestKey);      //수락 퀘스트 제거
                break;
            default:
                break;
        }
    }   //퀘스트 상태별 동작

    /// <summary>
    /// acceptQuest : 로드할 수락한 퀘스트 데이터
    /// </summary>
    public void LoadData(QuestDataFromJson acceptQuest)
    {
        //퀘스트 데이터 불러오기
        meQuestState = acceptQuest.meQuestState;
        miQuestKey = acceptQuest.miQuestKey;
        miKillCount = acceptQuest.miKillCount;
        miItemCount = acceptQuest.miItemCount;
        miInteractionCount = acceptQuest.miInteractionCount;
    }   //데이터 로드
}

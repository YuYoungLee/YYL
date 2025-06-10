using System.Collections.Generic;

public class QuestManager : ISubject
{
    private Dictionary<int, Quest> mAcceptQuest = new();        //수락 한 퀘스트
    private Dictionary<int, QuestNPCData> mQuestNPCData;        //퀘스트 NPCData
    private List<int> mClearQuestKey;                           //클리어 한 퀘스트 키값
    private List<IObserver> mObserver;                          //퀘스트 슬롯 옵저버

    public delegate DataManager DataMgr();
    public DataMgr dataMgr;     //데이터 mgr

    public Dictionary<int, QuestNPCData> GetQuestNPCData => mQuestNPCData;
    public Dictionary<int, Quest> GetAcceptQuest => mAcceptQuest;
    public List<int> GetClearQuestKey => mClearQuestKey;

    public bool IsClearQuest(int iQuestKey) => mClearQuestKey.Contains(iQuestKey);     //키값이 포함되어 있을때 true
    public Quest GetQuest(int iQuestKey)
    {
        //키값을 포함 할 때
        if(mAcceptQuest.ContainsKey(iQuestKey))
        {
            return mAcceptQuest[iQuestKey];
        }

        return null;
    }       //키값에 해당하는 퀘스트 리턴 없을시 null
    public void Initialize()
    {
        //수락한 퀘스트
        if(mAcceptQuest == null)
        {
            mAcceptQuest = new Dictionary<int, Quest>();
        }

        if(mQuestNPCData == null)
        {
            mQuestNPCData = new Dictionary<int, QuestNPCData>()
            {
                { 5002, new QuestNPCData(5002) },
                { 5006, new QuestNPCData(5006) }
            };
        }

        //클리어 한 퀘스트
        if(mClearQuestKey == null)
        {
            mClearQuestKey = new List<int>();
        }

        //퀘스트 구독하는 옵저버
        if(mObserver == null)
        {
            mObserver = new List<IObserver>();
        }

        //데이터 메니저
        if (dataMgr == null)
        {
            dataMgr = GameManager.Instance.GetDataMgr;
        }
    }

    private void AddQuest(QuestData questData)
    {
        //키값이 포함되어 있다면 리턴
        if (mAcceptQuest.ContainsKey(questData.GetQuestKey))
        {
            return;
        }

        //퀘스트 생성 후 삽입
        mAcceptQuest.Add(questData.GetQuestKey, new Quest(questData));
        (UIManager.Instance.GetGUI(EGUI.Quest) as QuestPanel).
            AddQuest(questData.GetQuestKey);    //퀘스트 생성
    }   //퀘스트 생성

    /// <summary>
    /// iQuestKey : 제거할 퀘스트 키값
    /// </summary>
    public void RemoveQuest(int iQuestKey)
    {
        //수락한 퀘스트에 있을 경우
        if (mAcceptQuest.ContainsKey(iQuestKey))
        {
            mAcceptQuest.Remove(iQuestKey);     //수락한 퀘스트 제거
            mClearQuestKey.Add(iQuestKey);      //제거한 퀘스트 키 삽입
        }
    }   //수락한 퀘스트 제거

    /// <summary>
    /// iNPCKey : NPC 키값
    /// </summary>
    public void Talk(int iNPCKey)
    {
        //퀘스트 NPC 데이터에 포함될 때만 실행
        if (!mQuestNPCData.ContainsKey(iNPCKey))
        {
            return;
        }

        Dialogue dialogue = UIManager.Instance.GetGUI(EGUI.Dialogue) as Dialogue;   //다이알로그
        GameManager gameMgr = GameManager.Instance;     //게임 메니저

        int iQuestKey = mQuestNPCData[iNPCKey].GetQuestKey;     //퀘스트 키값
        int iPreviousKey = gameMgr.GetDataMgr().GetQuestData(iQuestKey).GetPreviousQuestKey;     //이전 퀘스트 키값

        //클리어 한 퀘스트 일 경우 || 사전 퀘스트 클리어 못한 경우
        if (IsClearQuest(iQuestKey) || 
            (iPreviousKey != -1 && !IsClearQuest(iPreviousKey)))
        {
            //텍스트만 출력 후 리턴
            dialogue.ActiveDialogue(iQuestKey, EQuestState.None);
            return;
        }
        else if (!mAcceptQuest.ContainsKey(iQuestKey))
        {
            //수락한 퀘스트가 아닐 경우
            AddQuest(gameMgr.GetDataMgr().GetQuestData(iQuestKey));    //퀘스트 추가
        }

        dialogue.ActiveDialogue(iQuestKey, mAcceptQuest[iQuestKey].QuestState);     //대사 출력
    }   //퀘스트 대화

    public void EndTalk(int iQuestKey)
    {
        Quest questData = GetQuest(iQuestKey);

        if(questData == null)
        {
            return;
        }
         
        switch (questData.QuestState)
        {
            case EQuestState.Start:
                questData.SetQuestState(EQuestState.Progress);    //진행중으로 변경
                SaveLoadManager.Instance.Save(ESaveData.Auto, 0);   //자동 세이브
                break;
            case EQuestState.Clear:
                questData.SetQuestState(EQuestState.None);    //끝난 상태로 변경
                SaveLoadManager.Instance.Save(ESaveData.Auto, 0);   //자동 세이브
                break;
            default:
                break;
        }
    }   //퀘스트 대화가 끝났을 때 처리

    /// <summary>
    /// eTargetType : 퀘스트 타겟의 타입
    /// iTargetKey : 타겟의 키값
    /// iCount : 증가값
    /// </summary>
    public void AddCount(EQuestTargetType eTargetType, int iTargetKey, int iCount)
    {
        //수락한 퀘스트 순회
        foreach (int iKey in mAcceptQuest.Keys)
        {
            mAcceptQuest[iKey].AddTargetCount(eTargetType, iTargetKey, iCount);

            //퀘스트가 진행중일때 완료 했을 때 
            if(mAcceptQuest[iKey].QuestState == EQuestState.Progress &&
                mAcceptQuest[iKey].IsClear())
            {
                mAcceptQuest[iKey].SetQuestState(EQuestState.Clear);    //퀘스트 클리어 상태로 변경
            }
        }

        Notify();     //옵저버 알리기
    }   //퀘스트 목표 갯수 증가
    
    #region ISubject
    public void Notify()
    {
        for (int i = 0; i < mObserver.Count; ++i)
        {
            mObserver[i].UpdateObserver();
        }
    }   //옵저버에게 알리기
    /// <summary>
    /// observer : 구독할 옵저버
    /// </summary>
    public void Add(IObserver observer)
    {
        if (!mObserver.Contains(observer))
        {
            mObserver.Add(observer);
        }
    }   //옵저버 추가
    public void Remove(IObserver observer)
    {
        if (mObserver.Contains(observer))
        {
            mObserver.Remove(observer);
        }
    }   //옵저버 제거
    #endregion

    /// <summary>
    /// data : 세이브 데이터
    /// </summary>
    public void LoadData(SaveData data)
    {

        GameManager gameMgr = GameManager.Instance;

        mAcceptQuest.Clear();   //데이터 비우기
        mClearQuestKey.Clear();     //퀘스트 데이터 비우기
        
        //퀘스트 NPC 데이터 초기화
        foreach (QuestNPCData questNPCData in mQuestNPCData.Values)
        {
            questNPCData.ResetData();
        }

        for (int i = 0; i < data.mQuestData.Count; ++i)
        {
            AddQuest(gameMgr.GetDataMgr().GetQuestData(data.mQuestData[i].miQuestKey));
            mAcceptQuest[data.mQuestData[i].miQuestKey].LoadData(data.mQuestData[i]);
        }

        foreach (int iClearQuestKey in data.mClearQuestKey)
        {
            mClearQuestKey.Add(iClearQuestKey);
        }

        //퀘스트 NPC 데이터 로드
        foreach (QuestNPCDataFromJson questNPCData in data.mQuestNPCData)
        {
            mQuestNPCData[questNPCData.miNPCKey].LoadData(questNPCData);
        }

        Notify();   //옵저버 알리기
    }   //데이터 로드

    public void SetNPCQuestState(EQuestState eQuestState, int iQuestKey)
    {
        DataManager dataMgr = GameManager.Instance.GetDataMgr();
        foreach (QuestNPCData questNPCData in mQuestNPCData.Values)
        {
            //퀘스트 키가 일치한다면
            if (questNPCData.GetQuestKey == iQuestKey)
            {
                questNPCData.SetQuestState(eQuestState);    //퀘스트 상태 변경
            }

            //퀘스트를 끝났을 때, 다음 퀘스트 진입 해야 할 경우
            if (eQuestState == EQuestState.None &&
                iQuestKey == dataMgr.GetQuestData(questNPCData.GetQuestKey).GetPreviousQuestKey)
            {
                questNPCData.SetQuestState(EQuestState.Start);     //퀘스트 시작인 NPC Start 로 변경
            }
        }
    }    //퀘스트 키와 일치한 NPC 퀘스트 상태 변경

    public void SetQuestNPCState()
    {
        foreach (QuestNPCData questNPCData in mQuestNPCData.Values)
        {
            questNPCData.SetNPCState();
        }
    }   //퀘스트 NPC 상태 설정
}
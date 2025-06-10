using System.Collections.Generic;

public class QuestManager : ISubject
{
    private Dictionary<int, Quest> mAcceptQuest = new();        //���� �� ����Ʈ
    private Dictionary<int, QuestNPCData> mQuestNPCData;        //����Ʈ NPCData
    private List<int> mClearQuestKey;                           //Ŭ���� �� ����Ʈ Ű��
    private List<IObserver> mObserver;                          //����Ʈ ���� ������

    public delegate DataManager DataMgr();
    public DataMgr dataMgr;     //������ mgr

    public Dictionary<int, QuestNPCData> GetQuestNPCData => mQuestNPCData;
    public Dictionary<int, Quest> GetAcceptQuest => mAcceptQuest;
    public List<int> GetClearQuestKey => mClearQuestKey;

    public bool IsClearQuest(int iQuestKey) => mClearQuestKey.Contains(iQuestKey);     //Ű���� ���ԵǾ� ������ true
    public Quest GetQuest(int iQuestKey)
    {
        //Ű���� ���� �� ��
        if(mAcceptQuest.ContainsKey(iQuestKey))
        {
            return mAcceptQuest[iQuestKey];
        }

        return null;
    }       //Ű���� �ش��ϴ� ����Ʈ ���� ������ null
    public void Initialize()
    {
        //������ ����Ʈ
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

        //Ŭ���� �� ����Ʈ
        if(mClearQuestKey == null)
        {
            mClearQuestKey = new List<int>();
        }

        //����Ʈ �����ϴ� ������
        if(mObserver == null)
        {
            mObserver = new List<IObserver>();
        }

        //������ �޴���
        if (dataMgr == null)
        {
            dataMgr = GameManager.Instance.GetDataMgr;
        }
    }

    private void AddQuest(QuestData questData)
    {
        //Ű���� ���ԵǾ� �ִٸ� ����
        if (mAcceptQuest.ContainsKey(questData.GetQuestKey))
        {
            return;
        }

        //����Ʈ ���� �� ����
        mAcceptQuest.Add(questData.GetQuestKey, new Quest(questData));
        (UIManager.Instance.GetGUI(EGUI.Quest) as QuestPanel).
            AddQuest(questData.GetQuestKey);    //����Ʈ ����
    }   //����Ʈ ����

    /// <summary>
    /// iQuestKey : ������ ����Ʈ Ű��
    /// </summary>
    public void RemoveQuest(int iQuestKey)
    {
        //������ ����Ʈ�� ���� ���
        if (mAcceptQuest.ContainsKey(iQuestKey))
        {
            mAcceptQuest.Remove(iQuestKey);     //������ ����Ʈ ����
            mClearQuestKey.Add(iQuestKey);      //������ ����Ʈ Ű ����
        }
    }   //������ ����Ʈ ����

    /// <summary>
    /// iNPCKey : NPC Ű��
    /// </summary>
    public void Talk(int iNPCKey)
    {
        //����Ʈ NPC �����Ϳ� ���Ե� ���� ����
        if (!mQuestNPCData.ContainsKey(iNPCKey))
        {
            return;
        }

        Dialogue dialogue = UIManager.Instance.GetGUI(EGUI.Dialogue) as Dialogue;   //���̾˷α�
        GameManager gameMgr = GameManager.Instance;     //���� �޴���

        int iQuestKey = mQuestNPCData[iNPCKey].GetQuestKey;     //����Ʈ Ű��
        int iPreviousKey = gameMgr.GetDataMgr().GetQuestData(iQuestKey).GetPreviousQuestKey;     //���� ����Ʈ Ű��

        //Ŭ���� �� ����Ʈ �� ��� || ���� ����Ʈ Ŭ���� ���� ���
        if (IsClearQuest(iQuestKey) || 
            (iPreviousKey != -1 && !IsClearQuest(iPreviousKey)))
        {
            //�ؽ�Ʈ�� ��� �� ����
            dialogue.ActiveDialogue(iQuestKey, EQuestState.None);
            return;
        }
        else if (!mAcceptQuest.ContainsKey(iQuestKey))
        {
            //������ ����Ʈ�� �ƴ� ���
            AddQuest(gameMgr.GetDataMgr().GetQuestData(iQuestKey));    //����Ʈ �߰�
        }

        dialogue.ActiveDialogue(iQuestKey, mAcceptQuest[iQuestKey].QuestState);     //��� ���
    }   //����Ʈ ��ȭ

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
                questData.SetQuestState(EQuestState.Progress);    //���������� ����
                SaveLoadManager.Instance.Save(ESaveData.Auto, 0);   //�ڵ� ���̺�
                break;
            case EQuestState.Clear:
                questData.SetQuestState(EQuestState.None);    //���� ���·� ����
                SaveLoadManager.Instance.Save(ESaveData.Auto, 0);   //�ڵ� ���̺�
                break;
            default:
                break;
        }
    }   //����Ʈ ��ȭ�� ������ �� ó��

    /// <summary>
    /// eTargetType : ����Ʈ Ÿ���� Ÿ��
    /// iTargetKey : Ÿ���� Ű��
    /// iCount : ������
    /// </summary>
    public void AddCount(EQuestTargetType eTargetType, int iTargetKey, int iCount)
    {
        //������ ����Ʈ ��ȸ
        foreach (int iKey in mAcceptQuest.Keys)
        {
            mAcceptQuest[iKey].AddTargetCount(eTargetType, iTargetKey, iCount);

            //����Ʈ�� �������϶� �Ϸ� ���� �� 
            if(mAcceptQuest[iKey].QuestState == EQuestState.Progress &&
                mAcceptQuest[iKey].IsClear())
            {
                mAcceptQuest[iKey].SetQuestState(EQuestState.Clear);    //����Ʈ Ŭ���� ���·� ����
            }
        }

        Notify();     //������ �˸���
    }   //����Ʈ ��ǥ ���� ����
    
    #region ISubject
    public void Notify()
    {
        for (int i = 0; i < mObserver.Count; ++i)
        {
            mObserver[i].UpdateObserver();
        }
    }   //���������� �˸���
    /// <summary>
    /// observer : ������ ������
    /// </summary>
    public void Add(IObserver observer)
    {
        if (!mObserver.Contains(observer))
        {
            mObserver.Add(observer);
        }
    }   //������ �߰�
    public void Remove(IObserver observer)
    {
        if (mObserver.Contains(observer))
        {
            mObserver.Remove(observer);
        }
    }   //������ ����
    #endregion

    /// <summary>
    /// data : ���̺� ������
    /// </summary>
    public void LoadData(SaveData data)
    {

        GameManager gameMgr = GameManager.Instance;

        mAcceptQuest.Clear();   //������ ����
        mClearQuestKey.Clear();     //����Ʈ ������ ����
        
        //����Ʈ NPC ������ �ʱ�ȭ
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

        //����Ʈ NPC ������ �ε�
        foreach (QuestNPCDataFromJson questNPCData in data.mQuestNPCData)
        {
            mQuestNPCData[questNPCData.miNPCKey].LoadData(questNPCData);
        }

        Notify();   //������ �˸���
    }   //������ �ε�

    public void SetNPCQuestState(EQuestState eQuestState, int iQuestKey)
    {
        DataManager dataMgr = GameManager.Instance.GetDataMgr();
        foreach (QuestNPCData questNPCData in mQuestNPCData.Values)
        {
            //����Ʈ Ű�� ��ġ�Ѵٸ�
            if (questNPCData.GetQuestKey == iQuestKey)
            {
                questNPCData.SetQuestState(eQuestState);    //����Ʈ ���� ����
            }

            //����Ʈ�� ������ ��, ���� ����Ʈ ���� �ؾ� �� ���
            if (eQuestState == EQuestState.None &&
                iQuestKey == dataMgr.GetQuestData(questNPCData.GetQuestKey).GetPreviousQuestKey)
            {
                questNPCData.SetQuestState(EQuestState.Start);     //����Ʈ ������ NPC Start �� ����
            }
        }
    }    //����Ʈ Ű�� ��ġ�� NPC ����Ʈ ���� ����

    public void SetQuestNPCState()
    {
        foreach (QuestNPCData questNPCData in mQuestNPCData.Values)
        {
            questNPCData.SetNPCState();
        }
    }   //����Ʈ NPC ���� ����
}
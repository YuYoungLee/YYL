using System;
using System.Collections.Generic;

public enum EQuestState
{
    Start,      //����
    Progress,   //������
    Clear,      //���� Ŭ���� ����
    None,       //���� ������ ��
}

public class Quest
{
    private EQuestState meQuestState;     //����Ʈ ����
    private int miQuestKey = 0;           //����Ʈ Ű��

    private int miInteractionCount = 0;   //��ȣ�ۿ� ī��Ʈ
    private int miKillCount = 0;          //óġ ī��Ʈ ��
    private int miItemCount = 0;          //������ ī��Ʈ ��

    public delegate DataManager DataMgr();
    public DataMgr GetDataMgr;      //������ �޴��� ��������Ʈ
    public delegate QuestManager QuestMgr();
    public QuestMgr GetQuestMgr;    //����Ʈ �޴��� ��������Ʈ

    public EQuestState QuestState => meQuestState;
    public int QuestKey => miQuestKey;
    public int InteractionCount => miInteractionCount;
    public int KillCount => miKillCount;
    public int ItemCount => miItemCount;

    /// <summary>
    /// iQuestKey : ����Ʈ Ű��
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
    }   //������ �ʱ�ȭ

        /// <summary>
        /// iQuestKey : ����Ʈ Ű��
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

        //������ �ʱ�ȭ
        meQuestState = EQuestState.Start;  //���� ����
        miQuestKey = iQuestKey;
        miKillCount = 0;
        miInteractionCount = 0;
        miItemCount = 0;
    }

    /// <summary>
    /// eTargetType : Ÿ���� Ÿ��
    /// iKey : Ÿ���� Ű��
    /// iCount : ���� ����
    /// </summary>
    public void AddTargetCount(EQuestTargetType eTargetType, int iKey, int iCount)
    {
        //���� ���� �ƴ϶�� ����
        if(meQuestState != EQuestState.Progress)
        {
            return;
        }

        //����Ʈ Ÿ�� ������ ��ȸ
        foreach (Tuple<EQuestTargetType, int, int> tuple in GetDataMgr().GetQuestData(miQuestKey).GetTargetData)
        {
            //Ÿ���� Ű���� ���� ��
            if (tuple.Item2 == iKey)
            {
                //��ǥ �� ������ �� ����
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
        int iClearCount = 0;    //Ŭ���� �� ����
        int iMaxTargetCount = questData.Count;    //�� ��ǥ����
        //����Ʈ Ÿ�� ������ ��ȸ
        foreach (Tuple<EQuestTargetType, int, int> tuple in questData)
        {
            //��ǥ �� Ÿ�ٰ� ��
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
    }   //����Ʈ Ŭ���� �� �� true ����

    /// <summary>
    /// eQuestState : ������ ����Ʈ ����
    /// </summary>
    public void SetQuestState(EQuestState eQuestState)
    {
        meQuestState = eQuestState;    //����Ʈ ���� ����
        UIManager uiMgr = UIManager.Instance;    //ui �޴���
        GameManager gameMgr = GameManager.Instance;
        gameMgr.GetQuestMgr().SetNPCQuestState(meQuestState, miQuestKey);       //����Ʈ ���� ����
        //����Ʈ ���� �� �۵��ؾ� �� ���
        switch (meQuestState)
        {
            case EQuestState.None:
                //Ŭ���� ����Ʈ ǥ��
                (uiMgr.GetGUI(EGUI.Player) as PlayerPanel).GetClearQuest.PlayImageAnim();

                //������ ����
                int iRewardTableKey = GetDataMgr().GetQuestData(miQuestKey).GetRewardTableKey;
                EventManager.Instance.AddValueEvent(EEventMessage.AddTableItem, iRewardTableKey);
                gameMgr.GetPlayer.AddStat(EStatDataType.EXE, GetDataMgr().GetQuestData(miQuestKey).GetRewardExe);   //�÷��̾� ����ġ
                
                (UIManager.Instance.GetGUI(EGUI.Quest) as QuestPanel).RemoveQuestSlot(miQuestKey);      //UI����Ʈ ���� ����

                //�ٸ� NPC ���� ����

                GetQuestMgr().RemoveQuest(miQuestKey);      //���� ����Ʈ ����
                break;
            default:
                break;
        }
    }   //����Ʈ ���º� ����

    /// <summary>
    /// acceptQuest : �ε��� ������ ����Ʈ ������
    /// </summary>
    public void LoadData(QuestDataFromJson acceptQuest)
    {
        //����Ʈ ������ �ҷ�����
        meQuestState = acceptQuest.meQuestState;
        miQuestKey = acceptQuest.miQuestKey;
        miKillCount = acceptQuest.miKillCount;
        miItemCount = acceptQuest.miItemCount;
        miInteractionCount = acceptQuest.miInteractionCount;
    }   //������ �ε�
}

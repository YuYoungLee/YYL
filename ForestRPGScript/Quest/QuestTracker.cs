using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class QuestTracker : MonoBehaviour
{
    private StringBuilder mStrBuilder;     //��Ʈ�� ����

    [SerializeField] private GameObject mQuestTapObject;     //����Ʈ �� ������Ʈ
    [SerializeField] private TextMeshProUGUI mTitleText;    //Ÿ��Ʋ �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI mTargetInfoamtionText;     //Ÿ�� ���� �ؽ�Ʈ

    public bool IsActive => mQuestTapObject.activeSelf;     //Ȱ��ȭ �����϶� true ����

    public void Initialize()
    {
        if(mStrBuilder == null)
        {
            mStrBuilder = new StringBuilder();
        }
    }
    public void SetText(int iQuestKey)
    {
        GameManager gameMgr = GameManager.Instance;

        QuestData questData = gameMgr.GetDataMgr().GetQuestData(iQuestKey);     //����Ʈ ������
        mStrBuilder.Clear();
        mStrBuilder.Append(questData.GetQuestTitle);     //����Ʈ Ÿ��Ʋ
        mTitleText.text = mStrBuilder.ToString();

        //����Ʈ Ÿ�� �ؽ�Ʈ ����
        List<Tuple<EQuestTargetType, int, int>> targetData = questData.GetTargetData;
        mStrBuilder.Clear();    //�ý�Ʈ ����
        //Ÿ�� �ؽ�Ʈ
        for (int i = 0; i < targetData.Count; ++i)
        {
            switch (targetData[i].Item1)
            {
                case EQuestTargetType.Kill:
                    mStrBuilder.AppendFormat("{0} óġ {1}/{2}",
                        gameMgr.GetDataMgr().GetEnemyData(targetData[i].Item2).Name,
                        gameMgr.GetQuestMgr().GetQuest(iQuestKey).KillCount,
                        targetData[i].Item3);
                    break;
                case EQuestTargetType.InteractionNPC:
                    mStrBuilder.AppendFormat("{0} ��ȭ�ϱ� {1}/{2}",
                        gameMgr.GetDataMgr().GetNPCData(targetData[i].Item2).Name,
                        gameMgr.GetQuestMgr().GetQuest(iQuestKey).InteractionCount,
                        targetData[i].Item3);
                    break;
                case EQuestTargetType.Item:
                    mStrBuilder.AppendFormat("{0} ȹ�� {1}/{2}",
                        gameMgr.GetDataMgr().GetItemData(targetData[i].Item2).Name,
                        gameMgr.GetQuestMgr().GetQuest(iQuestKey).ItemCount,
                        targetData[i].Item3);
                    break;
                default:
                    Debug.LogError("SetText is Default");
                    break;
            }

            if (i < targetData.Count -1)
            {
                mStrBuilder.Append("\n");    //���� �� �߰�
            }

            //Ÿ�� ���� �ؽ�Ʈ
            mTargetInfoamtionText.text = mStrBuilder.ToString();
        }

        mQuestTapObject.SetActive(true);    //����Ʈ �� Ȱ��ȭ
    }

    public void ResetData()
    {
        //�ý�Ʈ ����
        mStrBuilder.Clear();
        mTitleText.text = mStrBuilder.ToString();
        mTargetInfoamtionText.text = mStrBuilder.ToString();
        mQuestTapObject.SetActive(false);    //����Ʈ �� ��Ȱ��ȭ
    }   //������ �ʱ�ȭ
}

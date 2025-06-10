using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class QuestTracker : MonoBehaviour
{
    private StringBuilder mStrBuilder;     //스트링 빌더

    [SerializeField] private GameObject mQuestTapObject;     //퀘스트 텝 오브젝트
    [SerializeField] private TextMeshProUGUI mTitleText;    //타이틀 텍스트
    [SerializeField] private TextMeshProUGUI mTargetInfoamtionText;     //타겟 정보 텍스트

    public bool IsActive => mQuestTapObject.activeSelf;     //활성화 상태일때 true 리턴

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

        QuestData questData = gameMgr.GetDataMgr().GetQuestData(iQuestKey);     //퀘스트 데이터
        mStrBuilder.Clear();
        mStrBuilder.Append(questData.GetQuestTitle);     //퀘스트 타이틀
        mTitleText.text = mStrBuilder.ToString();

        //퀘스트 타겟 텍스트 설정
        List<Tuple<EQuestTargetType, int, int>> targetData = questData.GetTargetData;
        mStrBuilder.Clear();    //택스트 비우기
        //타겟 텍스트
        for (int i = 0; i < targetData.Count; ++i)
        {
            switch (targetData[i].Item1)
            {
                case EQuestTargetType.Kill:
                    mStrBuilder.AppendFormat("{0} 처치 {1}/{2}",
                        gameMgr.GetDataMgr().GetEnemyData(targetData[i].Item2).Name,
                        gameMgr.GetQuestMgr().GetQuest(iQuestKey).KillCount,
                        targetData[i].Item3);
                    break;
                case EQuestTargetType.InteractionNPC:
                    mStrBuilder.AppendFormat("{0} 대화하기 {1}/{2}",
                        gameMgr.GetDataMgr().GetNPCData(targetData[i].Item2).Name,
                        gameMgr.GetQuestMgr().GetQuest(iQuestKey).InteractionCount,
                        targetData[i].Item3);
                    break;
                case EQuestTargetType.Item:
                    mStrBuilder.AppendFormat("{0} 획득 {1}/{2}",
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
                mStrBuilder.Append("\n");    //다음 행 추가
            }

            //타겟 정보 텍스트
            mTargetInfoamtionText.text = mStrBuilder.ToString();
        }

        mQuestTapObject.SetActive(true);    //퀘스트 탭 활성화
    }

    public void ResetData()
    {
        //택스트 비우기
        mStrBuilder.Clear();
        mTitleText.text = mStrBuilder.ToString();
        mTargetInfoamtionText.text = mStrBuilder.ToString();
        mQuestTapObject.SetActive(false);    //퀘스트 탭 비활성화
    }   //데이터 초기화
}

using System;
using System.Collections.Generic;

public enum EQuestTargetType     //퀘스트 타겟의 유형
{
    Kill,              //처치
    InteractionNPC,    //NPC상호작용
    Item,              //아이템 획득
}

public class QuestData
{
    private int miQuestKey;             //퀘스트 키값
    private string mstrTitle;           //퀘스트 타이틀
    private string mstrDescription;     //퀘스트 설명
    private List<int> miTextKey;        //텍스트 다이알로그 키값
    private List<Tuple<EQuestTargetType, int, int>> mTargetData;     //타겟의 데이터 <유형, 목표키값, 갯수>
    private int miRewardTableKey;       //보상 테이블 키
    private int miPreviousQuestKey;     //선행 퀘스트 키값 없으면 -1
    private int miRewardExe;            //퀘스트 보상경험치

    public int GetQuestKey => miQuestKey;
    public string GetQuestTitle => mstrTitle;
    public string GetQuestDescription => mstrDescription;
    public List<Tuple<EQuestTargetType, int, int>> GetTargetData => mTargetData;
    public int GetTextKey(int iIdx) => miTextKey[iIdx];
    public int GetRewardTableKey => miRewardTableKey;
    public int GetPreviousQuestKey => miPreviousQuestKey;
    public int GetRewardExe => miRewardExe;

    public void SetData(string[] data)
    {
        miQuestKey = int.Parse(data[0]);
        mstrTitle = data[1];
        mstrDescription = data[2];

        //타겟 데이터가 null 일 때
        if (mTargetData == null)
        {
            //리스트 생성 및 데이터 슬라이스
            mTargetData = new List<Tuple<EQuestTargetType, int, int>>();
            string[] strTargetType = CSVReader.Instance.GetListSlice(data[3]);
            string[] strTargetKey = CSVReader.Instance.GetListSlice(data[4]);
            string[] strTargetCount = CSVReader.Instance.GetListSlice(data[5]);

            //리스트에 타겟 데이터 삽입
            for(int i = 0; i < strTargetType.Length; ++i)
            {
                mTargetData.Add(Tuple.Create(
                    Enum.Parse<EQuestTargetType>(strTargetType[i]),     //타겟의 타입
                    int.Parse(strTargetKey[i]),                         //타겟의 키값
                    int.Parse(strTargetCount[i])                        //타겟의 갯수
                    ));
            }
        }

        if(miTextKey == null)
        {
            //리스트 생성 및 데이터 슬라이스
            miTextKey = new List<int>();
            string[] strTextKey = CSVReader.Instance.GetListSlice(data[6]);
            for(int i = 0; i < strTextKey.Length; ++i)
            {
                miTextKey.Add(int.Parse(strTextKey[i]));
            }
        }

        miRewardTableKey = int.Parse(data[7]);
        miPreviousQuestKey = int.Parse(data[8]);
        miRewardExe = int.Parse(data[9]);
    }
}

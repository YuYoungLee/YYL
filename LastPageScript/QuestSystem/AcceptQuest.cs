using System;
using System.Collections.Generic;

public class AcceptQuest : ISubject
{
    private QuestAddValueType addValueType;     //퀘스트 증가값 구분할 타입
    private int questID;                        //퀘스트 키
    private int clearCount;                    //보상값
    private int questCount;                     //퀘스트 카운트 증가값
    private List<IObserver> observer;           //구독할 옵저버

    public int Key => questID;
    public int QuestCount => questCount;
    public int ClearCount => clearCount;
    public int SetCount { set { questCount = value; } }
    public void SetData(QuestData data)
    {
        observer = new List<IObserver>();
        addValueType = data.AddValueType;
        questID = data.QuestID;
        clearCount = data.QuestClearValue;
        questCount = 0;
    }   //데이터 설정

    public override void Notify()
    {
        for(int i = 0; i < observer.Count; ++i)
        {
            observer[i].UpdateObserver(questID);
        }
    }   //옵저버 알림

    public override void Add(IObserver obs)
    {
        if(observer.Contains(obs))
        {
            return;
        }

        observer.Add(obs);
    }   //옵저버 구독
    public override void Remove(IObserver obs)
    {
        for (int i = 0; i < observer.Count; ++i)
        {
            if (observer[i] == obs)
            {
                observer.RemoveAt(i);       //퀘스트 구독 제거
                questCount = 0;             //퀘스트 카운트 초기화
                return;
            }
        }
    }   //옵저버 구독 취소

    public void AddCount(QuestAddValueType type)
    {
        //타입이 같을 때 카운트 증가
        if (addValueType == type)
        { questCount = Math.Min(++questCount, clearCount); }
    }   //타입이 같을때 카운트 증가, 퀘스트UI 수정
    public bool ClearQuestCheck()
    {
        return clearCount == questCount ? true : false;
    }
}

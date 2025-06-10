using System;
using System.Collections.Generic;

public class AcceptQuest : ISubject
{
    private QuestAddValueType addValueType;     //����Ʈ ������ ������ Ÿ��
    private int questID;                        //����Ʈ Ű
    private int clearCount;                    //����
    private int questCount;                     //����Ʈ ī��Ʈ ������
    private List<IObserver> observer;           //������ ������

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
    }   //������ ����

    public override void Notify()
    {
        for(int i = 0; i < observer.Count; ++i)
        {
            observer[i].UpdateObserver(questID);
        }
    }   //������ �˸�

    public override void Add(IObserver obs)
    {
        if(observer.Contains(obs))
        {
            return;
        }

        observer.Add(obs);
    }   //������ ����
    public override void Remove(IObserver obs)
    {
        for (int i = 0; i < observer.Count; ++i)
        {
            if (observer[i] == obs)
            {
                observer.RemoveAt(i);       //����Ʈ ���� ����
                questCount = 0;             //����Ʈ ī��Ʈ �ʱ�ȭ
                return;
            }
        }
    }   //������ ���� ���

    public void AddCount(QuestAddValueType type)
    {
        //Ÿ���� ���� �� ī��Ʈ ����
        if (addValueType == type)
        { questCount = Math.Min(++questCount, clearCount); }
    }   //Ÿ���� ������ ī��Ʈ ����, ����ƮUI ����
    public bool ClearQuestCheck()
    {
        return clearCount == questCount ? true : false;
    }
}

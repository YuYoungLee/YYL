using System;
using System.Collections.Generic;

public enum EQuestTargetType     //����Ʈ Ÿ���� ����
{
    Kill,              //óġ
    InteractionNPC,    //NPC��ȣ�ۿ�
    Item,              //������ ȹ��
}

public class QuestData
{
    private int miQuestKey;             //����Ʈ Ű��
    private string mstrTitle;           //����Ʈ Ÿ��Ʋ
    private string mstrDescription;     //����Ʈ ����
    private List<int> miTextKey;        //�ؽ�Ʈ ���̾˷α� Ű��
    private List<Tuple<EQuestTargetType, int, int>> mTargetData;     //Ÿ���� ������ <����, ��ǥŰ��, ����>
    private int miRewardTableKey;       //���� ���̺� Ű
    private int miPreviousQuestKey;     //���� ����Ʈ Ű�� ������ -1
    private int miRewardExe;            //����Ʈ �������ġ

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

        //Ÿ�� �����Ͱ� null �� ��
        if (mTargetData == null)
        {
            //����Ʈ ���� �� ������ �����̽�
            mTargetData = new List<Tuple<EQuestTargetType, int, int>>();
            string[] strTargetType = CSVReader.Instance.GetListSlice(data[3]);
            string[] strTargetKey = CSVReader.Instance.GetListSlice(data[4]);
            string[] strTargetCount = CSVReader.Instance.GetListSlice(data[5]);

            //����Ʈ�� Ÿ�� ������ ����
            for(int i = 0; i < strTargetType.Length; ++i)
            {
                mTargetData.Add(Tuple.Create(
                    Enum.Parse<EQuestTargetType>(strTargetType[i]),     //Ÿ���� Ÿ��
                    int.Parse(strTargetKey[i]),                         //Ÿ���� Ű��
                    int.Parse(strTargetCount[i])                        //Ÿ���� ����
                    ));
            }
        }

        if(miTextKey == null)
        {
            //����Ʈ ���� �� ������ �����̽�
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

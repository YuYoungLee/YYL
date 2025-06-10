using System;
public enum QuestAddValueType
{
    Kill,
    Default,
}
public struct QuestData
{
    private int questID;
    private string questTitle;
    private string questExplainText;
    private int priviousID;                         //���� ����Ʈ �� ����
    private int questClearValue;                    //����Ʈ Ŭ���� ī��Ʈ ��
    private QuestAddValueType addValueType;
    private int rewardValue;

    #region Lambda
    public int QuestID => questID;
    public string QuestTitle => questTitle;
    public string QuestExplainText => questExplainText;
    public int PriviousID => priviousID;
    public int QuestClearValue => questClearValue;
    public QuestAddValueType AddValueType => addValueType;
    public int RewardValue => rewardValue;
    #endregion

    public void SetData(string[] values)
    {
        questID = Int32.Parse(values[0]);
        questTitle = values[1];
        questExplainText = values[2];
        priviousID = Int32.Parse(values[3]);
        questClearValue = Int32.Parse(values[4]);
        addValueType = (QuestAddValueType)Enum.Parse(typeof(QuestAddValueType), values[5]);
        rewardValue = Int32.Parse(values[6]);
    }
}

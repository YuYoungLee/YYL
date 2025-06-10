using System.Collections.Generic;

public class Table<T>
{
    private List<T> mTableData;     //���̺� ������
    public List<T> GetTableData => mTableData;      //��ü ����Ʈ
    public T GetData(int iIdx) => mTableData[iIdx];     //idx ��ġ�� ������
    public int GetCount => mTableData.Count;

    public Table()
    {
        if(mTableData == null)
        {
            mTableData = new List<T>();
        }
    }

    /// <summary>
    /// tData : ����Ʈ�� �߰��� ������
    /// </summary>
    public void Add(T tData)
    {
        mTableData.Add(tData);
    }   //������ �߰�
}
using System.Collections.Generic;

public class Table<T>
{
    private List<T> mTableData;     //테이블 데이터
    public List<T> GetTableData => mTableData;      //전체 리스트
    public T GetData(int iIdx) => mTableData[iIdx];     //idx 위치의 데이터
    public int GetCount => mTableData.Count;

    public Table()
    {
        if(mTableData == null)
        {
            mTableData = new List<T>();
        }
    }

    /// <summary>
    /// tData : 리스트에 추가할 데이터
    /// </summary>
    public void Add(T tData)
    {
        mTableData.Add(tData);
    }   //데이터 추가
}
using UnityEngine;
using System.Text.RegularExpressions;

public class CSVReader : MonoBehaviourSingleton<CSVReader>
{
    private static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";   //�ٹٲ�
    private static string SPLIT_RE = @",";   //, ������
    private static string SPLIT_LIST = @"/";    // /���� ����Ʈ

    protected void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Initialize();
        }
    }
    public override void Initialize(){}

    /// <summary>
    /// path : ���� ��� + �����̸�, ex : Data/PlayerData
    /// </summary>
    public string[] LoadCSV(string path)
    {
        TextAsset data = Resources.Load(path) as TextAsset;     //�ؽ�Ʈ �������� �Ľ��Ͽ� ����
        return Regex.Split(data.text, LINE_SPLIT_RE);           //LINE_SPLIT_RE ������ ���� ����
    }   //���� ��ο� �ִ� �����͸� �������� ����

    /// <summary>
    /// csvData : ,������ �ڸ� ������
    /// </summary>
    public string[] GetLineSlice(string csvData)
    {
        return Regex.Split(csvData, SPLIT_RE);
    }   //',' �����ڷ� �߶� ����

    /// <summary>
    /// csvData : /������ �ڸ� ������
    /// </summary>
    public string[] GetListSlice(string csvData)
    {
        return Regex.Split(csvData, SPLIT_LIST);
    }   //'/' �����ڷ� �߶� ����
}

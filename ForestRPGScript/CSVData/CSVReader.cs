using UnityEngine;
using System.Text.RegularExpressions;

public class CSVReader : MonoBehaviourSingleton<CSVReader>
{
    private static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";   //줄바꿈
    private static string SPLIT_RE = @",";   //, 구분자
    private static string SPLIT_LIST = @"/";    // /구분 리스트

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
    /// path : 파일 경로 + 파일이름, ex : Data/PlayerData
    /// </summary>
    public string[] LoadCSV(string path)
    {
        TextAsset data = Resources.Load(path) as TextAsset;     //텍스트 에셋으로 파싱하여 저장
        return Regex.Split(data.text, LINE_SPLIT_RE);           //LINE_SPLIT_RE 단위로 끊어 리턴
    }   //파일 경로에 있는 데이터를 열단위로 리턴

    /// <summary>
    /// csvData : ,단위로 자를 데이터
    /// </summary>
    public string[] GetLineSlice(string csvData)
    {
        return Regex.Split(csvData, SPLIT_RE);
    }   //',' 구분자로 잘라 리턴

    /// <summary>
    /// csvData : /단위로 자를 데이터
    /// </summary>
    public string[] GetListSlice(string csvData)
    {
        return Regex.Split(csvData, SPLIT_LIST);
    }   //'/' 구분자로 잘라 리턴
}

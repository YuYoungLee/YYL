using UnityEngine;
using System.Text.RegularExpressions;

public class CSVReader
{
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

    public static string[] Read(string file)
    {
        TextAsset data = Resources.Load(file) as TextAsset;
        return Regex.Split(data.text, LINE_SPLIT_RE);
    }
}

using System.Collections.Generic;


public class SkillManager
{
    private Dictionary<int, SkillData> skillData = null;
    private string path = "CSVData/SkillData";              //스킬데이터 경로

    public void Initialize()
    {
        //데이터가 비어 있으면
        if (skillData == null)
        { skillData = new Dictionary<int, SkillData>(); }
        
        //SetData();
    }

    public void SetData()
    {
        string[] data = new string[11];              //스킬 데이터 문자열
        data = CSVReader.Instance.LoadCSV(path);   //스킬 데이터 불러오기
        for(int i = 1; i < data.Length; ++i)
        {
            SkillData skill = new SkillData();      //스킬 데이터 생성
            skill.SetData(CSVReader.Instance.GetLineSlice(data[i]));    //데이터 삽입;
            skillData[i] = skill;
        }
    }   //스킬 데이터 삽입

    public SkillData GetSkillData(int key)
    {
        return skillData[key];
    }   //스킬 데이터

    public bool SkillKeyCheck(int key)
    {
        return skillData.ContainsKey(key);
    }
}

using System.Collections.Generic;


public class SkillManager
{
    private Dictionary<int, SkillData> skillData = null;
    private string path = "CSVData/SkillData";              //��ų������ ���

    public void Initialize()
    {
        //�����Ͱ� ��� ������
        if (skillData == null)
        { skillData = new Dictionary<int, SkillData>(); }
        
        //SetData();
    }

    public void SetData()
    {
        string[] data = new string[11];              //��ų ������ ���ڿ�
        data = CSVReader.Instance.LoadCSV(path);   //��ų ������ �ҷ�����
        for(int i = 1; i < data.Length; ++i)
        {
            SkillData skill = new SkillData();      //��ų ������ ����
            skill.SetData(CSVReader.Instance.GetLineSlice(data[i]));    //������ ����;
            skillData[i] = skill;
        }
    }   //��ų ������ ����

    public SkillData GetSkillData(int key)
    {
        return skillData[key];
    }   //��ų ������

    public bool SkillKeyCheck(int key)
    {
        return skillData.ContainsKey(key);
    }
}

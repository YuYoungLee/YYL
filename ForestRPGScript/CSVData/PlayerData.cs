using System;
using System.Collections.Generic;

public enum EClassType
{
    None,
    Warrior,
}

public class StatData
{
    protected int maxHp;      //최대 체력
    protected int maxMp;      //최대 마나
    protected int maxExe;     //최대 경험치
    protected int miATK;      //공격력 주 클래스 스텟이 공격력으로
    protected int miDEF;      //방어력

    public int MaxHp => maxHp;
    public int MaxMp => maxMp;
    public int MaxExe => maxExe;
    public int Atk => miATK;
    public int Def => miDEF;
}

public class PlayerData : StatData
{
    private int miKey;              //키값
    private EClassType meClassType; //클래스 타입
    private float damageRate;       //데미지 배율
    private int miSTR;              //힘
    private int miDEX;              //민첩
    private int miINT;              //지능
    private List<int> miSkillKey;   //스킬 키값

    public int Key => miKey;
    public EClassType ClassType => meClassType;
    public float DamageRate => damageRate;
    public int Strength => miSTR;
    public int Dexterity => miDEX;
    public int Intelligence => miINT;
    public List<int> SkillKeyList => miSkillKey;
    public int SkillKey(int iIdx) => miSkillKey[iIdx];

    public void SetData(string[] data)
    {
        miKey = int.Parse(data[0]);
        meClassType = Enum.Parse<EClassType>(data[1]);
        maxHp = int.Parse(data[2]);
        maxMp = int.Parse(data[3]);
        maxExe = int.Parse(data[4]);
        miATK = int.Parse(data[5]);
        miDEF = int.Parse(data[6]);
        damageRate = float.Parse(data[7]);
        miSTR = int.Parse(data[8]);
        miDEX = int.Parse(data[9]);
        miINT = int.Parse(data[10]);

        //스킬 키
        if(miSkillKey == null)
        {
            miSkillKey = new List<int>();
            string[] listData = CSVReader.Instance.GetListSlice(data[11]);
            for(int i = 0; i < listData.Length; ++i)
            {
                miSkillKey.Add(int.Parse(listData[i]));
            }
        }
    }
}

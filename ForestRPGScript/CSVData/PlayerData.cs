using System;
using System.Collections.Generic;

public enum EClassType
{
    None,
    Warrior,
}

public class StatData
{
    protected int maxHp;      //�ִ� ü��
    protected int maxMp;      //�ִ� ����
    protected int maxExe;     //�ִ� ����ġ
    protected int miATK;      //���ݷ� �� Ŭ���� ������ ���ݷ�����
    protected int miDEF;      //����

    public int MaxHp => maxHp;
    public int MaxMp => maxMp;
    public int MaxExe => maxExe;
    public int Atk => miATK;
    public int Def => miDEF;
}

public class PlayerData : StatData
{
    private int miKey;              //Ű��
    private EClassType meClassType; //Ŭ���� Ÿ��
    private float damageRate;       //������ ����
    private int miSTR;              //��
    private int miDEX;              //��ø
    private int miINT;              //����
    private List<int> miSkillKey;   //��ų Ű��

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

        //��ų Ű
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

using System;
using System.Collections.Generic;

public class EnemyData
{
    private int miKey;              //���ʹ� Ű��
    private string mstrName;        //���ʹ� �̸�
    private int miMaxHp;            //�ִ� ü��
    private int miAttacker;         //���ݷ�
    private int miDefense;          //����
    private float mfHitDeleay;      //���� ������
    private List<int> mSkillKey;    //��ų Ű��
    private int miGetExe;           //������ ���� ����ġ
    private int miDropTableKey;     //��� ���̺� Ű
    private string mstrAttackSoundKey;
    private string mstrHitSoundKey;
    private EEnemyType meEnemyType;

    public int Key => miKey;
    public string Name => mstrName;
    public int MaxHp => miMaxHp;
    public int Attacker => miAttacker;
    public int Defense => miDefense;
    public float HitDeleay => mfHitDeleay;
    public List<int> SkillKey => mSkillKey;
    public int GetExe => miGetExe;
    public int DropTableKey => miDropTableKey;
    public EEnemyType GetEnemyType() => meEnemyType;

    public void SetData(string[] strData)
    {
        miKey = int.Parse(strData[0]);         //���ʹ� Ű
        mstrName = strData[1];
        miMaxHp = int.Parse(strData[2]);
        miAttacker = int.Parse(strData[3]);    //���ݷ�
        miDefense = int.Parse(strData[4]);     //����
        mfHitDeleay = float.Parse(strData[5]);      //���� ������

        //��ų Ű
        if(mSkillKey == null)
        {
            mSkillKey = new List<int>();
            //��ų Ű�� �����̽�
            string[] strKeySlice = CSVReader.Instance.GetListSlice(strData[6]);
            for (int i = 0; i < strKeySlice.Length; ++i)
            {
                mSkillKey.Add(int.Parse(strKeySlice[i]));
            }
        }

        miGetExe = int.Parse(strData[7]);              //������ ���� ����ġ
        miDropTableKey = int.Parse(strData[8]);        //��� ���̺� Ű

        mstrAttackSoundKey = strData[9];
        mstrHitSoundKey = strData[10];

        meEnemyType = Enum.Parse<EEnemyType>(strData[11]);
    }
}

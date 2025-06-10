using System;
using System.Collections.Generic;
using UnityEngine;

public enum EAttackType
{
    Melee,     //��������
    Buff,       //����
    Magic,      //����
}

public enum EFireType
{
    Default,            //������
    ThreeWay,           //3���� �߻�
    Forward,            //n�� ����
    DropTarget,         //Ÿ�� ���
    TargetSpawn,        //Ÿ�� ��ġ�� ����
}

public class SkillData
{
    private int miKey;                //��ų ������ key
    private EAttackType meAttackType; //���� Ÿ��
    private string mstrName;          //��ų �̸�
    private string mstrExplanation;   //����
    private float mfAngle;            //��ų�� ���� ����
    private float mfStartDist;        //���� ��ġ
    private float mfEndDist;          //�� ��ġ
    private float mfStartTime;        //���� �ð�
    private float mfRepeatTime;       //�ݺ� �ð�
    private int miDurationCount;      //�ݺ� Ƚ��
    private int miManaCost;           //���� �ڽ�Ʈ
    private float mfCoolTime;         //��Ÿ��(��)
    private int miDamage;             //��ų ������
    private EStatDataType meStatDataType;//���� Ÿ��
    private EPlayerAnim mePlayerAnim;    //�÷��̾� ���ϸ��̼� ��� Ÿ��
    private List<int> mPooledOBJKey;     //Ǯ�� Ű��
    private List<int> mFieldParticleKey;  //�ʵ� ����Ʈ Ű��
    private List<int> mSkillParticleKey;  //���� ����Ʈ Ű��
    private Sprite mSkillIcon;      //��ų ������
    private String mstrSkillSoundKey;   //��ų ���� Ű��
    private float mfStopTime;        //��ų ���� �ð�
    private EFireType meFireType;   //�߻� Ÿ��

    public int Key => miKey;
    public EAttackType AttackType => meAttackType;
    public string Name => mstrName;
    public string Explanation => mstrExplanation;
    public float Angle => mfAngle;
    public float StartDist => mfStartDist;
    public float EndDist => mfEndDist;
    public float StartTime => mfStartTime;
    public float RepeatTime => mfRepeatTime;
    public int DurationCount => miDurationCount;
    public int ManaCost => miManaCost;
    public float CoolTime => mfCoolTime;
    public int Damage => miDamage;
    public EStatDataType StatDataType => meStatDataType;
    public EPlayerAnim PlayerAnim => mePlayerAnim;
    public List<int> PooledObjectKey => mPooledOBJKey;
    public List<int> FieldParticleKey () => mFieldParticleKey;
    public List<int> SkillParticleKey () => mSkillParticleKey;
    public Sprite SkillIcon => mSkillIcon;
    public String SkillSoundKey => mstrSkillSoundKey;

    public EFireType FireType => meFireType;

    public float StopTime => mfStopTime;


    /// <summary>
    /// csvData : csv �ε��� ������
    /// </summary>
    public void SetData(string[] csvData)
    {
        miKey = int.Parse(csvData[0]);
        meAttackType = Enum.Parse<EAttackType>(csvData[1]);
        mstrName = csvData[2];
        mstrExplanation = csvData[3];
        mfAngle = float.Parse(csvData[4]);
        mfStartDist = float.Parse(csvData[5]);
        mfEndDist = float.Parse(csvData[6]);
        mfStartTime = float.Parse(csvData[7]);
        mfRepeatTime = float.Parse(csvData[8]);
        miDurationCount = int.Parse(csvData[9]);
        miManaCost = int.Parse(csvData[10]);
        mfCoolTime = float.Parse(csvData[11]);
        miDamage = int.Parse(csvData[12]);
        meStatDataType = Enum.Parse<EStatDataType>(csvData[13]);
        mePlayerAnim = Enum.Parse<EPlayerAnim>(csvData[14]);

        //Ǯ�� ������Ʈ Ű
        mPooledOBJKey = new List<int>();
        ListAddData(mPooledOBJKey, CSVReader.Instance.GetListSlice(csvData[15]));

        //�ʵ� ����Ʈ Ű
        mFieldParticleKey = new List<int>();
        ListAddData(mFieldParticleKey, CSVReader.Instance.GetListSlice(csvData[16]));

        //���� ����Ʈ Ű
        mSkillParticleKey = new List<int>();
        ListAddData(mSkillParticleKey, CSVReader.Instance.GetListSlice(csvData[17]));

        //��ų ������
        mSkillIcon = Resources.Load<Sprite>(csvData[18]);

        //���� Ű��
        mstrSkillSoundKey = csvData[19];

        //���� �ð�
        mfStopTime = float.Parse(csvData[20]);

        //�߻�Ÿ��
        meFireType = Enum.Parse<EFireType>(csvData[21]);
    }   //������ ����

    /// <summary>
    /// listData : ������ ������ �ɹ��� ����Ʈ
    /// strListData : ����Ʈ ������ csv ������
    /// </summary>
    private void ListAddData(List<int> listData, string[] strListData)
    {
        //��� ������ ����
        if (strListData[0] == "")
        {
            return;
        }

        //������ ����
        for (int i = 0; i < strListData.Length; ++i)
        {
            listData.Add(int.Parse(strListData[i]));
        }
    }   //����Ʈ�� ������ ����
}
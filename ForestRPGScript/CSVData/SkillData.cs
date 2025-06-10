using System;
using System.Collections.Generic;
using UnityEngine;

public enum EAttackType
{
    Melee,     //물리공격
    Buff,       //버프
    Magic,      //마법
}

public enum EFireType
{
    Default,            //모드없음
    ThreeWay,           //3방향 발사
    Forward,            //n개 직진
    DropTarget,         //타겟 드랍
    TargetSpawn,        //타겟 위치에 스폰
}

public class SkillData
{
    private int miKey;                //스킬 구분할 key
    private EAttackType meAttackType; //공격 타입
    private string mstrName;          //스킬 이름
    private string mstrExplanation;   //설명
    private float mfAngle;            //스킬의 각도 범위
    private float mfStartDist;        //시작 위치
    private float mfEndDist;          //끝 위치
    private float mfStartTime;        //시작 시간
    private float mfRepeatTime;       //반복 시간
    private int miDurationCount;      //반복 횟수
    private int miManaCost;           //마나 코스트
    private float mfCoolTime;         //쿨타임(초)
    private int miDamage;             //스킬 데미지
    private EStatDataType meStatDataType;//스텟 타입
    private EPlayerAnim mePlayerAnim;    //플레이어 에니매이션 출력 타입
    private List<int> mPooledOBJKey;     //풀링 키값
    private List<int> mFieldParticleKey;  //필드 이펙트 키값
    private List<int> mSkillParticleKey;  //공격 이펙트 키값
    private Sprite mSkillIcon;      //스킬 아이콘
    private String mstrSkillSoundKey;   //스킬 사운드 키값
    private float mfStopTime;        //스킬 멈춤 시간
    private EFireType meFireType;   //발사 타입

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
    /// csvData : csv 로드한 데이터
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

        //풀링 오브젝트 키
        mPooledOBJKey = new List<int>();
        ListAddData(mPooledOBJKey, CSVReader.Instance.GetListSlice(csvData[15]));

        //필드 이펙트 키
        mFieldParticleKey = new List<int>();
        ListAddData(mFieldParticleKey, CSVReader.Instance.GetListSlice(csvData[16]));

        //공격 이펙트 키
        mSkillParticleKey = new List<int>();
        ListAddData(mSkillParticleKey, CSVReader.Instance.GetListSlice(csvData[17]));

        //스킬 아이콘
        mSkillIcon = Resources.Load<Sprite>(csvData[18]);

        //사운드 키값
        mstrSkillSoundKey = csvData[19];

        //멈춤 시간
        mfStopTime = float.Parse(csvData[20]);

        //발사타입
        meFireType = Enum.Parse<EFireType>(csvData[21]);
    }   //데이터 삽입

    /// <summary>
    /// listData : 데이터 삽입할 맴버의 리스트
    /// strListData : 리스트 삽입할 csv 데이터
    /// </summary>
    private void ListAddData(List<int> listData, string[] strListData)
    {
        //비어 있으면 리턴
        if (strListData[0] == "")
        {
            return;
        }

        //데이터 삽입
        for (int i = 0; i < strListData.Length; ++i)
        {
            listData.Add(int.Parse(strListData[i]));
        }
    }   //리스트에 데이터 삽입
}
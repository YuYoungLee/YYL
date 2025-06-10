using System;
using UnityEngine;

public enum EStatDataType
{
    None = 0,
    HP,     //체력
    MP,     //마나
    EXE,    //경험치
    ATK,    //공격력
    DEF,    //방어력
    STR,    //힘
    DEX,    //민첩
    INT,    //지능
    Money,  //돈
}

public class Stat
{
    protected int miHP;               //체력
    protected int miMaxHP;            //최대 체력
    protected int miMP;               //마나
    protected int miMaxMP;            //최대 마나
    protected int miATK;              //공격력
    protected int miDEF;              //방어력

    public int HP => miHP;
    public int MP => miMP;
    public int Atk => miATK;
    public int Def => miDEF;

    public virtual void Initialize(int iKey)
    {
        DataManager dataMgr = GameManager.Instance.GetDataMgr();

        miMaxHP = dataMgr.GetEnemyData(iKey).MaxHp;     //체력
        miHP = miMaxHP;
        miMaxMP = 999999;    //몬스터의 마나 무한
        miMP = miMaxMP;
        miATK = dataMgr.GetEnemyData(iKey).Attacker;    //공격력
        miDEF = dataMgr.GetEnemyData(iKey).Defense;     //방어력
    }

    public void ResetData()
    {
        miHP = miMaxHP;
        miMP = miMaxMP;
    }   //hp, mp 초기화

    /// <summary>
    /// damage : 입을 데미지
    /// </summary>
    public bool Damaged(int damage)
    {
        //(데미지 - 방어력), 최소 공격력 1
        //(체력 - 데미지), 최소 체력 0
        miHP = Mathf.Max(miHP - Mathf.Max(damage - miDEF, 1), 0);
        if(miHP > 0) 
        { 
            return false;
        }
        return true;
    }   //체력이 없으면 true;

    /// <summary>
    /// eStatType : 스텟의 타입
    /// iStat : 증가시킬 스텟 값
    /// </summary>
    public virtual void Add(EStatDataType eStatType, int iStat)
    {
        switch (eStatType)
        {
            case EStatDataType.HP:
                miHP = Mathf.Min(miHP + iStat, miMaxHP);
                break;
            case EStatDataType.MP:
                miMP = Mathf.Min(miMP + iStat, miMaxMP);
                break;
            case EStatDataType.ATK:
                miATK = Mathf.Min(miATK + iStat, Int32.MaxValue);
                break;
            case EStatDataType.DEF:
                miDEF = Mathf.Min(miDEF + iStat, Int32.MaxValue);
                break;
        }
    }   //타입에 맞는 스텟 증가

    /// <summary>
    /// eStatType : 스텟의 타입
    /// iStat : 감소시킬 스텟 값
    /// </summary>
    public virtual bool Sum(EStatDataType eStatType, int iStat)
    {
        switch (eStatType)
        {
            case EStatDataType.HP:
                miHP = Mathf.Max(miHP - iStat, 0);
                if(miHP == 0)
                {
                    return false;   //HP량이 작다면 false 리턴
                }
                break;
            case EStatDataType.MP:
                if(iStat > miMP)    //마나량이 작다면 false 리턴
                { 
                    return false; 
                }
                miMP -= iStat;
                break;
            case EStatDataType.ATK:
                miATK = Mathf.Max(miATK - iStat, 0);
                break;
            case EStatDataType.DEF:
                miDEF = Mathf.Max(miDEF - iStat, 0);
                break;
        }
        return true;
    }   //타입에 맞는 스텟 감소
}

public class PlayerStat : Stat
{
    private EClassType meClassType;
    private int miLevel;              //레벨
    private int miEXE;                //경험치
    private int miMaxEXE;             //최대 경험치
    private float mfDamageRate;       //데미지 배율
    private int miStrength;           //힘
    private int miDexterity;          //민첩
    private int miIntelligence;       //지능
    private int miMoney;              //돈

    private PlayerPanel mPlayerPanel;
    public int Level => miLevel;
    public int MaxHP => miMaxHP;
    public int MaxMP => miMaxMP;
    public int Exe => miEXE;
    public int MaxExe => miMaxEXE;
    public float DamageRate => mfDamageRate;
    public int Strength => miStrength;
    public int Dexterity => miDexterity;
    public int Intelligence => miIntelligence;
    public int Money => miMoney;

    public override void Initialize(int iKey)
    {
        if(mPlayerPanel == null)
        {
            mPlayerPanel = (UIManager.Instance.GetGUI(EGUI.Player) as PlayerPanel);
        }

        ResetData(iKey);    //데이터 초기화

        //UI 설정
        PlayerPanel playerPanel = UIManager.Instance.GetGUI(EGUI.Player) as PlayerPanel;
        playerPanel.SetValue(EPlayerSlider.HP, miMaxHP, miHP);
        playerPanel.SetValue(EPlayerSlider.MP, miMaxMP, miMP);
        playerPanel.SetValue(EPlayerSlider.EXE, miMaxEXE, miEXE);
    }

    /// <summary>
    /// stat : 스텟 개수
    /// type : 스텟의 타입
    /// </summary>
    public override void Add(EStatDataType eStat, int iStat)
    {
        switch (eStat)
        {
            case EStatDataType.EXE:
                miEXE += iStat;
                while(miMaxEXE <= miEXE)    //경험치가 더 많을 경우
                {
                    miEXE -= miMaxEXE;      //경험치 감소
                    LevelUp();     //레벨 업
                    mPlayerPanel.PlayLevelUp();
                }
                mPlayerPanel.SliderAnim(EPlayerSlider.EXE, miEXE);    //스텟 anim
                break;
            case EStatDataType.STR:
                miStrength = Mathf.Min(miStrength + iStat, Int32.MaxValue);
                SetATK();
                break;
            case EStatDataType.DEX:
                miDexterity = Mathf.Min(miDexterity + iStat, Int32.MaxValue);
                SetATK();
                break;
            case EStatDataType.INT:
                miIntelligence = Mathf.Min(miIntelligence + iStat, Int32.MaxValue);
                SetATK();
                break;
            case EStatDataType.Money:
                miMoney += iStat;
                EventManager.Instance.AddValueEvent(EEventMessage.SetMoneyUI, miMoney);
                break;
            case EStatDataType.HP:
                base.Add(eStat, iStat);
                mPlayerPanel.SliderAnim(EPlayerSlider.HP, miHP);    //스텟 anim
                break;
            case EStatDataType.MP:
                base.Add(eStat, iStat);
                mPlayerPanel.SliderAnim(EPlayerSlider.MP, miMP);
                break;
            default :
                base.Add(eStat, iStat);
                break;
        }

        (UIManager.Instance.GetGUI(EGUI.Equipment) as Equipment).SetStatText();     //장비창 스텟 적용
    }   //타입에 맞게 스텟 증가

    /// <summary>
    /// stat : 스텟 개수
    /// type : 스텟의 타입
    /// </summary>
    public override bool Sum(EStatDataType eStat, int iStat)
    {
        switch (eStat)
        {
            case EStatDataType.STR:
                miStrength = Mathf.Max(miStrength - iStat, 0);
                SetATK();
                break;
            case EStatDataType.DEX:
                miDexterity = Mathf.Max(miDexterity - iStat, 0);
                SetATK();
                break;
            case EStatDataType.INT:
                miIntelligence = Mathf.Max(miIntelligence - iStat, 0);
                SetATK();
                break;
            case EStatDataType.Money:
                //뺀 값이 0보다 작으면 false
                if(miMoney - iStat < 0)
                {
                    return false;
                }
                miMoney -= iStat;
                EventManager.Instance.AddValueEvent(EEventMessage.SetMoneyUI, miMoney);
                break;
            case EStatDataType.HP:
                base.Sum(eStat, iStat);
                mPlayerPanel.SliderAnim(EPlayerSlider.HP, miHP);    //체력 표시 anim
                break;
            case EStatDataType.MP:
                base.Sum(eStat, iStat);
                mPlayerPanel.SliderAnim(EPlayerSlider.MP, miMP);    //마나 표시 anim
                break;
            default:
                if(base.Sum(eStat, iStat))
                {
                    SetPlayerUI();    //UI변경
                }
                break;
        }

        (UIManager.Instance.GetGUI(EGUI.Equipment) as Equipment).SetStatText();     //장비창 스텟 적용
        return true;
    }   //타입에 맞게 스텟 감소

    private void LevelUp()
    {
        ++miLevel;
        miMaxEXE = (int)(miMaxEXE * 1.3f);
        miMaxHP = (int)(miMaxHP * 1.3f);        //체력
        miHP = miMaxHP;
        miMaxMP = (int)(miMaxMP * 1.3f);        //마나
        miMP = miMaxMP;

        //UI변경
        SetPlayerUI();

        //플레이어 스킬레벨 증가
        (UIManager.Instance.GetGUI(EGUI.Skill) as SkillPanel).AddSkillCount();

        //스텟 증가
        switch (meClassType)
        {
            case EClassType.Warrior:
                Add(EStatDataType.STR, 5);
                Add(EStatDataType.DEF, 3);
                Add(EStatDataType.STR, 3);
                Add(EStatDataType.DEX, 1);
                Add(EStatDataType.INT, 1);
                break;
        }
    }   //래벨업 시 데이터 설정

    private void SetATK()
    {
        miATK = 0;
        //주 스텟 계산 * 1.5배
        switch (meClassType)
        {
            case EClassType.Warrior:
                miATK += (int)(miStrength * 1.5f);
                break;
        }
        //나머지 스텟 계산
        miATK += miDexterity;
        miATK += miIntelligence;
    }   //스텟을 기준으로 atk 계산

    private void SetPlayerUI()
    {
        //UI변경
        PlayerPanel playerPanel = UIManager.Instance.GetGUI(EGUI.Player) as PlayerPanel;
        playerPanel.SetValue(EPlayerSlider.HP, miMaxHP, miHP);
        playerPanel.SetValue(EPlayerSlider.MP, miMaxMP, miMP);
        playerPanel.SetValue(EPlayerSlider.EXE, miMaxEXE, miEXE);
    }

    public void ResetData(int iKey)
    {
        //Data 삽입
        PlayerData data = GameManager.Instance.GetDataMgr().GetPlayerData(iKey);

        meClassType = data.ClassType;
        miLevel = 1;
        miMoney = 0;
        miHP = data.MaxHp;
        miMaxHP = data.MaxHp;
        miMP = data.MaxMp;
        miMaxMP = data.MaxMp;
        miMaxEXE = data.MaxExe;
        miEXE = 0;
        miATK = data.Atk;
        miDEF = data.Def;
        mfDamageRate = data.DamageRate;
        miStrength = data.Strength;
        miDexterity = data.Dexterity;
        miIntelligence = data.Intelligence;
        SetATK();
    }   //데이터 리셋

    public void LoadData(SaveData saveData)
    {
        for(int i = 1; i < saveData.miPlayerLevel; ++i)
        {
            LevelUp();      //레벨업
        }

        Add(EStatDataType.Money, saveData.miMoney);
        Add(EStatDataType.EXE, saveData.miExe);

        SetPlayerUI();      //플레이어 UI 설정
    }
}
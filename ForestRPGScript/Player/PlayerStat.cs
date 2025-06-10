using System;
using UnityEngine;

public enum EStatDataType
{
    None = 0,
    HP,     //ü��
    MP,     //����
    EXE,    //����ġ
    ATK,    //���ݷ�
    DEF,    //����
    STR,    //��
    DEX,    //��ø
    INT,    //����
    Money,  //��
}

public class Stat
{
    protected int miHP;               //ü��
    protected int miMaxHP;            //�ִ� ü��
    protected int miMP;               //����
    protected int miMaxMP;            //�ִ� ����
    protected int miATK;              //���ݷ�
    protected int miDEF;              //����

    public int HP => miHP;
    public int MP => miMP;
    public int Atk => miATK;
    public int Def => miDEF;

    public virtual void Initialize(int iKey)
    {
        DataManager dataMgr = GameManager.Instance.GetDataMgr();

        miMaxHP = dataMgr.GetEnemyData(iKey).MaxHp;     //ü��
        miHP = miMaxHP;
        miMaxMP = 999999;    //������ ���� ����
        miMP = miMaxMP;
        miATK = dataMgr.GetEnemyData(iKey).Attacker;    //���ݷ�
        miDEF = dataMgr.GetEnemyData(iKey).Defense;     //����
    }

    public void ResetData()
    {
        miHP = miMaxHP;
        miMP = miMaxMP;
    }   //hp, mp �ʱ�ȭ

    /// <summary>
    /// damage : ���� ������
    /// </summary>
    public bool Damaged(int damage)
    {
        //(������ - ����), �ּ� ���ݷ� 1
        //(ü�� - ������), �ּ� ü�� 0
        miHP = Mathf.Max(miHP - Mathf.Max(damage - miDEF, 1), 0);
        if(miHP > 0) 
        { 
            return false;
        }
        return true;
    }   //ü���� ������ true;

    /// <summary>
    /// eStatType : ������ Ÿ��
    /// iStat : ������ų ���� ��
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
    }   //Ÿ�Կ� �´� ���� ����

    /// <summary>
    /// eStatType : ������ Ÿ��
    /// iStat : ���ҽ�ų ���� ��
    /// </summary>
    public virtual bool Sum(EStatDataType eStatType, int iStat)
    {
        switch (eStatType)
        {
            case EStatDataType.HP:
                miHP = Mathf.Max(miHP - iStat, 0);
                if(miHP == 0)
                {
                    return false;   //HP���� �۴ٸ� false ����
                }
                break;
            case EStatDataType.MP:
                if(iStat > miMP)    //�������� �۴ٸ� false ����
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
    }   //Ÿ�Կ� �´� ���� ����
}

public class PlayerStat : Stat
{
    private EClassType meClassType;
    private int miLevel;              //����
    private int miEXE;                //����ġ
    private int miMaxEXE;             //�ִ� ����ġ
    private float mfDamageRate;       //������ ����
    private int miStrength;           //��
    private int miDexterity;          //��ø
    private int miIntelligence;       //����
    private int miMoney;              //��

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

        ResetData(iKey);    //������ �ʱ�ȭ

        //UI ����
        PlayerPanel playerPanel = UIManager.Instance.GetGUI(EGUI.Player) as PlayerPanel;
        playerPanel.SetValue(EPlayerSlider.HP, miMaxHP, miHP);
        playerPanel.SetValue(EPlayerSlider.MP, miMaxMP, miMP);
        playerPanel.SetValue(EPlayerSlider.EXE, miMaxEXE, miEXE);
    }

    /// <summary>
    /// stat : ���� ����
    /// type : ������ Ÿ��
    /// </summary>
    public override void Add(EStatDataType eStat, int iStat)
    {
        switch (eStat)
        {
            case EStatDataType.EXE:
                miEXE += iStat;
                while(miMaxEXE <= miEXE)    //����ġ�� �� ���� ���
                {
                    miEXE -= miMaxEXE;      //����ġ ����
                    LevelUp();     //���� ��
                    mPlayerPanel.PlayLevelUp();
                }
                mPlayerPanel.SliderAnim(EPlayerSlider.EXE, miEXE);    //���� anim
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
                mPlayerPanel.SliderAnim(EPlayerSlider.HP, miHP);    //���� anim
                break;
            case EStatDataType.MP:
                base.Add(eStat, iStat);
                mPlayerPanel.SliderAnim(EPlayerSlider.MP, miMP);
                break;
            default :
                base.Add(eStat, iStat);
                break;
        }

        (UIManager.Instance.GetGUI(EGUI.Equipment) as Equipment).SetStatText();     //���â ���� ����
    }   //Ÿ�Կ� �°� ���� ����

    /// <summary>
    /// stat : ���� ����
    /// type : ������ Ÿ��
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
                //�� ���� 0���� ������ false
                if(miMoney - iStat < 0)
                {
                    return false;
                }
                miMoney -= iStat;
                EventManager.Instance.AddValueEvent(EEventMessage.SetMoneyUI, miMoney);
                break;
            case EStatDataType.HP:
                base.Sum(eStat, iStat);
                mPlayerPanel.SliderAnim(EPlayerSlider.HP, miHP);    //ü�� ǥ�� anim
                break;
            case EStatDataType.MP:
                base.Sum(eStat, iStat);
                mPlayerPanel.SliderAnim(EPlayerSlider.MP, miMP);    //���� ǥ�� anim
                break;
            default:
                if(base.Sum(eStat, iStat))
                {
                    SetPlayerUI();    //UI����
                }
                break;
        }

        (UIManager.Instance.GetGUI(EGUI.Equipment) as Equipment).SetStatText();     //���â ���� ����
        return true;
    }   //Ÿ�Կ� �°� ���� ����

    private void LevelUp()
    {
        ++miLevel;
        miMaxEXE = (int)(miMaxEXE * 1.3f);
        miMaxHP = (int)(miMaxHP * 1.3f);        //ü��
        miHP = miMaxHP;
        miMaxMP = (int)(miMaxMP * 1.3f);        //����
        miMP = miMaxMP;

        //UI����
        SetPlayerUI();

        //�÷��̾� ��ų���� ����
        (UIManager.Instance.GetGUI(EGUI.Skill) as SkillPanel).AddSkillCount();

        //���� ����
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
    }   //������ �� ������ ����

    private void SetATK()
    {
        miATK = 0;
        //�� ���� ��� * 1.5��
        switch (meClassType)
        {
            case EClassType.Warrior:
                miATK += (int)(miStrength * 1.5f);
                break;
        }
        //������ ���� ���
        miATK += miDexterity;
        miATK += miIntelligence;
    }   //������ �������� atk ���

    private void SetPlayerUI()
    {
        //UI����
        PlayerPanel playerPanel = UIManager.Instance.GetGUI(EGUI.Player) as PlayerPanel;
        playerPanel.SetValue(EPlayerSlider.HP, miMaxHP, miHP);
        playerPanel.SetValue(EPlayerSlider.MP, miMaxMP, miMP);
        playerPanel.SetValue(EPlayerSlider.EXE, miMaxEXE, miEXE);
    }

    public void ResetData(int iKey)
    {
        //Data ����
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
    }   //������ ����

    public void LoadData(SaveData saveData)
    {
        for(int i = 1; i < saveData.miPlayerLevel; ++i)
        {
            LevelUp();      //������
        }

        Add(EStatDataType.Money, saveData.miMoney);
        Add(EStatDataType.EXE, saveData.miExe);

        SetPlayerUI();      //�÷��̾� UI ����
    }
}
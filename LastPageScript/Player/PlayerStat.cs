using System.Diagnostics;
using UnityEngine;
public class PlayerStat
{
    private int hp;                  //���� ü��
    private int hpMax;               //�ִ� ä��
    private int level;               //���� ����
    private int exe;                 //���� ����ġ
    private int exeMax;              //���� ������ �ִ� ����ġ
    private int damage;              //���ݷ�
    private int defence;             //����
    private float attackSpeed;       //���� �ӵ�
    private float moveSpeed;         //�̵� �ӵ�
    private int health;              //�ʴ� ȸ����
    private float criticalRate;      //ũ��Ƽ�� ����
    private int criticalDamage;      //ũ��Ƽ�� ������
    private int money;

    public int Hp => hp;
    public int HpMax => hpMax;
    public int Level => level;
    public int Exe => exe;
    public int ExeMax => exeMax;
    public int Damage => damage;
    public int Defence => defence;
    public float AttackSpeed => attackSpeed;
    public float MoveSpeed => moveSpeed;
    public int Health => health;
    public float CriticalRate => criticalRate;
    public int CriticalDamage => criticalDamage;
    public int Money => money;

    public void SetData(WarriorData data)
    {
        hp = data.Hp;
        exe = 0;
        hpMax = data.Hp;
        exeMax = data.ExeMax;
        level = data.Level;
        damage = data.Damage;
        defence = data.Defence;
        attackSpeed = data.AttackSpeed;
        moveSpeed = data.MoveSpeed;
        health = data.Health;
        criticalRate = data.CriticalRate;
        criticalDamage = data.CriticalDamage;
    }

    public void AddExe(int addExe)
    {
        exe += addExe;
        while (exe >= exeMax)
        {
            exe -= exeMax;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        //�ִ� ü��, ����ġ, ���� ����
        hpMax = (int)(hpMax * 1.2f);
        hp = hpMax;
        exeMax = (int)(exeMax * 1.2f);
        ++level;
        damage += 10;
        criticalDamage += 5;
        GameManager.Instance.GetUIManager().GetPlayerUI.GetLevelUpPanel.PlayLevelUp(level);
    }

    public void UseItem(Item item, int count)
    {
        switch (item.ValueType)
        {
            case AddValueType.Health:
                hp = Mathf.Min(hp + item.AddValue, hpMax);
                break;
            case AddValueType.MaxHealth:
                hpMax += item.AddValue * count;
                hp = Mathf.Min(hp + item.AddValue, hpMax);
                break;
            case AddValueType.MoveSpeed:
                moveSpeed += item.AddValue * count;
                break;
            case AddValueType.AttackSpeed:
                attackSpeed += item.AddValue * count;
                break;
            case AddValueType.Critical:
                criticalRate += item.AddValue * count;
                break;
            case AddValueType.Defend:
                defence += item.AddValue * count;
                break;
        }
    }   //������ Ÿ�Կ� �°� ���� �߰�

    public void Damaged(int enemyDamege, bool defendStatus)
    {
        //��� ���� ���� 2�� �氨
        if (defendStatus) { enemyDamege = Mathf.Max(enemyDamege - defence * 2, 1); }
        else { enemyDamege = Mathf.Max(enemyDamege - defence, 1); }
        hp = Mathf.Max(hp - enemyDamege, 0);
    }   //������ ���� ��

    public void AddMoney(int addMoney)
    {
        money += addMoney;
    }

    public void Buy(int buy)
    {
        money = Mathf.Max(money - buy, 0);
    }

    public void Load(PlayerData data)
    {
        for(int i = 1; i < data.level; ++i)
        {
            damage += 10;
            criticalDamage += 5;
            hpMax = (int)(hpMax * 1.2f);
            exeMax = (int)(exeMax * 1.2f);
        }
        hp = hpMax;
        level = data.level;
        exe = data.exe;
        money = data.money;
    }
}

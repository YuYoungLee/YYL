using System.Diagnostics;
using UnityEngine;
public class PlayerStat
{
    private int hp;                  //현재 체력
    private int hpMax;               //최대 채력
    private int level;               //현재 래벨
    private int exe;                 //현재 경험치
    private int exeMax;              //현재 레벨당 최대 경험치
    private int damage;              //공격력
    private int defence;             //방어력
    private float attackSpeed;       //공격 속도
    private float moveSpeed;         //이동 속도
    private int health;              //초당 회복량
    private float criticalRate;      //크리티컬 배율
    private int criticalDamage;      //크리티컬 데미지
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
        //최대 체력, 경험치, 레벨 증가
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
    }   //아이템 타입에 맞게 스텟 추가

    public void Damaged(int enemyDamege, bool defendStatus)
    {
        //방어 상태 방어력 2배 경감
        if (defendStatus) { enemyDamege = Mathf.Max(enemyDamege - defence * 2, 1); }
        else { enemyDamege = Mathf.Max(enemyDamege - defence, 1); }
        hp = Mathf.Max(hp - enemyDamege, 0);
    }   //데미지 입을 때

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

using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[CreateAssetMenu(fileName = "WarriorData", menuName = "ScriptableObject/WarriorData Asset", order = 2)]
public class WarriorData : ScriptableObject
{
    [SerializeField] LayerMask targetMask;
    [SerializeField] int hp = 100;
    [SerializeField] int level = 1;
    [SerializeField] int exe = 0;
    [SerializeField] int exeMax = 100;
    [SerializeField] int damage = 10;
    [SerializeField] int defence = 1;
    [SerializeField] float attackSpeed = 1.0f;
    [SerializeField] int moveSpeed = 5;
    [SerializeField] int health = 1;    //회복량
    [SerializeField] float criticalRate = 1.0f;  //크리티컬 배율
    [SerializeField] int criticalDamage = 10;
    [SerializeField] List<SkillData> skillData = new List<SkillData>();

    #region WarriorDataRamda
    public LayerMask TargetMask => targetMask;
    public int Hp => hp;
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
    public List<SkillData> SkillData => skillData;
    public int SkillDataSize => skillData.Count;
    #endregion
}

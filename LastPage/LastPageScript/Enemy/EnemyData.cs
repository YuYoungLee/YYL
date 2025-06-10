using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObject/EnemyData Asset/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    [SerializeField] LayerMask targetMask;      //Ÿ��(�÷��̾�)�� ����ũ
    [SerializeField] [Range(100, 500)] int hp = 100;                //ü��

    [SerializeField] [Range(1.0f, 10.0f)]float attackRange = 1.0f;   //���� �Ÿ�
    [SerializeField] float attackRangeSquare = 100;                 //���� �Ÿ� ����
    [SerializeField] float searchRangeSquare = 10000f;              //Ž�� ���� ����
    [SerializeField] int attackAngle = 15;                          //���� ���� �������
    [SerializeField] [Range(10, 30)]int damege = 10;                //���ݷ�
    [SerializeField] int getExe = 10;                               //(��밡)���� ����ġ
    [SerializeField] [Range(3.0f, 5.0f)]float attackDelay = 3.0f;   //���� ������
    //TODO ������ �־�� �Ҷ�
    [SerializeField] [Range(0.0f, 5.0f)]float hitDelay = 3.0f;      //�¾��� �� ������

    [SerializeField] List<SkillData> skillData = new List<SkillData>();

    public LayerMask TargetMask => targetMask;
    public int HP => hp;
    public float AttackRange => attackRange;
    public float AttackRangeSquare => attackRangeSquare;
    public float SearchRangeSquare => searchRangeSquare;
    public int AttackAngle => attackAngle;
    public int Damege => damege;
    public int GetExe => getExe;
    public float AttackDelay => attackDelay;
    public float HitDelay => hitDelay;
    public List<SkillData> SkillData => skillData;
    public int SkillDataSize => skillData.Count;
}

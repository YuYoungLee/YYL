using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObject/EnemyData Asset/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    [SerializeField] LayerMask targetMask;      //타겟(플레이어)의 마스크
    [SerializeField] [Range(100, 500)] int hp = 100;                //체력

    [SerializeField] [Range(1.0f, 10.0f)]float attackRange = 1.0f;   //공격 거리
    [SerializeField] float attackRangeSquare = 100;                 //공격 거리 제곱
    [SerializeField] float searchRangeSquare = 10000f;              //탐색 범위 제곱
    [SerializeField] int attackAngle = 15;                          //공격 각도 전방기준
    [SerializeField] [Range(10, 30)]int damege = 10;                //공격력
    [SerializeField] int getExe = 10;                               //(상대가)얻을 경험치
    [SerializeField] [Range(3.0f, 5.0f)]float attackDelay = 3.0f;   //공격 딜레이
    //TODO 딜레이 넣어야 할때
    [SerializeField] [Range(0.0f, 5.0f)]float hitDelay = 3.0f;      //맞았을 때 딜레이

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

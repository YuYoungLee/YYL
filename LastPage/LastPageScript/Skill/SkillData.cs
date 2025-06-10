using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObject/SkillData Asset/SkillData", order = 0)]
public class SkillData : ScriptableObject
{

    [SerializeField] bool skillUiStatus = false;            //플레이어 일 때
    [SerializeField] SkillKey skillKey;                     //어떤 키입력을 할 때 사용할 것인지
    [SerializeField][Range(0, 180)] int angle = 0;          //전방기준 공격 각도
    [SerializeField][Range(0f, 10f)] float attackRange = 0; //공격 할 거리
    [SerializeField][Range(0, 10)] int cooltime = 0;    //쿨타임 시간(초)
    [SerializeField] Sprite spriteImage = null;         //스프라이트 이미지
    [SerializeField] Skill skill = null;                //스킬 클래스

    public bool SkillUiStatus => skillUiStatus;
    public SkillKey SkillKey => skillKey;
    public int Angle => angle;
    public float AttackRange => attackRange;
    public int Cooltime => cooltime;
    public Sprite SpriteImage => spriteImage;
    public Skill Skill => skill;
}

using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObject/SkillData Asset/SkillData", order = 0)]
public class SkillData : ScriptableObject
{

    [SerializeField] bool skillUiStatus = false;            //�÷��̾� �� ��
    [SerializeField] SkillKey skillKey;                     //� Ű�Է��� �� �� ����� ������
    [SerializeField][Range(0, 180)] int angle = 0;          //������� ���� ����
    [SerializeField][Range(0f, 10f)] float attackRange = 0; //���� �� �Ÿ�
    [SerializeField][Range(0, 10)] int cooltime = 0;    //��Ÿ�� �ð�(��)
    [SerializeField] Sprite spriteImage = null;         //��������Ʈ �̹���
    [SerializeField] Skill skill = null;                //��ų Ŭ����

    public bool SkillUiStatus => skillUiStatus;
    public SkillKey SkillKey => skillKey;
    public int Angle => angle;
    public float AttackRange => attackRange;
    public int Cooltime => cooltime;
    public Sprite SpriteImage => spriteImage;
    public Skill Skill => skill;
}

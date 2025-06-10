using System.Collections;

public class Defend : Skill
{
    protected override void Damage()
    {
    }
    protected override IEnumerator SkillCoroutine()
    {
        StopSkill();
        yield break;
    }
}

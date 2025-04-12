using UnityEngine;

public class ActiveSkill:Skill
{
    protected virtual void OnEnable()
    {
        skillType = SkillType.Active;
    }
}

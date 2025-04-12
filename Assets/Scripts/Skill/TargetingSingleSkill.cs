using UnityEngine;

[CreateAssetMenu(fileName = "TargetingSingleSkill", menuName = "Scriptable Objects/Skill/ActiveSkill/TargetingSingleSkill")]
public class TargetingSingleSkill: ActiveSkill
{
    protected override void OnEnable()
    {
        base.OnEnable();
        skillTargetType = SkillTargetType.Single;
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "AOESkill", menuName = "Scriptable Objects/Skill/ActiveSkill/AOESkill")]
public class AOESkill: ActiveSkill
{
    public int[] skillAreaRanges;
    public SkillAreaType skillAreaType;
    public SkillAreaEffectedType skillAreaEffectedType;
    protected override void OnEnable()
    {
        base.OnEnable();
        skillTargetType = SkillTargetType.Multiple;
    }
}

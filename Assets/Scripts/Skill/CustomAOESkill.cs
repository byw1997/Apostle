using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomAOESkill", menuName = "Scriptable Objects/Skill/ActiveSkill/CustomAOESkill")]
public class CustomAOESkill:AOESkill
{
    public CustomRange[] customRanges;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (customRanges == null)
        {
            customRanges = new CustomRange[0];
        }
        skillAreaType = SkillAreaType.Custom;
    }

    public List<Vector2Int> GetCustomArea(Vector2Int startPos, int level)
    {
        List<Vector2Int> area = new List<Vector2Int>();
        foreach (Vector2Int pos in customRanges[level].tilePositions)
        {
            Vector2Int newPos = startPos + pos;
            area.Add(newPos);
        }
        return area;
    }
}

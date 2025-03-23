using UnityEngine;

public enum SkillType
{
    Active,
    Passive
}

public enum SkillTargetTile
{
    Self,
    Ally,
    Enemy,
    All
}

public enum SkillTarget
{
    Self,
    Ally,
    Enemy,
    All
}

public enum SkillRangeType
{
    Orthogonal,
    Diagonal,
    Weapon
}

public enum SkillEffectType
{
    Damage,
    Heal,
    Buff,
    Debuff
}

public enum SkillAreaType
{
    Single,
    Multiple
}

public enum DamageType
{
    Weapon,
    Piercing,
    Blunt,
    Slashing,
    Fire,
    Lightning,
    Ice,
    Shock,
    Holy,
    Dark
}

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Objects/Skill")]
public class Skill : ScriptableObject
{
    [Header("Skill Info")]
    public int skillID;
    public string skillName;
    public int skillMPCost;
    public int[] skillAPCost;
    public SkillType skillType;
    public SkillTarget skillTarget;
    public SkillRangeType skillRangeType;
    public int[] skillRange;
    public SkillEffectType skillEffectType;
    public SkillAreaType skillAreaType;
    public DamageType damageType;
    public float[] skillEffectBaseValue;
    public float[] skillEffectScaleValue;
    public float[] skillEffectDuration;
    public float[] skillEffectChance;
    public int effectID;
    public string skillDescription;
    public string FlavourText;
    public Sprite skillIcon;
}

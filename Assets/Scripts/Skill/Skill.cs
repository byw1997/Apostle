using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

public enum StatType
{
    Strength,
    Dexterity,
    Constitution,
    Knowledge,
    Wisdom,
    Luck,
    Weapon,
    None
}

public enum SkillType
{
    Active,
    Passive
}

public enum SkillTarget//Skill Targeting.
{
    Self,
    Ally,
    Enemy,
    All
}

public enum SkillRangeType//Skill Range Calculated by
{
    Orthogonal,
    Diagonal,
    Circle,
    Weapon
}

public enum SkillTargetType//Skill Target numbers
{
    Single,
    Multiple
}

public enum SkillEffectType
{
    Damage,
    Heal,
    Buff,
    Debuff
}

public enum SkillAreaEffectedType
{
    Ally,
    Enemy,
    All
}


public enum SkillAreaType
{
    Orthogonal,
    Diagonal,
    Circle,
    Custom,
    None
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
    Dark,
    None
}

[System.Serializable]
public class CustomRange
{
    public List<Vector2Int> tilePositions = new List<Vector2Int>();
}

public class Skill : ScriptableObject//ReFacture - TargetingSingleSkill, TargetingAreaSkill, AOESkill
{
    [Header("Skill Info")]
    public int skillID;
    public string skillName;
    public int[] skillHPCost;
    public int[] skillMPCost;
    public int[] skillAPCost;
    public SkillType skillType;//active or passive
    public SkillTarget skillTarget;
    public SkillRangeType skillRangeType;
    public bool usedBySubWeapon;
    public int[] skillRanges;
    public int[] accuracy;
    public SkillEffectType skillEffectType;//Damage, Heal, Buff, Debuff
    public SkillTargetType skillTargetType;//Single Target or Multiple Target
    public DamageType damageType;
    public float[] skillEffectBaseValue;
    public StatType skillEffectModifierStat;
    public float[] skillEffectScaleValue;
    public float[] skillEffectDuration;
    public float[] skillEffectChance;
    public int effectID;
    public string skillDescription;
    public string FlavourText;
    public Sprite skillIcon;

    
}


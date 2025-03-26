using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
    Circle,
    Weapon
}

public enum SkillTargetType
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



public enum SkillAreaType
{
    Orthogonal,
    Diagonal,
    Circle,
    Custom
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

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Objects/Skill")]
public class Skill : ScriptableObject
{
    [Header("Skill Info")]
    public int skillID;
    public string skillName;
    public int[] skillMPCost;
    public int[] skillAPCost;
    public SkillType skillType;
    public SkillTarget skillTarget;
    public SkillRangeType skillRangeType;
    public int[] skillRanges;
    public CustomRange[] customRanges;
    public SkillEffectType skillEffectType;
    public SkillTargetType skillTargetType;
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

    private void OnEnable()
    {
        if (customRanges == null)
        {
            customRanges = new CustomRange[0];
        }
    }
}


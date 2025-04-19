using UnityEngine;

public enum EffectType
{
    Buff,
    Debuff,
    CrowdControl,
    DamageOverTime,
    HealOverTime
}

public enum BuffType
{
    Stat,
    Actionpoint,
    Damage,
    DamageReduction,
    SkillCost
}

public enum DebuffType
{
    Stat,
    Actionpoint,
    Damage,
    DamageReduction,
    SkillCost
}

public enum CrowdControlType
{
    Stun,
    Silence,
    Root
}

public enum ActionpointEffectType
{
    Move,
    Weapon,
    Magic
}

public enum SkillCostEffectType
{
    HP,
    MP
}

public enum DOTType
{
    Bleeding,
    Poison,
    Burn
}



[CreateAssetMenu(fileName = "Effect", menuName = "Scriptable Objects/Effect")]
public class Effect : ScriptableObject
{
    public int effectID;
    public string effectName;
    public string effectDescription;
    public EffectType effectType;
    public BuffType buffType;
    public DebuffType debuffType;
    public StatType statType;
    public CrowdControlType crowdControlType;
    public ActionpointEffectType actionpointEffectType;
    public SkillCostEffectType skillCostEffectType;
    public DOTType dotType;
    public float value;
    public int duration;
}

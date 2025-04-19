using UnityEngine;
using System.Collections.Generic;

public class OnEffectStruct
{
    [Header("Stat Effects")]
    public OnEffect strEffect;
    public OnEffect dexEffect;
    public OnEffect conEffect;
    public OnEffect knoEffect;
    public OnEffect wisEffect;
    public OnEffect lukEffect;

    [Header("Actionpoint Effects")]
    public OnEffect moveEffect;
    public OnEffect weaponEffect;
    public OnEffect magicEffect;

    [Header("Damage Effects")]
    public OnEffect damageEffect;
    public OnEffect damageReductionEffect;

    [Header("Skill Cost Effects")]
    public OnEffect skillCostEffect;

    [Header("Crowd Control Effects")]
    public List<OnEffect> crowdControlEffects = new List<OnEffect>();

    [Header("Damage Over Time Effects")]
    public List<OnEffect> damageOverTimeEffects = new List<OnEffect>();

    [Header("Heal Over Time Effects")]
    public List<OnEffect> healOverTimeEffects = new List<OnEffect>();
}

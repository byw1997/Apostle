using UnityEngine;

public enum WeaponType
{
    Dagger,
    Sword,
    GreatSword,
    Axe,
    GreatAxe,
    Hammer,
    GreatHammer,
    Flail,
    MorningStar,
    Rapier,
    Spear,
    Lance,
    Glaive,
    Bow,
    Crossbow,
    Staff,
    Wand,
    SmallShield,
    MediumShield,
    LargeShield,
    None
}

public enum WeaponHandedness
{
    OneHanded,
    TwoHanded
}

public enum StatBonusType
{
    Strength,
    Dexterity,
    Knowledge,
    Wisdom,
    Luck
}

public enum RangeType
{
    Orthogonal,
    Diagonal
}


public abstract class Weapon : Equipment
{
    WeaponType weaponType;
    RangeType rangeType;
    int minimumDamage;
    int maximumDamage;
    StatBonusType statBonusType;
    bool technical;
    DamageType damageType;
}

using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
public enum CharacterType
{
    Player,
    Companion,
    Enemy
}

public enum MoveType
{
    Orthogonal,
    Diagonal
}

public enum Resistance
{
    Weak,
    Normal,
    Strong,
    Immune
}

public enum EngagementType
{
    Orthogonal,
    Diagonal
}

public enum ClassType
{
    Fighter,
    Wizard,
    Rogue,
    Priest,
    None
}



public enum CharacterStatus
{
    Idle,
    Moving,
    Attacking,
    Casting,
    Dead
}
public class Character : MonoBehaviour, IDamageable
{
    [Header("Character Info")]
    public string characterName;
    [Header("Stat")]
    public int hp;
    public int currentHp;
    public int mp;
    public int currentMp;
    public int str;
    public int dex;
    public int con;
    public int kno;
    public int wis;
    public int luk;
    public int actionPoint;
    public Resistance piercingResistance;
    public Resistance bluntResistance;
    public Resistance slashingResistance;
    public Resistance fireResistance;
    public Resistance lightningResistance;
    public Resistance iceResistance;
    public Resistance shockResistance;
    public Resistance holyResistance;
    public Resistance darkResistance;

    public Tile tileUnderCharacter;
    public Vector2Int gridPos;

    public CharacterType type;

    [Header("Range")]
    public int currentActionPoint;
    public int EngageRange;
    public MoveType moveType;
    public EngagementType engagementType;

    [Header("Class")]
    public ClassType mainClass;
    public ClassType subClass;

    [Header("Skill")]
    public Skill[] skillSet = new Skill[6];
    public int[] skillLevel = new int[6];
    public List<int> acquiredSkills = new List<int>();

    [Header("Equipment")]
    public Helmet helmet;
    public Armor armor;
    public Glove glove;
    public Boots boots;
    public Weapon mainHandWeapon;
    public Weapon subHandWeapon;

    public CharacterStatus status;

    private Slider hpSlider;
    private Slider mpSlider;
    private Slider apSlider;

    public void InitializeBattle()
    {
        currentHp = hp;
        currentMp = mp;
        currentActionPoint = actionPoint;
        status = CharacterStatus.Idle;
    }

    

    public void InitializeTurn()
    {
        currentActionPoint = actionPoint;
    }

    public Vector3 GridPositionToActualPosition(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * 2.5f, 0, gridPos.y * 2.5f);
    }

    public void Move(Tile tile, Pathfinder.Node node)
    {
        StartCoroutine(MoveAlongPath(tile, node));
    }

    IEnumerator MoveAlongPath(Tile tile, Pathfinder.Node node)
    {
        status = CharacterStatus.Moving;
        List<Vector2Int> path = node.path;
        int totalCost = node.cost;
        Vector2Int currentPosition = tile.gridPos;
        Vector2Int nextPosition;
        currentActionPoint -= totalCost;
        UpdateApSlider();
        for (int i = 1; i < path.Count; i++)
        {
            nextPosition = path[i];
            yield return StartCoroutine(MoveToPosition(nextPosition));
        }

        tileUnderCharacter.RemoveObjectOnTile();

        tile.Deploy(gameObject);
        
        status = CharacterStatus.Idle;
    }

    private IEnumerator MoveToPosition(Vector2Int targetPosition)//Move animation for each tile
    {
        float elapsedTime = 0f;
        float duration = 0.25f;
        Vector3 startingPosition = transform.position;
        Vector3 actualTargetPosition = GridPositionToActualPosition(targetPosition);
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startingPosition, actualTargetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = actualTargetPosition;
    }

    public void ConnectUI(Slider hpSlider, Slider mpSlider, Slider apSlider)
    {
        this.hpSlider = hpSlider;
        this.mpSlider = mpSlider;
        this.apSlider = apSlider;
        hpSlider.maxValue = hp;
        mpSlider.maxValue = mp;
        apSlider.maxValue = actionPoint;
        UpdateHpSlider();
        UpdateMpSlider();
        UpdateApSlider();
    }

    public void UpdateHpSlider()
    {
        if (hpSlider)
        {
            hpSlider.value = currentHp;
        }
    }

    public void UpdateMpSlider()
    {
        if (mpSlider)
        {
            mpSlider.value = currentMp;
        }
    }

    public void UpdateApSlider()
    {
        if (apSlider)
        {
            apSlider.value = currentActionPoint;
        }
    }

    public void DisconnectUI()
    {
        if (hpSlider)
        {
            hpSlider = null;
        }
        if(mpSlider)
        {
            mpSlider = null;
        }
        if(apSlider)
        {
            apSlider = null;
        }
    }

    public void UseSkill(Tile tile, int index)
    {
        Debug.Log("Using skill: " + skillSet[index].skillName);
        Skill skill = skillSet[index];
        switch (skill.skillTargetType)
        {
            case SkillTargetType.Single:
                if (tile.objectOnTile != null)
                {
                    Character character = tile.objectOnTile.GetComponent<Character>();
                    if (character != null)
                    {
                        if (IsSkillCostSufficient(index))
                        {
                            Debug.Log("Skill cost is sufficient");
                            TakeDamage(DamageType.None, skillSet[index].skillHPCost[skillLevel[index]]);
                            TakeMPDamage(skillSet[index].skillMPCost[skillLevel[index]]);
                            TakeAPDamage(skillSet[index].skillAPCost[skillLevel[index]]);
                            StartCoroutine(UseSkillOnTarget(character, index));
                        }
                    }
                }
                break;
            case SkillTargetType.Multiple:
                // Implement area skill logic here
                break;
        }

    }

    bool IsSkillCostSufficient(int index)
    {
        Skill skill = skillSet[index];
        if (skill == null)
        {
            Debug.LogError("Skill not found");
            return false;
        }
        int level = skillLevel[index];
        if (currentActionPoint >= skill.skillAPCost[level] && currentHp >= skill.skillHPCost[level] && currentMp >= skill.skillMPCost[level])
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator UseSkillOnTarget(Character character, int index)
    {
        status = CharacterStatus.Casting;
        // Implement the logic to use the skill on the target
        if(IsSkillAvailable(character, index) == false)
        {
            Debug.Log("Skill not available");
            yield break;
        }
        yield return StartCoroutine(ActivateSkill(character, index));
        status = CharacterStatus.Idle;
    }

    IEnumerator ActivateSkill(Character character, int index)
    {
        Skill skill = skillSet[index];
        switch (skill.skillEffectType)
        {
            case SkillEffectType.Damage:
                // Implement damage logic here
                character.TakeDamage(skill.damageType, CalculateSkillEffectAmount(mainHandWeapon, skill, skillLevel[index]));
                break;
            case SkillEffectType.Heal:
                // Implement heal logic here
                character.TakeHeal(CalculateSkillEffectAmount(mainHandWeapon, skill, skillLevel[index]));
                break;
            case SkillEffectType.Buff:
                break;
            case SkillEffectType.Debuff:
                break;
        }
        yield return null;
    }

    public int CalculateWeaponDamage(Weapon weapon)
    {
        if((mainClass == ClassType.Wizard || subClass == ClassType.Wizard) && (weapon.weaponType == WeaponType.Staff || weapon.weaponType == WeaponType.Wand))
        {
            return Random.Range(weapon.minimumDamage, weapon.maximumDamage) + kno;
        }
        else
        {
            if (weapon.technical)
            {
                return Random.Range(weapon.minimumDamage, weapon.maximumDamage) + Mathf.Max(str, dex);
            }
            else
            {
                return Random.Range(weapon.minimumDamage, weapon.maximumDamage) + str;
            }
        }
    }

    public int CalculateSkillEffectAmount(Weapon weapon, Skill skill, int level)
    {
        switch (skill.damageType)
        {
            case DamageType.Weapon:
                return Mathf.RoundToInt(skill.skillEffectBaseValue[level] + CalculateWeaponDamage(weapon) * skill.skillEffectScaleValue[level]);
            default:
                switch (skill.skillEffectModifierStat)
                {
                    case StatType.Strength:
                        return Mathf.RoundToInt(skill.skillEffectBaseValue[level] + str * skill.skillEffectScaleValue[level]);
                    case StatType.Dexterity:
                        return Mathf.RoundToInt(skill.skillEffectBaseValue[level] + dex * skill.skillEffectScaleValue[level]);
                    case StatType.Constitution:
                        return Mathf.RoundToInt(skill.skillEffectBaseValue[level] + con * skill.skillEffectScaleValue[level]);
                    case StatType.Knowledge:
                        return Mathf.RoundToInt(skill.skillEffectBaseValue[level] + kno * skill.skillEffectScaleValue[level]);
                    case StatType.Wisdom:
                        return Mathf.RoundToInt(skill.skillEffectBaseValue[level] + wis * skill.skillEffectScaleValue[level]);
                    case StatType.Luck:
                        return Mathf.RoundToInt(skill.skillEffectBaseValue[level] + luk * skill.skillEffectScaleValue[level]);
                }
                break;
        }
        return 0;
    }

    public bool IsSkillAvailable(Character character, int index)
    {
        if(index < 0 || index >= skillSet.Length)
        {
            Debug.LogError("Invalid skill index");
            return false;
        }

        Skill skill = skillSet[index];
        if (skill == null)
        {
            Debug.LogError("Skill not found");
            return false;
        }

        switch (skill.skillTarget)
        {
            case SkillTarget.Self:
                return this == character;
            case SkillTarget.Enemy:
                return IsAlly(character) == false;
            case SkillTarget.Ally:
                return IsAlly(character);
            case SkillTarget.All:
                return true;
            default:
                return false;
        }
    }

    public bool IsAlly(Character character)
    {
        switch (type)
        {
            case CharacterType.Player:
                if (character.type == CharacterType.Player || character.type == CharacterType.Companion)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case CharacterType.Companion:
                if (character.type == CharacterType.Player || character.type == CharacterType.Companion)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case CharacterType.Enemy:
                if (character.type == CharacterType.Enemy)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            default:
                return false;
        }
    }

    public void TakeHeal(int heal)
    {
        currentHp += heal;
        if (currentHp > hp)
        {
            currentHp = hp;
        }
        UpdateHpSlider();
    }

    public void TakeDamage(DamageType damageType, int damage)
    {
        switch (damageType)
        {
            case DamageType.Piercing:
                damage = Mathf.RoundToInt(damage * (GetDamagePercentAfterResistance(piercingResistance)));
                break;
            case DamageType.Blunt:
                damage = Mathf.RoundToInt(damage * (GetDamagePercentAfterResistance(bluntResistance)));
                break;
            case DamageType.Slashing:
                damage = Mathf.RoundToInt(damage * (GetDamagePercentAfterResistance(slashingResistance)));
                break;
            case DamageType.Fire:
                damage = Mathf.RoundToInt(damage * (GetDamagePercentAfterResistance(fireResistance)));
                break;
            case DamageType.Lightning:
                damage = Mathf.RoundToInt(damage * (GetDamagePercentAfterResistance(lightningResistance)));
                break;
            case DamageType.Ice:
                damage = Mathf.RoundToInt(damage * (GetDamagePercentAfterResistance(iceResistance)));
                break;
            case DamageType.Shock:
                damage = Mathf.RoundToInt(damage * (GetDamagePercentAfterResistance(shockResistance)));
                break;
            case DamageType.Holy:
                damage = Mathf.RoundToInt(damage * (GetDamagePercentAfterResistance(holyResistance)));
                break;
            case DamageType.Dark:
                damage = Mathf.RoundToInt(damage * (GetDamagePercentAfterResistance(darkResistance)));
                break;
            case DamageType.None:
                break;
        }
        currentHp -= damage;
        if (currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }
        UpdateHpSlider();
    }

    public float GetDamagePercentAfterResistance(Resistance resistance)
    {
        switch (resistance)
        {
            case Resistance.Weak:
                return 1.5f;
            case Resistance.Normal:
                return 1f;
            case Resistance.Strong:
                return 0.5f;
            case Resistance.Immune:
                return 0f;
            default:
                return 1f;
        }
    }

    public void TakeMPDamage(int damage)
    {
        currentMp -= damage;
        if (currentMp <= 0)
        {
            currentMp = 0;
        }
        UpdateMpSlider();
    }

    public void TakeAPDamage(int damage)
    {
        currentActionPoint -= damage;
        if (currentActionPoint <= 0)
        {
            currentActionPoint = 0;
        }
        UpdateApSlider();
    }
    public void Die()
    {
        status = CharacterStatus.Dead;
        tileUnderCharacter.RemoveCharacterFromTile();
        tileUnderCharacter = null;
        gameObject.SetActive(false);
    }
}

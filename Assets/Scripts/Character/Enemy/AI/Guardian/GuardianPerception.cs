using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GuardianPerception : IEnemyPerception
{
    public IEnemyContext UpdatePerception(Enemy self)
    {
        var context = new GuardianContext();
        context.self = self;
        context.selfPos = self.gridPos;
        var playerCharacters = BattleManager.Instance.charactersOnBattle.Where(c => c.type != CharacterType.Enemy && c.status != CharacterStatus.Dead).ToList();
        var enemies = BattleManager.Instance.charactersOnBattle.Where(c => c.type == CharacterType.Enemy && c.status != CharacterStatus.Dead).ToList();
        context.playerCharacters = playerCharacters;
        context.enemies = enemies;
        context.playerFormationCenter = GetFormationCenter(playerCharacters);
        context.enemyFormationCenter = GetFormationCenter(enemies);
        return context;
    }

    public bool IsSelfFrontlineGuardian(Character self)
    {

        return false;
    }

    private Vector2 GetFormationCenter(List<Character> characters)
    {
        float avgX = 0f;
        float avgY = 0f;
        foreach (var character in characters)
        {
            avgX += character.gridPos.x;
            avgY += character.gridPos.y;
        }
        avgX /= characters.Count;
        avgY /= characters.Count;

        return new Vector2(avgX, avgY);
    }

    private Character GetMostVulnerable(List<Character> characters)
    {
        Character mostVulnerableEnemy = null;

        return mostVulnerableEnemy;
    }
}

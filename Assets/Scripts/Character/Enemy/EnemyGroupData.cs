using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyGroupData", menuName = "Scriptable Objects/EnemyGroupData")]
public class EnemyGroupData : ScriptableObject
{
    public List<int> enemyIdList = new List<int>();
    public List<Vector2Int> enemyPosList = new List<Vector2Int>();
}

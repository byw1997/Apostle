using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour
{
    public GameObject playerCharacterPrefab;
    public List<GameObject> activeCompanionCharactersPrefabs = new List<GameObject>();

    public GameObject playerCharacter;
    public List<GameObject> activeCompanionCharacters = new List<GameObject>();

    private void Awake()
    {
        playerCharacter = Instantiate(playerCharacterPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        playerCharacter.SetActive(false);
        foreach (GameObject companionCharacterPrefab in activeCompanionCharactersPrefabs)
        {
            GameObject companionCharacter = Instantiate(companionCharacterPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            companionCharacter.SetActive(false);
            activeCompanionCharacters.Add(companionCharacter);
        }
    }
}

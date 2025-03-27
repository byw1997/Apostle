using UnityEngine;

public class TestFloorGenerator : MonoBehaviour
{
    public GameObject floorPrefab;

    public GameObject[] enemyList;

    public int mapSize;
    void Awake()
    {
        for(int i = 0; i < 2 * mapSize + 1; i++)
        {
            for (int j = 0; j < 2 * mapSize + 1; j++)
            {
                GameObject floor = Instantiate(floorPrefab, new Vector3(2.5f * i - mapSize * 2.5f, 0, 2.5f * j - mapSize * 2.5f), Quaternion.identity);
                floor.transform.SetParent(transform);
            }
        }
    }

}

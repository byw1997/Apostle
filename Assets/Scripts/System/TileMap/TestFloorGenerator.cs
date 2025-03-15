using UnityEngine;

public class TestFloorGenerator : MonoBehaviour
{
    public GameObject floorPrefab;
    void Awake()
    {
        for(int i = 0; i < 11; i++)
        {
            for (int j = 0; j < 11; j++)
            {
                GameObject floor = Instantiate(floorPrefab, new Vector3(2.5f * i - 12.5f, 0, 2.5f * j - 12.5f), Quaternion.identity);
                floor.transform.SetParent(transform);
            }
        }
    }

}

using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject battleUI;

    public void ShowBattleUI()
    {
        battleUI.SetActive(true);
    }
}

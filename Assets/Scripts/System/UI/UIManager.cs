using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject battleUI;

    [Header("UI")]
    public Slider hpSlider;
    public Slider mpSlider;
    public Slider apSlider;

    public void UpdateUI(Character character)
    {
        ConnectUIWithCharacter(character);
    }

    public void ConnectUIWithCharacter(Character character)
    {
        character.ConnectUI(hpSlider, mpSlider, apSlider);
    }

    public void ShowBattleUI()
    {
        battleUI.SetActive(true);
    }

    public void UnShowBattleUI()
    {
        battleUI.SetActive(false);
    }
}

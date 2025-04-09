using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject battleUI;
    [SerializeField] GameObject OptionUI;

    [Header("BattleUI")]
    public Slider hpSlider;
    public Slider mpSlider;
    public Slider apSlider;

    [Header("BattleLog")]
    public ScrollRect log;
    public GameObject logContent;
    public GameObject logItemPrefab;
    public int maxLogItems = 100;
    private Queue<GameObject> logItems = new Queue<GameObject>();

    public void AddLog(string message)
    {
        if (logItems.Count >= maxLogItems)
        {
            Destroy(logItems.Dequeue());
        }
        GameObject newLogItem = Instantiate(logItemPrefab, logContent.transform);
        newLogItem.GetComponent<TMP_Text>().text = message;
        logItems.Enqueue(newLogItem);
        Canvas.ForceUpdateCanvases();
        log.verticalNormalizedPosition = 0f;
    }

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

    public void ShowOptionUI()
    {
        OptionUI.SetActive(true);
    }

    public void UnShowOptionUI()
    {
        OptionUI.SetActive(false);
    }

    public void UnShowInventoryUI()
    {

    }

    public void UnShowSkillsUI()
    {

    }
}

using UnityEngine;
using TMPro;
using System.Linq;

/// <summary>
/// Wallet management
/// </summary>
public class Wallet : MonoBehaviour
{
    public static Wallet instance;

    public int gold;

    // UI
    public GameObject win;
    public TMP_Text info;
    public GameObject block;
    public TMP_Text walletUI;

    [SerializeField] private GameObject[] collectableList;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        gold = 0;
        UpdateUI();
    }

    /// <summary>
    /// Add gold
    /// </summary>
    /// <param name="value">add value</param>
    public void AddGold(int value)
    {
        gold += value;
        UpdateUI();
    }

    /// <summary>
    /// Set gold number
    /// </summary>
    /// <param name="value">Set value</param>
    public void SetGold(int value)
    {
        gold = value;
        UpdateUI();
    }

    /// <summary>
    /// Use gold
    /// </summary>
    /// <param name="value">Use value</param>
    /// <returns>true if have enough gold</returns>
    public bool SpendGold(int value)
    {
        if (gold < value) return false;
        gold -= value;
        return true;
    }

    /// <summary>
    /// Update wallet UI
    /// </summary>
    private void UpdateUI()
    {
        walletUI.text = gold.ToString();
    }

    /// <summary>
    /// Reset gold number, UI and collectable items
    /// </summary>
    public void ResetGold()
    {
        gold = 0;
        UpdateUI();
        foreach (GameObject collectable in collectableList)
        {
            collectable.SetActive(true);
        }
    }

    /// <summary>
    /// If player collected all golds and return to start zone, end game
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && collectableList.Where(o => o.activeInHierarchy).ToArray().Length == 0)
        {
            info.text = $"You collect {gold}/33 gold!";
            win.SetActive(true);
            block.SetActive(true);
        }
    }
}

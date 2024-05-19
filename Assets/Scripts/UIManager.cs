using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private PlayerBuilding playerBuilding;

    [SerializeField] private GameObject buildMenuPanel;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _killsText;
    [SerializeField] private TextMeshProUGUI _ammoText;
    [SerializeField] private TextMeshProUGUI _timeAbilityText;

    void Start()
    {
        playerBuilding = FindObjectOfType<PlayerBuilding>();
    }

    public void SetBuildingIndex(int index)
    {
        playerBuilding.SetBuildingIndex(index);
        CloseBuildMenu();
    }

    public void OpenBuildMenu()
    {
        buildMenuPanel.SetActive(true);
    }

    public void CloseBuildMenu()
    {
        buildMenuPanel.SetActive(false);
    }

    public void UpdateHealthText(int health)
    {
        _healthText.text = health.ToString();
    }

    public void UpdateKillsText(int kills)
    {
        _killsText.text = kills.ToString();
    }

    public void UpdateAmmoText(int ammo)
    {
        _ammoText.text = ammo.ToString();
    }

    public void UpdateTimeAbilityText(float time)
    {
        _timeAbilityText.text = time.ToString();
    }
}

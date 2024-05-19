using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private PlayerBuilding playerBuilding;
    [SerializeField]private GameObject buildMenuPanel;

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
}

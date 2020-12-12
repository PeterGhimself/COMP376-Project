using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulesMenu : MonoBehaviour
{
    public GameObject basicControlPanel;
    public GameObject playerPanel;
    public GameObject enemiesPanel;
    public GameObject bossesPanel;
    public GameObject weaponsPanel;
    public GameObject abilitiesPanel;
    public GameObject powerUpsPanel;
    public GameObject obstaclesPanel;

    public void showBasicControlsPanel()
    {
        basicControlPanel.SetActive(true);
        playerPanel.SetActive(false);
        enemiesPanel.SetActive(false);
        bossesPanel.SetActive(false);
        weaponsPanel.SetActive(false);
        abilitiesPanel.SetActive(false);
        powerUpsPanel.SetActive(false);
        obstaclesPanel.SetActive(false);
    }

    public void showPlayerPanel()
    {
        basicControlPanel.SetActive(false);
        playerPanel.SetActive(true);
        enemiesPanel.SetActive(false);
        bossesPanel.SetActive(false);
        weaponsPanel.SetActive(false);
        abilitiesPanel.SetActive(false);
        powerUpsPanel.SetActive(false);
        obstaclesPanel.SetActive(false);
    }

    public void showEnemiesPanel()
    {
        basicControlPanel.SetActive(false);
        playerPanel.SetActive(false);
        enemiesPanel.SetActive(true);
        bossesPanel.SetActive(false);
        weaponsPanel.SetActive(false);
        abilitiesPanel.SetActive(false);
        powerUpsPanel.SetActive(false);
        obstaclesPanel.SetActive(false);
    }

    public void showBossesPanel()
    {
        basicControlPanel.SetActive(false);
        playerPanel.SetActive(false);
        enemiesPanel.SetActive(false);
        bossesPanel.SetActive(true);
        weaponsPanel.SetActive(false);
        abilitiesPanel.SetActive(false);
        powerUpsPanel.SetActive(false);
        obstaclesPanel.SetActive(false);
    }

    public void showWeaponsPanel()
    {
        basicControlPanel.SetActive(false);
        playerPanel.SetActive(false);
        enemiesPanel.SetActive(false);
        bossesPanel.SetActive(false);
        weaponsPanel.SetActive(true);
        abilitiesPanel.SetActive(false);
        powerUpsPanel.SetActive(false);
        obstaclesPanel.SetActive(false);
    }

    public void showAbilitiesPanel()
    {
        basicControlPanel.SetActive(false);
        playerPanel.SetActive(false);
        enemiesPanel.SetActive(false);
        bossesPanel.SetActive(false);
        weaponsPanel.SetActive(false);
        abilitiesPanel.SetActive(true);
        powerUpsPanel.SetActive(false);
        obstaclesPanel.SetActive(false);
    }

    public void showPowerUpsPanel()
    {
        basicControlPanel.SetActive(false);
        playerPanel.SetActive(false);
        enemiesPanel.SetActive(false);
        bossesPanel.SetActive(false);
        weaponsPanel.SetActive(false);
        abilitiesPanel.SetActive(false);
        powerUpsPanel.SetActive(true);
        obstaclesPanel.SetActive(false);
    }

    public void showObstaclesPanel()
    {
        basicControlPanel.SetActive(false);
        playerPanel.SetActive(false);
        enemiesPanel.SetActive(false);
        bossesPanel.SetActive(false);
        weaponsPanel.SetActive(false);
        abilitiesPanel.SetActive(false);
        powerUpsPanel.SetActive(false);
        obstaclesPanel.SetActive(true);
    }
}

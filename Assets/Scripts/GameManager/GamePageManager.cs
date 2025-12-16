using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePageManager : MonoBehaviour
{

    [SerializeField] private SpawnEnemies spawnEnemies;

    public Toggle easyLevelToggle;
    public Toggle hardLevelToggle;


    private bool isUpdating = false;


    void Awake()
    {
        easyLevelToggle.isOn = true;
        hardLevelToggle.isOn = false;
    }

    public void SelectEasyLevel()
    {
        if (isUpdating) return;

        isUpdating = true;
        easyLevelToggle.isOn = true;
        hardLevelToggle.isOn = false;
        isUpdating = false;

        spawnEnemies.SetEasyLevel();
    }


    public void SelectHardLevel()
    {
        if (isUpdating) return;

        isUpdating = true;
        hardLevelToggle.isOn = true;
        easyLevelToggle.isOn = false;
        isUpdating = false;

        spawnEnemies.SetHardLevel();
    }

    public void LockDifficulty()
    {
        easyLevelToggle.interactable = false;
        hardLevelToggle.interactable = false;
    }


}

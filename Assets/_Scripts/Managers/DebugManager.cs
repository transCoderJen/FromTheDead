using System;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : Singleton<DebugManager>
{
    private bool isDebugMode = false;
    [SerializeField] private GameObject checkpointsParent;
    public List<Checkpoint> checkpoints = new List<Checkpoint>();
    [SerializeField] private UI_Options uiOptions;
    [SerializeField] private GameObject enemiesParent;

    private void Start()
    {
        InitializeCheckpoints();
    }

    private void InitializeCheckpoints()
    {
        if (checkpointsParent == null)
        {
            Debug.LogError("Checkpoints parent is not assigned in the DebugManager.");
            return;
        }

        checkpoints.Clear();
        foreach (Transform child in checkpointsParent.transform)
        {
            Checkpoint checkpoint = child.GetComponent<Checkpoint>();
            if (checkpoint != null)
            {
                checkpoints.Add(checkpoint);
            }
        }

        Debug.Log("Initialized " + checkpoints.Count + " checkpoints.");
    }

    public void ToggleDebugMode()
    {
        isDebugMode = !isDebugMode;

        Debug.Log("Debug mode is now " + (isDebugMode ? "ON" : "OFF"));
        uiOptions.ShowDebugOptions(isDebugMode);
    }

    public bool isDebugModeEnabled()
    {
        return isDebugMode;
    }

    public void ToggleEnemies()
    {
        enemiesParent.SetActive(!enemiesParent.activeSelf);
    }
}

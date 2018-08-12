using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverdaysArea : MonoBehaviour
{
    [SerializeField]
    private UpgradeButton elevatorUpgradeButton;

    [SerializeField]
    private UpgradeButton overdaysUpgradeButton;

    [SerializeField]
    private Manager overdaysManager;
    [SerializeField]
    private Manager elevatorManager;

    private OverdaysWorker[] workers;

    private void Start()
    {
        elevatorUpgradeButton.upgradeCost = Mathf.RoundToInt(GameCore.Instance.Data.elevatorUpgradeCost);
        elevatorUpgradeButton.OnUpgraded += HandleElevatorUpgraded;
        elevatorManager.OnManagerBought += HandleElevatorManagerBought;
        elevatorManager.OnManagerActivated += HandleElevatorManagerActivated;

        overdaysUpgradeButton.upgradeCost = Mathf.RoundToInt(GameCore.Instance.Data.overdaysUpgradeCost);
        overdaysUpgradeButton.OnUpgraded += HandleOverdaysUpgraded;
        overdaysManager.OnManagerBought += HandleOverdaysManagerBought;
        overdaysManager.OnManagerActivated += HandleOverdaysManagerActivated;

        workers = GetComponentsInChildren<OverdaysWorker>();

        elevatorManager.managerCost = Mathf.RoundToInt(GameCore.Instance.Data.elevatorManagerCost);
        overdaysManager.managerCost = Mathf.RoundToInt(GameCore.Instance.Data.overdaysManagerCost);
    }

    private void HandleElevatorManagerBought()
    {
        ElevatorWorker elevatorGuy = GetComponentInChildren<ElevatorWorker>();
        elevatorGuy.active = true;
        elevatorGuy.hasManager = true;
    }

    private void HandleElevatorManagerActivated()
    {
        throw new NotImplementedException();
    }

    private void HandleOverdaysManagerBought()
    {
        workers = GetAllWorkers();
        foreach (var worker in workers)
        {
            worker.active = true;
            worker.hasManager = true;
        }
    }

    private OverdaysWorker[] GetAllWorkers()
    {
        Array.Clear(workers, 0, workers.Length);
        return GetComponentsInChildren<OverdaysWorker>();
    }

    private void HandleOverdaysManagerActivated()
    {
        throw new NotImplementedException();
    }

    private void HandleElevatorUpgraded()
    {
        throw new NotImplementedException();
    }

    private void HandleOverdaysUpgraded()
    {
        throw new NotImplementedException();
    }
}

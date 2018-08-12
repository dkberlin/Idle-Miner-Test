using System;
using UnityEngine;

public class OverdaysArea : MonoBehaviour
{
    [SerializeField] private Manager elevatorManager;

    [SerializeField] private UpgradeButton elevatorUpgradeButton;

    [SerializeField] private Manager overdaysManager;

    [SerializeField] private UpgradeButton overdaysUpgradeButton;

    private OverdaysWorker[] workers;

    private void Start()
    {
        elevatorUpgradeButton.upgradeCost = Mathf.RoundToInt(GameCore.Instance.Data.ElevatorUpgradeCost);
        elevatorUpgradeButton.OnUpgraded += HandleElevatorUpgraded;
        elevatorManager.OnManagerBought += HandleElevatorManagerBought;
        elevatorManager.OnManagerActivated += HandleElevatorManagerActivated;

        overdaysUpgradeButton.upgradeCost = Mathf.RoundToInt(GameCore.Instance.Data.OverdaysUpgradeCost);
        overdaysUpgradeButton.OnUpgraded += HandleOverdaysUpgraded;
        overdaysManager.OnManagerBought += HandleOverdaysManagerBought;
        overdaysManager.OnManagerActivated += HandleOverdaysManagerActivated;

        workers = GetComponentsInChildren<OverdaysWorker>();

        elevatorManager.managerCost = Mathf.RoundToInt(GameCore.Instance.Data.ElevatorManagerCost);
        overdaysManager.managerCost = Mathf.RoundToInt(GameCore.Instance.Data.OverdaysManagerCost);
    }

    private void HandleElevatorManagerBought()
    {
        var elevatorGuy = GetComponentInChildren<ElevatorWorker>();
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
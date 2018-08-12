using System;
using UnityEngine;

public class OverdaysArea : MonoBehaviour
{
    [SerializeField] private Manager elevatorManager;

    [SerializeField] private UpgradeButton elevatorUpgradeButton;

    [SerializeField] private Manager overdaysManager;

    [SerializeField] private UpgradeButton overdaysUpgradeButton;

    [SerializeField] private ElevatorWorker elevatorGuy;

    [SerializeField]
    private UpgradeButton upgradeButton;
    [SerializeField]
    private ContainerBase overdaysContainer;

    [SerializeField]
    private ElevatorWorker elevatorContainer;

    public OverdaysWorker workerPrefab;

    private OverdaysWorker[] workers;
    private float multiplier;

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

        multiplier = GameCore.Instance.Data.BoughtUpgradeMultiplier;
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
        if (elevatorGuy.timesUpdated < elevatorGuy.maxSpeedUpgrades)
        {
            elevatorGuy.walkingSpeed = elevatorGuy.walkingSpeed * multiplier;
        }
        elevatorGuy.capacity = Mathf.RoundToInt(elevatorGuy.capacity * multiplier);
        elevatorGuy.timeToLoad = elevatorGuy.timeToLoad - multiplier / 2;
        elevatorGuy.timeToUnload = elevatorGuy.timeToUnload - multiplier / 2;
        elevatorGuy.timesUpdated++;
        elevatorGuy.SetElevatorWOrkerCapacityText();
        elevatorUpgradeButton.upgradeCost = GameCore.Instance.Data.GetNewUpgradeCost(elevatorUpgradeButton.upgradeCost);
    }

    private void HandleOverdaysUpgraded()
    {
        workers = GetAllWorkers();
        var allMinersMaxed = false;

        foreach (var miner in workers)
        {
            if (miner.timesUpdated == 3)
            {
                allMinersMaxed = true;
                continue;
            }

            if (miner.timesUpdated > 3)
            {
                continue;
            }

            miner.walkingSpeed = miner.walkingSpeed * multiplier;
            miner.capacity = Mathf.RoundToInt(miner.capacity * multiplier);
            miner.timeToLoad = miner.timeToLoad - multiplier / 2;
            miner.timeToUnload = miner.timeToUnload - multiplier / 2;
            miner.timesUpdated++;
            allMinersMaxed = false;
        }

        if (allMinersMaxed)
        {
            AddNewMiner();
        }

        overdaysContainer.maxCapacity = Mathf.RoundToInt(overdaysContainer.maxCapacity * 1.7f);
        overdaysContainer.SetContainerCapacityText();

        var newUpgradeCost = GameCore.Instance.Data.GetNewUpgradeCost(upgradeButton.upgradeCost);
        upgradeButton.upgradeCost = newUpgradeCost;
    }

    private void AddNewMiner()
    {
        var newMiner = Instantiate(workerPrefab, transform.position, transform.rotation, transform);
        if (overdaysManager.managerBought)
        {
            newMiner.active = true;
        }
    }
}
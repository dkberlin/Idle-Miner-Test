using System;
using System.Collections;
using UnityEngine;

public class OverdaysArea : MonoBehaviour
{
    [SerializeField] private ElevatorWorker elevatorContainer;
    [SerializeField] private ElevatorWorker elevatorGuy;
    [SerializeField] private Manager elevatorManager;
    [SerializeField] private UpgradeButton elevatorUpgradeButton;
    [SerializeField] private ContainerBase overdaysContainer;
    [SerializeField] private Manager overdaysManager;
    [SerializeField] private UpgradeButton overdaysUpgradeButton;
    [SerializeField] private UpgradeButton upgradeButton;

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

        elevatorManager.SetManagerCost(Mathf.RoundToInt(GameCore.Instance.Data.ElevatorManagerCost));
        overdaysManager.SetManagerCost(Mathf.RoundToInt(GameCore.Instance.Data.OverdaysManagerCost));

        multiplier = GameCore.Instance.Data.BoughtUpgradeMultiplier;
    }

    private void HandleElevatorManagerBought()
    {
        elevatorGuy.active = true;
        elevatorGuy.hasManager = true;
        elevatorGuy.shouldBeMoving = true;
    }

    private void HandleElevatorManagerActivated()
    {
        var boostTime = elevatorManager.managerBonusTime;

        if (!elevatorManager.activated &&
            !elevatorManager.isCoolingDown)
        {
            StartCoroutine(ManagerBoost(boostTime, elevatorManager));
        }
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
        var boostTime = overdaysManager.managerBonusTime;

        if (!elevatorManager.activated &&
            !elevatorManager.isCoolingDown)
        {
            StartCoroutine(ManagerBoost(boostTime, overdaysManager));
        }
    }

    private IEnumerator ManagerBoost(int boostTime, Manager manager)
    {
        manager.activated = true;
        var bonus = GameCore.Instance.Data.ActiveManagerMultiplier;
        manager.SwitchToInactiveSprite();

        var elevatorArea = elevatorManager.workingPlace;
        var overdaysArea = overdaysManager.workingPlace;

        if (manager.workingPlace == overdaysArea)
        {
            foreach (var miner in workers)
            {
                miner.capacity = Mathf.RoundToInt(miner.capacity * bonus);
                miner.walkingSpeed = miner.walkingSpeed * bonus;
                miner.timeToLoad = miner.timeToLoad / bonus;
                miner.timeToUnload = miner.timeToUnload / bonus;
            }
        }
        else
        {
            elevatorGuy.capacity = Mathf.RoundToInt(elevatorGuy.capacity * bonus);
            elevatorGuy.walkingSpeed = elevatorGuy.walkingSpeed * bonus;
            elevatorGuy.timeToLoad = elevatorGuy.timeToLoad / bonus;
            elevatorGuy.timeToUnload = elevatorGuy.timeToUnload / bonus;
        }

        yield return new WaitForSeconds(boostTime);

        manager.activated = false;

        if (manager.workingPlace == overdaysArea)
        {
            foreach (var miner in workers)
            {
                miner.capacity = Mathf.RoundToInt(miner.capacity / bonus);
                miner.walkingSpeed = miner.walkingSpeed / bonus;
                miner.timeToLoad = miner.timeToLoad * bonus;
                miner.timeToUnload = miner.timeToUnload * bonus;
            }
        }
        else
        {
            elevatorGuy.capacity = Mathf.RoundToInt(elevatorGuy.capacity / bonus);
            elevatorGuy.walkingSpeed = elevatorGuy.walkingSpeed / bonus;
            elevatorGuy.timeToLoad = elevatorGuy.timeToLoad * bonus;
            elevatorGuy.timeToUnload = elevatorGuy.timeToUnload * bonus;
        }

        StartCoroutine(StartCoolDownPhase(manager));
    }

    private IEnumerator StartCoolDownPhase(Manager manager)
    {
        manager.isCoolingDown = true;

        yield return new WaitForSecondsRealtime(GameCore.Instance.Data.ManagerCoolDownTime);

        manager.SwitchToActiveSprite();
        manager.isCoolingDown = false;
    }

    private void HandleElevatorUpgraded()
    {
        if (elevatorGuy.timesUpdated < elevatorGuy.maxSpeedUpgrades)
        {
            elevatorGuy.walkingSpeed = elevatorGuy.walkingSpeed * multiplier;
            elevatorGuy.timeToLoad = elevatorGuy.timeToLoad - multiplier / 2;
            elevatorGuy.timeToUnload = elevatorGuy.timeToUnload - multiplier / 2;
        }

        elevatorGuy.capacity = Mathf.RoundToInt(elevatorGuy.capacity * multiplier);
        elevatorGuy.UpgradeAdded();
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

        overdaysContainer.SetNewMaxCapacity(Mathf.RoundToInt(overdaysContainer.maxCapacity * 1.7f));
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
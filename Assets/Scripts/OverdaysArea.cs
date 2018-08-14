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
    [SerializeField] private OverdaysWorker workerPrefab;

    private OverdaysWorker[] workers;
    private float multiplier;

    private void Start()
    {
        elevatorUpgradeButton.SetNewUpgradeCost(Mathf.RoundToInt(GameCore.Instance.Data.ElevatorUpgradeCost));
        elevatorUpgradeButton.OnUpgraded += HandleElevatorUpgraded;
        elevatorManager.OnManagerBought += HandleElevatorManagerBought;
        elevatorManager.OnManagerActivated += HandleElevatorManagerActivated;

        overdaysUpgradeButton.SetNewUpgradeCost(Mathf.RoundToInt(GameCore.Instance.Data.OverdaysUpgradeCost));
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
                miner.SetCap(Mathf.RoundToInt(miner.Capacity * bonus));
                miner.SetWalkingSpeed(miner.WalkingSpeed * bonus);
                miner.SetTimeToLoad(miner.TimeToLoad / bonus);
                miner.SetTimeToUnload(miner.TimeToUnload / bonus);
            }
        }
        else
        {
            elevatorGuy.SetCap(Mathf.RoundToInt(elevatorGuy.Capacity * bonus));
            elevatorGuy.SetWalkingSpeed(elevatorGuy.WalkingSpeed * bonus);
            elevatorGuy.SetTimeToLoad(elevatorGuy.TimeToLoad / bonus);
            elevatorGuy.SetTimeToUnload(elevatorGuy.TimeToUnload / bonus);
        }

        yield return new WaitForSeconds(boostTime);

        manager.activated = false;

        if (manager.workingPlace == overdaysArea)
        {
            foreach (var miner in workers)
            {
                miner.SetCap(Mathf.RoundToInt(miner.Capacity / bonus));
                miner.SetWalkingSpeed(miner.WalkingSpeed / bonus);
                miner.SetTimeToLoad(miner.TimeToLoad * bonus);
                miner.SetTimeToUnload(miner.TimeToUnload * bonus);
            }
        }
        else
        {
            elevatorGuy.SetCap(Mathf.RoundToInt(elevatorGuy.Capacity / bonus));
            elevatorGuy.SetWalkingSpeed(elevatorGuy.WalkingSpeed / bonus);
            elevatorGuy.SetTimeToLoad(elevatorGuy.TimeToLoad * bonus);
            elevatorGuy.SetTimeToUnload(elevatorGuy.TimeToUnload * bonus);
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
            elevatorGuy.SetWalkingSpeed(elevatorGuy.WalkingSpeed * multiplier);
            elevatorGuy.SetTimeToLoad(elevatorGuy.TimeToLoad - multiplier / 2);
            elevatorGuy.SetTimeToUnload(elevatorGuy.TimeToUnload - multiplier / 2);
        }

        elevatorGuy.SetCap(Mathf.RoundToInt(elevatorGuy.Capacity * multiplier));
        elevatorGuy.UpgradeAdded();
        elevatorGuy.SetElevatorWOrkerCapacityText();
        elevatorUpgradeButton.SetNewUpgradeCost(GameCore.Instance.Data.GetNewUpgradeCost(elevatorUpgradeButton.upgradeCost));
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

            miner.SetWalkingSpeed(miner.WalkingSpeed * multiplier);
            miner.SetCap(Mathf.RoundToInt(miner.Capacity * multiplier));
            miner.SetTimeToLoad(miner.TimeToLoad - multiplier / 2);
            miner.SetTimeToUnload(miner.TimeToUnload - multiplier / 2);
            miner.AddUpdateAmount();
            allMinersMaxed = false;
        }

        if (allMinersMaxed)
        {
            AddNewMiner();
        }

        overdaysContainer.SetNewMaxCapacity(Mathf.RoundToInt(overdaysContainer.maxCapacity * 1.7f));
        overdaysContainer.SetContainerCapacityText();

        var newUpgradeCost = GameCore.Instance.Data.GetNewUpgradeCost(upgradeButton.upgradeCost);
        upgradeButton.SetNewUpgradeCost(newUpgradeCost);
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
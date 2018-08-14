using System;
using System.Collections;
using UnityEngine;

public class MineShaft : MonoBehaviour
{
    [SerializeField] private Miner minerPrefab;
    [SerializeField] private MineContainer container;
    [SerializeField] private MineContainer elevatorShaft;
    [SerializeField] private ContainerBase endOfMine;
    [SerializeField] private Manager shaftManager;
    [SerializeField] private UpgradeButton upgradeButton;
    [SerializeField] private int mineshaftFloor;

    public bool isFirstMineshaft = true;

    private Miner[] mineShaftWorkers;
    private float multiplier;

    private void Start()
    {
        if (isFirstMineshaft)
        {
            upgradeButton.SetNewUpgradeCost(Mathf.RoundToInt(GameCore.Instance.Data.BasicMineshaftUpgradeCost));
        }

        upgradeButton.OnUpgraded += HandleUpgrade;
        mineShaftWorkers = GetComponentsInChildren<Miner>();
        shaftManager.OnManagerBought += HandleManagerBought;
        shaftManager.OnManagerActivated += HandleManagerActivated;
        multiplier = GameCore.Instance.Data.BoughtUpgradeMultiplier;
        mineshaftFloor = GameCore.Instance.GetAmountOfMineshafts();
        shaftManager.SetManagerCost(GameCore.Instance.Data.GetManagerCost(mineshaftFloor));
        elevatorShaft.SetNewMaxCapacity(elevatorShaft.GetMaxCapacity() * mineshaftFloor);
        shaftManager.SetManagerBonusTime(((mineshaftFloor * 6) / 2) + 7);
        elevatorShaft.SetContainerCapacityText();
    }
    #region GETTER
    public MineContainer GetMineshaftContainer()
    {
        return container;
    }

    public UpgradeButton GetMineshaftUpgradeButton()
    {
        return upgradeButton;
    }

    public Manager GetShaftManager()
    {
        return shaftManager;
    }

    public MineContainer GetElevatorShaftContainer()
    {
        return elevatorShaft;
    }

    public ContainerBase GetEndOfMine()
    {
        return endOfMine;
    }
    #endregion

    private void HandleManagerActivated()
    {
        int boostTime = shaftManager.managerBonusTime;

        if (!shaftManager.activated &&
            !shaftManager.isCoolingDown)
        {
            StartCoroutine(ManagerBoost(boostTime));
        }
    }

    private IEnumerator ManagerBoost(int boostTime)
    {
        shaftManager.activated = true;
        var bonus = GameCore.Instance.Data.ActiveManagerMultiplier;
        shaftManager.SetManagerSpriteActive(false);

        foreach (var miner in mineShaftWorkers)
        {
            miner.SetCap(Mathf.RoundToInt(miner.GetCapacity() * bonus));
            miner.SetWalkingSpeed(miner.GetWalkingSpeed() * bonus);
            miner.SetTimeToLoad(miner.GetTimeToLoad() / bonus);
            miner.SetTimeToUnload(miner.GetTimeToUnload() / bonus);
        }

        yield return new WaitForSeconds(boostTime);

        shaftManager.activated = false;

        foreach (var miner in mineShaftWorkers)
        {
            miner.SetCap(Mathf.RoundToInt(miner.GetCapacity() / bonus));
            miner.SetWalkingSpeed(miner.GetWalkingSpeed() / bonus);
            miner.SetTimeToLoad(miner.GetTimeToLoad() * bonus);
            miner.SetTimeToUnload(miner.GetTimeToUnload() * bonus);
        }

        StartCoroutine(StartCoolDownPhase());
    }

    private IEnumerator StartCoolDownPhase()
    {
        shaftManager.isCoolingDown = true;

        yield return new WaitForSecondsRealtime(GameCore.Instance.Data.ManagerCoolDownTime);

        shaftManager.SetManagerSpriteActive(true);
        shaftManager.isCoolingDown = false;
    }

    private void HandleManagerBought()
    {
        mineShaftWorkers = GetAllMineShaftWorkers();
        foreach (var worker in mineShaftWorkers)
        {
            worker.active = true;
            worker.hasManager = true;
        }
    }

    private Miner[] GetAllMineShaftWorkers()
    {
        Array.Clear(mineShaftWorkers, 0, mineShaftWorkers.Length);
        return GetComponentsInChildren<Miner>();
    }

    private void HandleUpgrade()
    {
        mineShaftWorkers = GetAllMineShaftWorkers();
        var allMinersMaxed = false;

        foreach (var miner in mineShaftWorkers)
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

            miner.SetWalkingSpeed(miner.GetWalkingSpeed() * multiplier * mineshaftFloor);
            miner.SetCap(Mathf.RoundToInt(miner.GetCapacity() * multiplier * mineshaftFloor));
            miner.SetTimeToLoad(miner.GetTimeToLoad() - multiplier * mineshaftFloor / 3);
            miner.SetTimeToUnload(miner.GetTimeToUnload() - multiplier * mineshaftFloor / 3);
            miner.AddUpdateCount();
            allMinersMaxed = false;
        }

        if (allMinersMaxed)
        {
            AddNewMiner();
        }

        elevatorShaft.SetNewMaxCapacity(Mathf.RoundToInt(elevatorShaft.GetMaxCapacity() * 1.7f));
        elevatorShaft.SetContainerCapacityText();

        var newUpgradeCost = GameCore.Instance.Data.GetNewUpgradeCost(upgradeButton.upgradeCost, mineshaftFloor);
        upgradeButton.SetNewUpgradeCost(newUpgradeCost);
    }

    private void AddNewMiner()
    {
        var newMiner = Instantiate(minerPrefab, transform.position, transform.rotation, transform);
        if (shaftManager.managerBought)
        {
            newMiner.active = true;
            newMiner.hasManager = true;
        }
    }
}
using System;
using UnityEngine;

public class MineShaft : MonoBehaviour
{
    public MineContainer container;
    public MineContainer elevatorShaft;
    public ContainerBase endOfMine;
    public bool isFirstMineshaft = true;

    [SerializeField] private Miner minerPrefab;

    [SerializeField] private int mineshaftFloor;

    private Miner[] mineShaftWorkers;
    private float multiplier;

    [SerializeField] public Manager shaftManager;

    public UpgradeButton upgradeButton;

    private void Start()
    {
        if (isFirstMineshaft)
        {
            upgradeButton.upgradeCost = Mathf.RoundToInt(GameCore.Instance.Data.BasicMineshaftUpgradeCost);
        }

        upgradeButton.OnUpgraded += HandleUpgrade;
        mineShaftWorkers = GetComponentsInChildren<Miner>();
        shaftManager.OnManagerBought += HandleManagerBought;
        shaftManager.OnManagerActivated += HandleManagerActivated;
        elevatorShaft.SetContainerCapacityText();
        multiplier = GameCore.Instance.Data.BoughtUpgradeMultiplier;
        mineshaftFloor = GameCore.Instance.mineShaftList.Count;
        shaftManager.managerCost = GameCore.Instance.Data.GetManagerCost(mineshaftFloor);
    }

    private void HandleManagerActivated()
    {
        throw new NotImplementedException();
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

            miner.walkingSpeed = miner.walkingSpeed * multiplier * mineshaftFloor;
            miner.capacity = Mathf.RoundToInt(miner.capacity * multiplier * mineshaftFloor);
            miner.timeToLoad = miner.timeToLoad - multiplier * mineshaftFloor / 3;
            miner.timeToUnload = miner.timeToUnload - multiplier * mineshaftFloor / 3;
            miner.timesUpdated++;
            allMinersMaxed = false;
        }

        if (allMinersMaxed)
        {
            AddNewMiner();
        }

        elevatorShaft.maxCapacity = Mathf.RoundToInt(elevatorShaft.maxCapacity * 1.7f);
        elevatorShaft.SetContainerCapacityText();

        var newUpgradeCost = GameCore.Instance.Data.GetNewUpgradeCost(upgradeButton.upgradeCost, mineshaftFloor);
        upgradeButton.upgradeCost = newUpgradeCost;
    }

    private void AddNewMiner()
    {
        var newMiner = Instantiate(minerPrefab, transform.position, transform.rotation, transform);
        if (shaftManager.managerBought)
        {
            newMiner.active = true;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineShaft : MonoBehaviour
{
    [SerializeField]
    private Miner minerPrefab;
    public MineContainer container;
    public UpgradeButton upgradeButton;
    //[SerializeField]
    //private int upgradeCost;
    private Miner[] mineShaftWorkers;
    [SerializeField]
    private int mineshaftFloor;
    private float multiplier;
    public ContainerBase endOfMine;
    public MineContainer elevatorShaft;
    [SerializeField]
    public Manager shaftManager;

    private void Start()
    {
        upgradeButton.OnUpgraded += HandleUpgrade;
        mineShaftWorkers = GetComponentsInChildren<Miner>();
        upgradeButton.upgradeCost = Mathf.RoundToInt(GameCore.Instance.Data.basicMineshaftUpgradeCost);
        //upgradeButton.upgradeCost = upgradeCost;
        shaftManager.OnManagerBought += HandleManagerBought;
        shaftManager.OnManagerActivated += HandleManagerActivated;
        elevatorShaft.SetContainerCapacityText();
        multiplier = GameCore.Instance.Data.boughtUpgradeMultiplier;
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
        bool allMinersMaxed = false;

        foreach (var miner in mineShaftWorkers)
        {
            if (miner.timesUpdated == 3)
            {
                allMinersMaxed = true;
                continue;
            }

            if (miner.timesUpdated <= 3)
            {
                miner.walkingSpeed = miner.walkingSpeed * multiplier * mineshaftFloor;
                miner.capacity = (int)Mathf.RoundToInt(miner.capacity * multiplier * mineshaftFloor);
                miner.timeToLoad = miner.timeToLoad - ((multiplier * mineshaftFloor)/3);
                miner.timeToUnload = miner.timeToUnload - ((multiplier * mineshaftFloor) / 3);
                miner.timesUpdated++;
                allMinersMaxed = false;
            }
        }

        if (allMinersMaxed == true)
        {
            //GameCore.Instance.AddNewMiner(gameObject, minerPrefab);
            AddNewMiner();
        }

        //int newUpgradeCost = (int)Mathf.RoundToInt(upgradeCost * multiplier * upgradeLevel);
        int newUpgradeCost = GameCore.Instance.Data.GetNewUpgradeCost(upgradeButton.upgradeCost, mineshaftFloor);
        upgradeButton.upgradeCost = newUpgradeCost;
        //upgradeCost = newUpgradeCost;
    }

    private void AddNewMiner()
    {
        var newMiner = Instantiate(minerPrefab, transform.position, transform.rotation, transform);
        if (shaftManager.managerBought == true)
        {
            newMiner.active = true;
        }
    }
}
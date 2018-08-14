using System;
using System.Collections;
using UnityEngine;

public class MineShaft : MonoBehaviour
{
    [SerializeField] private Miner minerPrefab;
    [SerializeField] private int mineshaftFloor;

    public MineContainer container;
    public MineContainer elevatorShaft;
    public ContainerBase endOfMine;
    public Manager shaftManager;
    public UpgradeButton upgradeButton;
    public bool isFirstMineshaft = true;

    private Miner[] mineShaftWorkers;
    private float multiplier;

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
        mineshaftFloor = GameCore.Instance.GetAmountOfMineshafts();
        shaftManager.managerCost = GameCore.Instance.Data.GetManagerCost(mineshaftFloor);
        shaftManager.managerBonusTime = ((mineshaftFloor * 6) / 2) + 7;
    }

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
        shaftManager.spriteRenderer.sprite = shaftManager.inactiveSprite;

        foreach (var miner in mineShaftWorkers)
        {
            miner.capacity = Mathf.RoundToInt(miner.capacity * bonus);
            miner.walkingSpeed = miner.walkingSpeed * bonus;
            miner.timeToLoad = miner.timeToLoad / bonus;
            miner.timeToUnload = miner.timeToUnload / bonus;
        }

        yield return new WaitForSeconds(boostTime);

        shaftManager.activated = false;

        foreach (var miner in mineShaftWorkers)
        {
            miner.capacity = Mathf.RoundToInt(miner.capacity / bonus);
            miner.walkingSpeed = miner.walkingSpeed / bonus;
            miner.timeToLoad = miner.timeToLoad * bonus;
            miner.timeToUnload = miner.timeToUnload * bonus;
        }

        StartCoroutine(StartCoolDownPhase());
    }

    private IEnumerator StartCoolDownPhase()
    {
        shaftManager.isCoolingDown = true;

        yield return new WaitForSecondsRealtime(GameCore.Instance.Data.ManagerCoolDownTime);

        shaftManager.spriteRenderer.sprite = shaftManager.activeSprite;
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

        elevatorShaft.SetNewMaxCapacity(Mathf.RoundToInt(elevatorShaft.maxCapacity * 1.7f));
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
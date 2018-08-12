using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCore : MonoSingleton<GameCore>
{
    [SerializeField]
    private MineShaft mineShaftPrefab;
    [SerializeField]
    public List<UpgradeButton> upgradeButtons;
    public List<Manager> managers;
    public float gapBetweenMineShafts;
    public GameObject parentObject;
    public List<MineShaft> mineShaftList;
    public ElevatorWorker elevatorWorker;
    public DataContainer Data { get; set; }
    public Text moneyInfo;
    private OverdaysWorker[] overdaysWorkers;
    [SerializeField]
    private GameObject overDaysArea;
    [SerializeField]
    private UpgradeButton AddNewMineshaftButton;

    [SerializeField]
    private float basicMineShaftManagerCost;
    [SerializeField]
    private float basicMineshaftUpgradeCost;
    [SerializeField]
    private float basicNewMineshaftCost;
    [SerializeField]
    private float overdaysUpgradeCost;
    [SerializeField]
    private float overdaysManagerCost;
    [SerializeField]
    private float elevatorUpgradeCost;
    [SerializeField]
    private float elevatorManagerCost;
    [SerializeField]
    private float boughtUpgradeMultiplier;
    [SerializeField]
    private float activeManagerMultiplier;

    public void AddNewMineShaft()
    {
        Vector2 lowestShaftPosition = mineShaftList[mineShaftList.Count - 1].transform.position;
        Vector2 newPosition = new Vector2(lowestShaftPosition.x, lowestShaftPosition.y - gapBetweenMineShafts);
        var newShaft = Instantiate(mineShaftPrefab, newPosition, transform.rotation, parentObject.transform);

        mineShaftList.Add(newShaft);
        elevatorWorker.loadingPositions.Add(newShaft.elevatorShaft);
        upgradeButtons.Add(newShaft.upgradeButton);
        managers.Add(newShaft.shaftManager);
        newShaft.shaftManager.OnManagerBought += HandleUpgradeBought;
    }

    //public void AddNewMiner(MineShaft mineShaft, Miner miner)
    //{
    //    Instantiate(miner, new Vector2 (0,0) , transform.rotation, mineShaft.transform);
    //}

    private void Awake()
    {
        Data = new DataContainer();
        overdaysWorkers = GetAllOverdaysWorkers();
        RegisterWorkers();
        RegisterManagers();
        RegisterUpgradeButtons();
        SetPricesAndMultipliers();
        AddNewMineshaftButton.OnUpgraded += AddNewMineShaft;
        AddNewMineshaftButton.upgradeCost = Mathf.RoundToInt(basicNewMineshaftCost);
    }

    private void SetPricesAndMultipliers()
    {
        Data.activeManagerMultiplier = activeManagerMultiplier;
        Data.boughtUpgradeMultiplier = boughtUpgradeMultiplier;

        Data.elevatorManagerCost = elevatorManagerCost;
        Data.elevatorUpgradeCost = elevatorUpgradeCost;

        Data.basicMineShaftManagerCost = basicMineShaftManagerCost;
        Data.basicMineshaftUpgradeCost = basicMineshaftUpgradeCost;

        Data.overdaysManagerCost = overdaysManagerCost;
        Data.overdaysUpgradeCost = overdaysUpgradeCost;

        Data.basicNewMineshaftCost = basicNewMineshaftCost;
    }

    private void RegisterUpgradeButtons()
    {
        foreach (var upgradeButton in upgradeButtons)
        {
            upgradeButton.OnUpgraded += HandleUpgradeBought;
        }
    }

    private void RegisterManagers()
    {
        foreach (var manager in managers)
        {
            manager.OnManagerBought += HandleUpgradeBought;
        }
    }

    private void HandleUpgradeBought()
    {
        foreach (var manager in managers)
        {
            if (!manager.managerBought)
            {
                manager.CheckIfManagerAvailable(Data.earnedMoney);
            }
        }

        moneyInfo.text = ("Money earned: " + Data.earnedMoney);
    }

    private void RegisterWorkers()
    {
        foreach (var worker in overdaysWorkers)
        {
            worker.OnMoneyEarned += HandleMoneyIncome;
        }
    }

    private OverdaysWorker[] GetAllOverdaysWorkers()
    {
        return overDaysArea.GetComponentsInChildren<OverdaysWorker>();
    }

    //public void UpdateMoneyUI()
    //{
    //    moneyInfo.text = ("Money earned: " + GameCore.Instance.Data.earnedMoney);
    //}

    private void HandleMoneyIncome(int income)
    {
        Data.earnedMoney += income;
        moneyInfo.text = ("Money earned: " + Data.earnedMoney);
        foreach(var button in upgradeButtons)
        {
            button.CheckIfUpgradeAvailable(Data.earnedMoney);
        }
        foreach (var manager in managers)
        {
            if (!manager.managerBought)
            {
                manager.CheckIfManagerAvailable(Data.earnedMoney);
            }
        }
    }
}

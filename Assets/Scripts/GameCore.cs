using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCore : MonoSingleton<GameCore>
{
    [SerializeField] private float activeManagerMultiplier;

    [SerializeField] private UpgradeButton AddNewMineshaftButton;

    [SerializeField] private float basicMineShaftManagerCost;

    [SerializeField] private float basicMineshaftUpgradeCost;

    [SerializeField] private float basicNewMineshaftCost;

    [SerializeField] private float boughtUpgradeMultiplier;

    [SerializeField] private float elevatorManagerCost;

    [SerializeField] private float elevatorUpgradeCost;

    public ElevatorWorker elevatorWorker;
    public float gapBetweenMineShafts;
    public List<Manager> managers;
    public List<MineShaft> mineShaftList;

    [SerializeField] private MineShaft mineShaftPrefab;

    public Text moneyInfo;

    [SerializeField] private GameObject overDaysArea;

    [SerializeField] private float overdaysManagerCost;

    [SerializeField] private float overdaysUpgradeCost;

    private OverdaysWorker[] overdaysWorkers;
    public GameObject parentObject;

    [SerializeField] public List<UpgradeButton> upgradeButtons;

    public DataContainer Data { get; set; }

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

    public void AddNewMineShaft()
    {
        Vector2 lowestShaftPosition = mineShaftList[mineShaftList.Count - 1].transform.position;
        var newPosition = new Vector2(lowestShaftPosition.x, lowestShaftPosition.y - gapBetweenMineShafts);
        var newShaft = Instantiate(mineShaftPrefab, newPosition, transform.rotation, parentObject.transform);

        mineShaftList.Add(newShaft);
        elevatorWorker.loadingPositions.Add(newShaft.elevatorShaft);
        upgradeButtons.Add(newShaft.upgradeButton);
        managers.Add(newShaft.shaftManager);
        newShaft.shaftManager.OnManagerBought += HandleUpgradeBought;
        newShaft.isFirstMineshaft = false;
        int lastShaftUpgradeCost = mineShaftList[mineShaftList.Count - 2].upgradeButton.upgradeCost;
        newShaft.upgradeButton.upgradeCost = Data.GetNewUpgradeCost(lastShaftUpgradeCost, mineShaftList.Count);
    }

    private void SetPricesAndMultipliers()
    {
        Data.ActiveManagerMultiplier = activeManagerMultiplier;
        Data.BoughtUpgradeMultiplier = boughtUpgradeMultiplier;

        Data.ElevatorManagerCost = elevatorManagerCost;
        Data.ElevatorUpgradeCost = elevatorUpgradeCost;

        Data.BasicMineShaftManagerCost = basicMineShaftManagerCost;
        Data.BasicMineshaftUpgradeCost = basicMineshaftUpgradeCost;

        Data.OverdaysManagerCost = overdaysManagerCost;
        Data.OverdaysUpgradeCost = overdaysUpgradeCost;

        Data.BasicNewMineshaftCost = basicNewMineshaftCost;
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
        CheckIfManagerAvailable();
        CheckIfUpgradeAvailable();

        moneyInfo.text = "Money earned: " + Data.EarnedMoney;
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

    private void HandleMoneyIncome(int income)
    {
        Data.EarnedMoney += income;
        moneyInfo.text = "Money earned: " + Data.EarnedMoney;
        CheckIfUpgradeAvailable();
        CheckIfManagerAvailable();
    }

    public void CheckIfUpgradeAvailable()
    {
        foreach (var upgradeButton in upgradeButtons)
        {
            if (upgradeButton.upgradeCost <= Data.EarnedMoney)
            {
                upgradeButton.upgradeCanBeBought = true;
                upgradeButton.spriteR.sprite = upgradeButton.upgradeAvailable;
            }
            else
            {
                upgradeButton.upgradeCanBeBought = false;
                upgradeButton.spriteR.sprite = upgradeButton.upgradeUnavailable;
            }
        }
    }

    public void CheckIfManagerAvailable()
    {
        foreach (var manager in managers)
        {
            if (manager.managerCost <= Data.EarnedMoney)
            {
                manager.managerCanBeBought = true;
                manager.spriteRenderer.sprite = manager.managerAvailable;
            }
            else
            {
                manager.managerCanBeBought = false;
                manager.spriteRenderer.sprite = manager.managerUnavailable;
            }
        }
    }
}
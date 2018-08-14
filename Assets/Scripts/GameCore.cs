using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCore : MonoSingleton<GameCore>
{
    #region EDITOR
    [SerializeField] private UpgradeButton AddNewMineshaftButton;

    [SerializeField] private MineShaft mineShaftPrefab;
    [SerializeField] private GameObject overDaysArea;
    [SerializeField] private float activeManagerMultiplier;
    [SerializeField] private float basicMineShaftManagerCost;
    [SerializeField] private float basicMineshaftUpgradeCost;
    [SerializeField] private float basicNewMineshaftCost;
    [SerializeField] private float boughtUpgradeMultiplier;
    [SerializeField] private float elevatorManagerCost;
    [SerializeField] private float elevatorUpgradeCost;
    [SerializeField] private float globalManagerCoolDownTime;
    [SerializeField] private float newShaftValueMultiplier;
    [SerializeField] private float overdaysManagerCost;
    [SerializeField] private float overdaysUpgradeCost;

    [SerializeField] public List<UpgradeButton> upgradeButtons;

    private OverdaysWorker[] overdaysWorkers;

    public ElevatorWorker elevatorWorker;
    public float gapBetweenMineShafts;
    public List<Manager> managers;
    public List<MineShaft> mineShaftList;
    public Text moneyInfo;
    public GameObject parentObject;
    #endregion

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

        var lastShaftUpgradeCost = mineShaftList[mineShaftList.Count - 2].upgradeButton.upgradeCost;
        var newShaftMultiplier = Instance.Data.NewShaftValueMultiplier;

        elevatorWorker.loadingPositions.Add(newShaft.elevatorShaft);
        upgradeButtons.Add(newShaft.upgradeButton);
        managers.Add(newShaft.shaftManager);

        newShaft.shaftManager.OnManagerBought += HandleUpgradeBought;
        newShaft.isFirstMineshaft = false;
        newShaft.upgradeButton.upgradeCost = Data.GetNewUpgradeCost(lastShaftUpgradeCost, mineShaftList.Count);
        newShaft.elevatorShaft.SetNewMaxCapacity(Mathf.RoundToInt(newShaft.elevatorShaft.maxCapacity * newShaftMultiplier));

        CheckIfManagerAvailable();
        CheckIfUpgradeAvailable();
    }

    private void SetPricesAndMultipliers()
    {
        Data.ActiveManagerMultiplier = activeManagerMultiplier;
        Data.BoughtUpgradeMultiplier = boughtUpgradeMultiplier;

        Data.ElevatorManagerCost = elevatorManagerCost;
        Data.ElevatorUpgradeCost = elevatorUpgradeCost;
        Data.ManagerCoolDownTime = globalManagerCoolDownTime;

        Data.BasicMineShaftManagerCost = basicMineShaftManagerCost;
        Data.BasicMineshaftUpgradeCost = basicMineshaftUpgradeCost;

        Data.OverdaysManagerCost = overdaysManagerCost;
        Data.OverdaysUpgradeCost = overdaysUpgradeCost;

        Data.BasicNewMineshaftCost = basicNewMineshaftCost;
        Data.NewShaftValueMultiplier = newShaftValueMultiplier;
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
            if (manager.managerBought)
            {
                continue;
            }

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
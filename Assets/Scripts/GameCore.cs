using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCore : MonoSingleton<GameCore>
{
    #region EDITOR
    [SerializeField] private UpgradeButton AddNewMineshaftButton;
    [SerializeField] private List<UpgradeButton> upgradeButtons;
    [SerializeField] private List<Manager> managers;
    [SerializeField] private MineShaft mineShaftPrefab;
    [SerializeField] private GameView gameView;
    [SerializeField] private ElevatorWorker elevatorWorker;
    [SerializeField] private GameObject overDaysArea;
    [SerializeField] private GameObject parentObject;
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
    [SerializeField] private float gapBetweenMineShafts;
    #endregion

    private OverdaysWorker[] overdaysWorkers;
    private List<MineShaft> mineShaftList;

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

    private void AddNewMineShaft()
    {
        Vector2 lowestShaftPosition = mineShaftList[mineShaftList.Count - 1].transform.position;
        var newPosition = new Vector2(lowestShaftPosition.x, lowestShaftPosition.y - gapBetweenMineShafts);
        var newShaft = Instantiate(mineShaftPrefab, newPosition, transform.rotation, parentObject.transform);

        mineShaftList.Add(newShaft);

        var lastShaftUpgradeCost = mineShaftList[mineShaftList.Count - 2].upgradeButton.upgradeCost;
        var newShaftMultiplier = Instance.Data.NewShaftValueMultiplier;

        elevatorWorker.AddNewLoadingPositions(newShaft.elevatorShaft);
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
        Data.SetActiveManagerMultiplier(activeManagerMultiplier);
        Data.SetBoughtUpgradeMultiplier(boughtUpgradeMultiplier);

        Data.SetElevatorManagerCost(elevatorManagerCost);
        Data.SetElevatorUpgradeCost(elevatorUpgradeCost);
        Data.SetManagerCoolDownTime(globalManagerCoolDownTime);

        Data.SetBasicMineshaftManagerCost(basicMineShaftManagerCost);
        Data.SetBasicMineshaftUpgradeCost(basicMineshaftUpgradeCost);

        Data.SetOverdaysManagerCost(overdaysManagerCost);
        Data.SetOverdaysUpgradeCost(overdaysUpgradeCost);

        Data.SetBasicNewMineshaftCost(basicNewMineshaftCost);
        Data.SetNewShaftValueMultiplier(newShaftValueMultiplier);
    }

    private void RegisterUpgradeButtons()
    {
        foreach (var upgradeButton in upgradeButtons)
        {
            upgradeButton.OnUpgraded += HandleUpgradeBought;
        }
    }

    public int GetAmountOfMineshafts()
    {
        return mineShaftList.Count;
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

        gameView.UpgradeMoneyTextField(Data.EarnedMoney);
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
        gameView.UpgradeMoneyTextField(Data.EarnedMoney);
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
                manager.SwitchSpriteToAvailable();
            }
            else
            {
                manager.managerCanBeBought = false;
                manager.SwitchSpriteToUnavailable();
            }
        }
    }
}
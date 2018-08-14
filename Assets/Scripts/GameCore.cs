using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCore : MonoSingleton<GameCore>
{
    #region EDITOR
    [SerializeField] private UpgradeButton AddNewMineshaftButton;
    [SerializeField] private List<UpgradeButton> upgradeButtons;
    [SerializeField] private List<Manager> managers;
    [SerializeField] private List<MineShaft> mineShaftList;
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
        AddNewMineshaftButton.SetNewUpgradeCost( Mathf.RoundToInt(basicNewMineshaftCost));
    }

    private void AddNewMineShaft()
    {
        Vector2 lowestShaftPosition = mineShaftList[mineShaftList.Count - 1].transform.position;
        var newPosition = new Vector2(lowestShaftPosition.x, lowestShaftPosition.y - gapBetweenMineShafts);
        var newShaft = Instantiate(mineShaftPrefab, newPosition, transform.rotation, parentObject.transform);
        var newShaftManager = newShaft.GetShaftManager();
        var newUpgradeButton = newShaft.GetMineshaftUpgradeButton();
        var newShaftElevatorShaft = newShaft.GetElevatorShaftContainer();

        mineShaftList.Add(newShaft);

        var lastShaftUpgradeCost = mineShaftList[mineShaftList.Count - 2].GetMineshaftUpgradeButton().upgradeCost;
        var newShaftMultiplier = Instance.Data.NewShaftValueMultiplier;

        elevatorWorker.AddNewLoadingPositions(newShaft.GetElevatorShaftContainer());
        upgradeButtons.Add(newUpgradeButton);
        managers.Add(newShaftManager);

        newShaftManager.OnManagerBought += HandleUpgradeBought;
        newShaft.isFirstMineshaft = false;
        newUpgradeButton.SetNewUpgradeCost(Data.GetNewUpgradeCost(lastShaftUpgradeCost, mineShaftList.Count));
        newShaftElevatorShaft.SetNewMaxCapacity(Mathf.RoundToInt(newShaftElevatorShaft.GetMaxCapacity() * newShaftMultiplier));

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
                upgradeButton.SetUpgradeAvailable(true);
            }
            else
            {
                upgradeButton.upgradeCanBeBought = false;
                upgradeButton.SetUpgradeAvailable(false);
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
                manager.SetManagerSpriteAvailable(true);
            }
            else
            {
                manager.managerCanBeBought = false;
                manager.SetManagerSpriteAvailable(false);
            }
        }
    }
}
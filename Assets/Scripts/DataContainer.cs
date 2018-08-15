using System.Collections.Generic;
using UnityEngine;

public class DataContainer
{
    public int EarnedMoney { get; set; }
    public float BasicMineShaftManagerCost { get; private set; }
    public float OverdaysUpgradeCost { get; private set; }
    public float OverdaysManagerCost { get; private set; }
    public float ElevatorUpgradeCost { get; private set; }
    public float ElevatorManagerCost { get; private set; }
    public float BoughtUpgradeMultiplier { get; private set; }
    public float ActiveManagerMultiplier { get; private set; }
    public float BasicMineshaftUpgradeCost { get; private set; }
    public float BasicNewMineshaftCost { get; private set; }
    public float ManagerCoolDownTime { get; private set; }
    public float NewShaftValueMultiplier { get; private set; }

    #region GETTER AND SETTER
    public int GetNewUpgradeCost(int oldCost, int upgradeLevel = 1)
    {
        return Mathf.RoundToInt(oldCost * BoughtUpgradeMultiplier * upgradeLevel);
    }

    public int GetManagerCost(int mineShaftFloor)
    {
        return Mathf.RoundToInt(BasicMineShaftManagerCost * (mineShaftFloor * 0.7f));
    }

    protected internal void SetBasicNewMineshaftCost(float cost)
    {
        BasicNewMineshaftCost = cost;
    }

    protected internal void SetNewShaftValueMultiplier(float multiplier)
    {
        NewShaftValueMultiplier = multiplier;
    }

    protected internal void SetManagerCoolDownTime(float time)
    {
        ManagerCoolDownTime = time;
    }

    protected internal void SetBasicMineshaftUpgradeCost(float cost)
    {
        BasicMineshaftUpgradeCost = cost;
    }

    protected internal void SetBoughtUpgradeMultiplier(float multiplier)
    {
        BoughtUpgradeMultiplier = multiplier;
    }

    protected internal void SetElevatorManagerCost(float cost)
    {
        ElevatorManagerCost = cost;
    }

    protected internal void SetActiveManagerMultiplier(float multiplier)
    {
        ActiveManagerMultiplier = multiplier;
    }

    protected internal void SetBasicMineshaftManagerCost(float cost)
    {
        BasicMineShaftManagerCost = cost;
    }

    protected internal void SetOverdaysUpgradeCost(float cost)
    {
        OverdaysUpgradeCost = cost;
    }

    protected internal void SetOverdaysManagerCost(float cost)
    {
        OverdaysManagerCost = cost;
    }

    protected internal void SetElevatorUpgradeCost(float cost)
    {
        ElevatorUpgradeCost = cost;
    }
    #endregion
}

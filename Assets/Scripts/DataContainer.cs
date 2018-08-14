using System.Collections.Generic;
using UnityEngine;

public class DataContainer
{
    public int EarnedMoney { get; set; }

    public float BasicMineShaftManagerCost { get; protected set; }
    public float OverdaysUpgradeCost { get; protected set; }
    public float OverdaysManagerCost { get; protected set; }
    public float ElevatorUpgradeCost { get; protected set; }
    public float ElevatorManagerCost { get; protected set; }
    public float BoughtUpgradeMultiplier { get; protected set; }
    public float ActiveManagerMultiplier { get; protected set; }
    public float BasicMineshaftUpgradeCost { get; protected set; }
    public float BasicNewMineshaftCost { get; protected set; }
    public float ManagerCoolDownTime { get; protected set; }
    public float NewShaftValueMultiplier { get; protected set; }

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
}

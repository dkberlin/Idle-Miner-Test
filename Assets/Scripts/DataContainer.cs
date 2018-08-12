using System.Collections.Generic;
using UnityEngine;

public class DataContainer
{
    public int EarnedMoney { get; set; }

    public float BasicMineShaftManagerCost { get; set; }
    public float OverdaysUpgradeCost { get; set; }
    public float OverdaysManagerCost { get; set; }
    public float ElevatorUpgradeCost { get; set; }
    public float ElevatorManagerCost { get; set; }
    public float BoughtUpgradeMultiplier { get; set; }
    public float ActiveManagerMultiplier { get; set; }
    public float BasicMineshaftUpgradeCost { get; set; }
    public float BasicNewMineshaftCost { get; set; }


   

    public int GetNewUpgradeCost(int oldCost, int upgradeLevel = 1)
    {
        return Mathf.RoundToInt(oldCost * BoughtUpgradeMultiplier * upgradeLevel);
    }

    public int GetManagerCost(int mineShaftFloor)
    {
        return Mathf.RoundToInt(BasicMineShaftManagerCost * (mineShaftFloor * 0.7f));
    }

    public int GetNewMineshaftCost(int mineShaftFloor)
    {
        return Mathf.RoundToInt(BasicNewMineshaftCost * (mineShaftFloor));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataContainer
{
    public int earnedMoney { get; set; }

    public float basicMineShaftManagerCost { get; set; }
    public float overdaysUpgradeCost { get; set; }
    public float overdaysManagerCost { get; set; }
    public float elevatorUpgradeCost { get; set; }
    public float elevatorManagerCost { get; set; }
    public float boughtUpgradeMultiplier { get; set; }
    public float activeManagerMultiplier { get; set; }
    public float basicMineshaftUpgradeCost { get; set; }

    private Dictionary<int, int> mineShaftCosts;

    private void Start()
    {
        mineShaftCosts = new Dictionary<int, int>();
    }

    public int GetNewUpgradeCost(int oldCost, int upgradeLevel = 1)
    {
        return Mathf.RoundToInt(oldCost * boughtUpgradeMultiplier * upgradeLevel);
    }

    public int GetManagerCost(int mineShaftFloor)
    {
        return Mathf.RoundToInt(basicMineShaftManagerCost * (mineShaftFloor * 0.7f));
    }
}
